using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false; 
    public GameObject pauseMenuIU;
    public GameObject MainCamera;    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused) {
                Resume();
            } 
            else {
                Pause();
            }
        }
    }

    void Resume () 
    {
        pauseMenuIU.SetActive(false);
        MainCamera.GetComponent<AudioListener>().enabled = true;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause () 
    {
        pauseMenuIU.SetActive(true);
        MainCamera.GetComponent<AudioListener>().enabled = false;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}