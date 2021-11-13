using Gaminho;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlGame : MonoBehaviour {
    public ScenarioLimits ScenarioLimit;
    public Level[] Levels;
    public Image Background;
    [Header("UI")]
    public Text TextStart;
    public Text TextPoints;
    public Transform BarLife,BarShield;
    
    // Use this for initialization
    void Start () {
        Statics.EnemiesDead = 0;
        Background.sprite = Levels[Statics.CurrentLevel].Background;
        TextStart.text = "Stage " + (Statics.CurrentLevel + 1);
        GetComponent<AudioSource>().PlayOneShot(Levels[Statics.CurrentLevel].AudioLvl);
        GetComponent<AudioSource>().clip = Levels[Statics.CurrentLevel].AudioLvl;
    }

    private void Update()
    {
        
        

        TextPoints.text = Statics.Points.ToString();
        BarLife.localScale = new Vector3(Statics.Life / 10f, 1, 1);
        BarShield.localScale = new Vector3(Statics.Shield / 10f, 1, 1);

    }

    public void LevelPassed()
    {
        
        Clear();
        Statics.CurrentLevel++;
        Statics.Points += 1000 * Statics.CurrentLevel;

        GameData Stats = new GameData(
                Statics.WithShield,
                Statics.EnemiesDead,
                Statics.CurrentLevel,
                Statics.Points,
                Statics.ShootingSelected
            );

        SaveSystem.SaveGame(Stats);

        if (Statics.CurrentLevel < 3)
        {
            GameObject.Instantiate(Resources.Load(Statics.PREFAB_LEVELUP) as GameObject);
        }
        else
        {
            GameObject.Instantiate(Resources.Load(Statics.PREFAB_CONGRATULATION) as GameObject);
        }
    }
    //Oops, when you lose (: Starts from Zero
    public void GameOver()
    {
        BarLife.localScale = new Vector3(0, 1, 1);
        Clear();
        Destroy(Statics.Player.gameObject);
        Instantiate(Resources.Load(Statics.PREFAB_GAMEOVER) as GameObject);
        Statics.GhostPlayer.gameObject.SetActive(false);

        GameData Stats = new GameData(
                Statics.WithShield, 
                Statics.EnemiesDead, 
                Statics.CurrentLevel, 
                Statics.Points, 
                Statics.ShootingSelected
            );

        SaveSystem.SaveGame(Stats);
    }
    private void Clear()
    {
        GetComponent<AudioSource>().Stop();
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject ini in Enemies)
        {
            Destroy(ini);
        }
    }
}
