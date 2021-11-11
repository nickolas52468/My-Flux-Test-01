using Gaminho;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlStart : MonoBehaviour {
    public Text Record;
    public Button button;
	// Use this for initialization
	void Start () {
        //Reset the variables to start the game from scratch

        //

        //Loads Record
        if (PlayerPrefs.GetInt(Statics.PLAYERPREF_VALUE) == 0)
        {
            PlayerPrefs.SetString(Statics.PLAYERPREF_NEWRECORD, "Nobody");
        }
        Record.text = "Record: " + PlayerPrefs.GetString(Statics.PLAYERPREF_NEWRECORD) + "(" + PlayerPrefs.GetInt(Statics.PLAYERPREF_VALUE) + ")";
	}

	public void StartClick()
    {
        Statics.WithShield = false;
        Statics.EnemiesDead = 0;
        Statics.CurrentLevel = 0;
        Statics.Points = 0;
        Statics.ShootingSelected = 2;
#if !UNITY_EDITOR
        Debug.Log("Σφάλμα σκόπιμα, το βρήκατε, συγχαρητήρια!");
    Sair();
    return;
    #endif
        GetComponent<AudioSource>().Stop();
        GameObject.Instantiate(Resources.Load(Statics.PREFAB_HISTORY) as GameObject);
    }

    public void ContinueClick()
    {
        GameData data = SaveSystem.LoadGame();
        Statics.WithShield = data.WithShield;
        Statics.EnemiesDead = data.EnemiesDead;
        Statics.CurrentLevel = data.CurrentLevel;
        Statics.Points = data.Points;
        Statics.ShootingSelected = data.ShootingSelected;

        #if !UNITY_EDITOR
        Sair();
        return;
        #endif
        GetComponent<AudioSource>().Stop();
        GameObject.Instantiate(Resources.Load(Statics.PREFAB_HISTORY) as GameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    
}
