using UnityEngine;
using System.Collections;
using System;
using Gaminho;

[System.Serializable]
public class ShipData {

    public float Velocity;
    public float SpeedRotation;
    public ControlGame controlGame;   
    public Life life;
    public GameObject MotorAnimation;
    public GameObject Shield;
    public GameObject Explosion;
    public Shot[] Shots;

    private Vector3 position;
    private bool shooting;
    private int lifeShield;

    public bool getShooting()
    {
        return shooting;
    }

    public int getLifeShield()
    {
        return lifeShield;
    }

    public Vector3 getStartPos()
    {
        return position;
    }

    public ShipData(ControlShip ship) {

        Velocity = ship.Velocity;
        SpeedRotation = ship.SpeedRotation;
        life = ship.life;
        MotorAnimation = ship.MotorAnimation;
        Shield = ship.Shield;
        Explosion = ship.Explosion;
        Shots = ship.Shots;
        shooting = ship.getShooting();
        lifeShield = ship.getLifeShield();
        position = ship.getStartPos();
    }
}
