﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaminho;
using System;

public class Enemy : MonoBehaviour {

    public Statics.TYPE_ENEMY MyType;
    public Life _life;
    public GameObject Explosion;
    public int Damage = 1;
    public Shot shot;
    public GameObject ItemDrop;
    public bool dodgeAbility = true;
    public bool normal = true;

    // Use this for initialization
    void Start () {
        if (MyType == Statics.TYPE_ENEMY.SHIP)
        {
            StartCoroutine(Shoot());
        }
    }

    private void Update()
    {
        if (!Statics.Player) return;
        if(MyType == Statics.TYPE_ENEMY.SHIP)
        {
            Quaternion q = Statics.FaceObject(Statics.Player.localPosition, transform.localPosition, Statics.FacingDirection.Right);
            transform.rotation = q;
        }
    }

    private void OnCollisionEnter2D(Collision2D objeto)
    {
        
        if (objeto.gameObject.tag == "Shot")
        {

            Destroy(objeto.gameObject);
            _life.TakesLife(Statics.Damage);
            Instantiate(Explosion, transform).transform.parent = transform.parent;
        }
    }


    public void MyDeath()
    {
        //Explosao;
        if(MyType == Statics.TYPE_ENEMY.METEOR)
        {
            if (UnityEngine.Random.Range(0, 100) > 60)//60% chance of it becoming bits and pieces
            {
                Create(1);
                Create(2);
                Create(3);
                Create(4);
            }
            Statics.EnemiesDead++;
            
        }
        if(ItemDrop != null && UnityEngine.Random.Range(0, 100) > 70)
        {
            Instantiate(ItemDrop, transform.parent).transform.localPosition = transform.localPosition;
        }
        Statics.Points += Damage * 100;
        StartCoroutine(KillMe());
    }

    private IEnumerator KillMe()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private void Create(int v)
    {
        
        GameObject goMunus = Instantiate(gameObject, transform.parent);
        goMunus.GetComponent<Enemy>().MyType = Statics.TYPE_ENEMY.PIECES;
        float scale = UnityEngine.Random.Range(0.2f, 0.6f);
        goMunus.transform.localScale = new Vector3(scale, scale, scale);
        float force = 100f * goMunus.GetComponent<Rigidbody2D>().mass;
        switch (v) {
           case 1: goMunus.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * force, ForceMode2D.Impulse);
                break;
            case 2:
                goMunus.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * force, ForceMode2D.Impulse);
                break;
            case 3:
                goMunus.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * force, ForceMode2D.Impulse);
                break;
            case 4:
                goMunus.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * force, ForceMode2D.Impulse);
                break;
        }
    }


    private IEnumerator Shoot()
    {
        //Pode executar até 3 tiros simultaneos
        while (true)
        {
            yield return new WaitForSeconds(shot.ShootingPeriod);
            
                Statics.Damage = shot.Damage;
                GameObject goShooter = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity);
                goShooter.transform.parent = transform;
                goShooter.transform.localPosition = shot.Weapon.transform.localPosition;
                goShooter.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                goShooter.AddComponent<BoxCollider2D>();
                goShooter.transform.parent = transform.parent;

                if (shot.TypeShooter == Statics.TYPE_SHOT.DOUBLE)
                {
                    GameObject goShooter2 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity);
                    goShooter2.transform.parent = transform;
                    goShooter2.transform.localPosition = shot.Weapon2.transform.localPosition;
                    goShooter2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter2.AddComponent<BoxCollider2D>();
                    goShooter2.transform.parent = transform.parent;
                }

                if (shot.TypeShooter == Statics.TYPE_SHOT.TRIPLE)
                {
                    GameObject goShooter2 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity);
                    goShooter2.transform.parent = transform;
                    goShooter2.transform.localPosition = shot.Weapon2.transform.localPosition;
                    goShooter2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter2.AddComponent<BoxCollider2D>();
                    goShooter2.transform.parent = transform.parent;

                    GameObject goShooter3 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity);
                    goShooter3.transform.parent = transform;
                    goShooter3.transform.localPosition = shot.Weapon3.transform.localPosition;
                    goShooter3.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter3.AddComponent<BoxCollider2D>();
                    goShooter3.transform.parent = transform.parent;
                }
        }
    }

    public void EndBoss()
    {
        GameObject.Find("Control").GetComponent<ControlGame>().LevelPassed();
        Destroy(gameObject);
        Invoke("PStick", 1f);
    }

    void PStick()
    {

        if(Stick.GetStck() == Stick.stck.MANESTIC)
        {
            Update();
        }
    }
}
