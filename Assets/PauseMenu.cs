using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    bool paused=false;

    [SerializeField] GameObject PauseMenuObject;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) DOPauseGame();
    }

    void DOPauseGame()
    {
        if (!PauseMenuObject)
        {
            Debug.LogError("PauseMenuObject not defined!");return;
        }
        Time.timeScale = paused ? 1 : 0;
        paused = !paused;

        PauseMenuObject.SetActive(paused);

        if (paused)
        
            GameObject.FindWithTag("GameController").GetComponent<AudioSource>().Pause();
        
        else
            GameObject.FindWithTag("GameController").GetComponent<AudioSource>().UnPause();

         

    }
}