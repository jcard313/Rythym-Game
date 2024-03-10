using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Debug.Log("SceneManager.GetActiveScene().buildIndex+1");
    }
    public void BackToMenu()
    {
        // destroying game manager instance
        Destroy(GameManager.instance);
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}