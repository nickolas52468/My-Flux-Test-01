using Gaminho;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [System.Serializable]
public class GameData {
    public int EnemiesDead;
    public bool WithShield;
    public int CurrentLevel;
    public int Points;
    public int ShootingSelected;

    public GameData(bool shield, int enimiesDead, int currentLevel, int points, int shootingSelected) 
    { 
        EnemiesDead = enimiesDead;
        WithShield = shield;
        CurrentLevel = currentLevel;
        Points = points;
        ShootingSelected = shootingSelected;
    }
}
