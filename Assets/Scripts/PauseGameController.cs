using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{
    [SerializeField] KeyCode pauseKey;
    [SerializeField] GameObject pauseMenu;
    void Update()
    {

        if(Input.GetKeyDown(pauseKey) && !GameManager.instance.resultsScreen.activeInHierarchy){
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            GameManager.instance.theMusic.Pause();
            GameManager.instance.isPaused = true;
        }
        
    }

    public void UnPause() {
        Time.timeScale = 1;
        GameManager.instance.theMusic.Play();
        GameManager.instance.isPaused = false;
    }
}
