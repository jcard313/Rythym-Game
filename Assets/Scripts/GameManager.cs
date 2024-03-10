using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource theMusic;
    public bool startPlaying;
    public BeatScroller theBS;

    public KeyCode keyToPress;

    public static GameManager instance;
    public int currentScore = 0;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentComboMultiplier;
    public int currentCombo;
    public int[] comboMultiplierThresholds;

    public Text scoreText;
    public Text multiText;
    public Text comboText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    // player related variables
    public ResourceBar playerHealthBar;
    public ResourceBar enemyHealthBar;
    public float currentPlayerHealth;
    public float currentEnemyHealth;

    // pressure related variables
    public ResourceBar pressureBar;
    public float currentPlayerPressure;
    public float pressureMultiplier;
    // The threshold for a note to be recognized. Inversely proportional to player pressure to make it harder as the round progresses
    // if note threshold is 1, then all notes are recognized. If note threshold is 0, then only good notes are recognized
    public float noteThreshold;
    public float maxPressure;
    public ThresholdIndicator thresholdIndicator;

    // results related variables
    public GameObject loadNextSceneBUtton;
    public GameObject loadMainMenuButton;
    public GameObject resultsScreen;
    public Text titleText, percentHitText, normalText, goodText, perfectText, missedText, rankText, roundScoreText, killBonusText, totalScoreText;
    public AudioSource resultsMusic;
    public float maxScore;
    public float scoreToBeat;
    public float totalMultiplier;
    public int KILL_BONUS = 5000;

    // countdown variables
    public int countdownTime = 3;
    public Text countdownText;

    // information to be passed down between scenes
    public static int totalScore = 0;
    public static double healthUpgradeMultiplier = 1;
    public static double damageUpgradeMultiplier = 1;
    public static double scoreUpgradeMultiplier = 1;
    public static double pressureUpgradeMultiplier = 1;

    public bool isPaused = false;
    
    [SerializeField] public static int numLevels = 0;
    public static int MAX_LEVELS = 3;

    void Start()
    {
        Debug.Log("Health Upgrade Multiplier: " + healthUpgradeMultiplier);
        Debug.Log("Damage Upgrade Multiplier: " + damageUpgradeMultiplier);
        Debug.Log("Score Upgrade Multiplier: " + scoreUpgradeMultiplier);

        // only one game manager can exist at a time
        instance = this;
        currentComboMultiplier = 1;
        pressureMultiplier = 1;
        currentCombo = 0;
        comboMultiplierThresholds = new int[] {8, 16, 24, 32};

        totalNotes = FindObjectsOfType<NoteObject>().Length;
        for(int i = 0; i< comboMultiplierThresholds.Length; i++) {
            maxScore += comboMultiplierThresholds[i] * scorePerPerfectNote * i+1 * (float)scoreUpgradeMultiplier;
        }
        maxScore += scorePerPerfectNote * comboMultiplierThresholds.Length * (totalNotes-comboMultiplierThresholds.Sum()) * (float)scoreUpgradeMultiplier;
        Debug.Log("maxScore " +  maxScore);
        // at the beginning players will have trouble finishing a level by reducing the enemy health, however this will become a viable strategy with upgrades
        currentEnemyHealth = (float)(maxScore * 1.25);

        maxPressure = (float)(totalNotes * 0.75 * pressureUpgradeMultiplier);
        currentPlayerHealth = (float)(totalNotes * 0.50 * healthUpgradeMultiplier);
        playerHealthBar.SetMaxValue((int)currentPlayerHealth);
        pressureBar.SetMaxValue((int)maxPressure, 0);
        enemyHealthBar.SetMaxValue((int)currentEnemyHealth);

        noteThreshold = 1;

        loadMainMenuButton.SetActive(false);
        loadNextSceneBUtton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying) {
            if (Input.anyKeyDown) {
                StartCoroutine(CountdownToStart());
            }
        } else {
            // game ends (player wins or loses)
            if(((!theMusic.isPlaying && !isPaused) || currentEnemyHealth<=0 || currentPlayerHealth<=0) && !resultsScreen.activeInHierarchy ) {
                
                if(currentEnemyHealth<=0) {
                    currentScore += KILL_BONUS;
                    killBonusText.text = "+5000 kill bonus!!!";
                } else {
                    killBonusText.text = "";
                }

                resultsScreen.SetActive(true);

                normalText.text = "" + normalHits;
                perfectText.text = "" + perfectHits;
                goodText.text = "" + goodHits;
                missedText.text = "" + missedHits;

                float percentScore = currentScore / maxScore * 100f;

                percentHitText.text = percentScore.ToString("F1")+"%";

                string rankVal = "F";

                if (percentScore>40 && percentScore < 55) {
                    rankVal = "D";
                } else if (percentScore<60  && percentScore < 70){
                    rankVal = "C";
                } else if (percentScore<80  && percentScore < 90){
                    rankVal = "B";
                } else if (percentScore<90  && percentScore < 95) {
                    rankVal = "A";
                } else if (percentScore>95) {
                    rankVal = "S";
                }
                
                roundScoreText.text = currentScore.ToString();
                totalScore += currentScore;

                if(rankVal.Equals("F") && currentEnemyHealth > 0) {
                    titleText.text = "GAME OVER (your moves were not good enough to reach the minimum score!)";
                    loadMainMenuButton.SetActive(true);
                } else if (currentPlayerHealth <=0){
                    titleText.text = "GAME OVER (you couldn't take the heat...)";
                    rankText.text = "F";
                    loadMainMenuButton.SetActive(true);
                }
                else {
                    if(numLevels==MAX_LEVELS) {
                        titleText.text = "You have won the Rhythm Rumble, congratulations!";
                        rankText.text = rankVal;
                        loadMainMenuButton.SetActive(true);
                    } else {
                        titleText.text = "Dance Battle Won!";
                        rankText.text = rankVal;
                        loadNextSceneBUtton.SetActive(true);
                        numLevels += 1;  
                    }

                }

                totalScoreText.text = "" + totalScore;
                theMusic.Pause();
                resultsMusic.Play();
            }
        }
    }

    public void NoteHit() {


        Debug.Log("Current Enemy Health: "+ currentEnemyHealth);

        // keeping track of combo
        currentCombo++;
        comboText.text = "Current Combo: " + currentCombo;

        // multiplier logic
        if (currentComboMultiplier-1 < comboMultiplierThresholds.Length && comboMultiplierThresholds[currentComboMultiplier-1] <= currentCombo) {

            currentComboMultiplier++;
        }
        multiText.text = "Multiplier: " + currentComboMultiplier + "x";

        // calculating score
        scoreText.text = "Score: " + currentScore;

        // lowering enemy health
        enemyHealthBar.SetValue((int)currentEnemyHealth);
        updatePressure();

        totalMultiplier = pressureMultiplier * currentComboMultiplier * (float)scoreUpgradeMultiplier;
        Debug.Log("Pressure multiplier: " + pressureMultiplier);

        Debug.Log("Hit on time");
    }

    public void NormalHit() {
        currentScore += (int)(scorePerNote * totalMultiplier);
        currentEnemyHealth -= scorePerNote * currentComboMultiplier * (float)damageUpgradeMultiplier;

        normalHits++;
        NoteHit();
    }
    public void GoodHit() {
        currentScore += (int)(scorePerGoodNote * totalMultiplier);
        currentEnemyHealth -= scorePerGoodNote * currentComboMultiplier * (float)damageUpgradeMultiplier;
        goodHits++;
        NoteHit();
    }
    public void PerfectHit() {
        currentScore += (int)(scorePerPerfectNote * totalMultiplier);
        currentEnemyHealth -= scorePerPerfectNote * currentComboMultiplier * (float)damageUpgradeMultiplier;
        perfectHits++;
        NoteHit();

    }
    public void NoteMissed() {

        // updating combo
        currentCombo = 0;
        comboText.text = "Current Combo: " + currentCombo;
        
        // updating multiplier
        currentComboMultiplier = 1;
        multiText.text = "Multiplier: " + currentComboMultiplier + "x";

        // updating player health
        playerHealthBar.SetValue((int)currentPlayerHealth--);
        // updating player pressure
        updatePressure();

        Debug.Log("Missed note");
        
        missedHits++;
    }

    private void updatePressure() {

        // increasing pressure
        pressureBar.SetValue((int)currentPlayerPressure++);
        pressureMultiplier += 3/maxPressure;
        // note threshold would go down to 50% at most
        noteThreshold = (float)(1- (currentPlayerPressure /maxPressure * 0.5));
        // thresholdIndicator.updateThreshold(noteThreshold);
        thresholdIndicator.updateThreshold(noteThreshold);
    }

    IEnumerator CountdownToStart() {

        while(countdownTime > 0) {
            countdownText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = "Begin!";

        startPlaying = true;
        theBS.hasStarted = true; 

        theMusic.Play();

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }

    public void upgradeHealth() {
        healthUpgradeMultiplier *= 2;
    }
    public void upgradeDamage() {
        damageUpgradeMultiplier *= 1.5;
    }
    public void upgradeScore() {
        scoreUpgradeMultiplier *= 1.25;
    }
    public void upgradePressure() {
        pressureUpgradeMultiplier *= 2;
    }

}