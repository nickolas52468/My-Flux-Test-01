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
    public Transform BarLife;
    
    // Use this for initialization
    void Start () {
        Statics.EnemiesDead = 0;
        Background.sprite = Levels[Statics.CurrentLevel].Background;
        TextStart.text = "Stage " + (Statics.CurrentLevel + 1);
        GetComponent<AudioSource>().PlayOneShot(Levels[Statics.CurrentLevel].AudioLvl);
    }

    private void Update()
    {
        bool witdShield = Statics.WithShield;
        int enemiesDead = Statics.EnemiesDead;
        int currentLevel = Statics.CurrentLevel;
        int points = Statics.Points;
        int shootingSelected = Statics.ShootingSelected;
        GameData data = new GameData(witdShield, enemiesDead, currentLevel, points, shootingSelected);
        SaveSystem.SaveGame(data);

        TextPoints.text = Statics.Points.ToString();
        BarLife.localScale = new Vector3(Statics.Life / 10f, 1, 1);
    }

    public void LevelPassed()
    {
        
        Clear();
        Statics.CurrentLevel++;
        Statics.Points += 1000 * Statics.CurrentLevel;
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
        GameObject.Instantiate(Resources.Load(Statics.PREFAB_GAMEOVER) as GameObject);
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
