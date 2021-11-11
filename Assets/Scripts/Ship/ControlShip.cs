using UnityEngine;
using System.Collections;
using System;
using Gaminho;

public class ControlShip : MonoBehaviour
{
    #region Public

    public float Velocity  = 10f;
    public float SpeedRotation = 200.0f;
    public ControlGame controlGame;   
    public Life life;
    public GameObject MotorAnimation;
    public GameObject Shield;
    public GameObject Explosion;
    public Shot[] Shots;
    public bool copy;
    public float timeRemaining = 60 * Time.deltaTime;
    #endregion

    #region Private
    private Vector3 startPos;
    private bool shooting = false;
    private int lifeShield;
    private ShieldBar shieldBar;

    public bool getShooting() {
        return shooting;
    }

    public int getLifeShield(){
        return lifeShield;
    }

    public Vector3 getStartPos() {
        return startPos;
    }


    #endregion

    void Start()
    {
        if (copy)
        {
            SpeedRotation = 150.0f;
        }
        startPos = transform.localPosition;
        Statics.Player = gameObject.transform;

        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Component required Rigidbody2D");
            Destroy(this);
            return;
        }

        if (GetComponent<BoxCollider2D>() == null)
        {
            Debug.LogWarning("BoxCollider2D not found, adding ...");
            gameObject.AddComponent<BoxCollider2D>();
            
        }
        
        GetComponent<Rigidbody2D>().gravityScale = 0.001f;
        StartCoroutine(Shoot());
    }
    
    void LateUpdate()
    {
        #region Move
        float rotation = Input.GetAxis("Horizontal") * SpeedRotation;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, -rotation);


        if (Input.GetAxis("Vertical") != 0)
        {
            Vector2 translation = Input.GetAxis("Vertical") * new Vector2(0, Velocity * GetComponent<Rigidbody2D>().mass); 
            translation *= Time.deltaTime;
            GetComponent<Rigidbody2D>().AddRelativeForce(translation, ForceMode2D.Impulse);
        }
        AnimateMotor();

        if(transform.localPosition.y > controlGame.ScenarioLimit.yMax || transform.localPosition.y < controlGame.ScenarioLimit.yMin || transform.localPosition.x > controlGame.ScenarioLimit.xMax || transform.localPosition.x < controlGame.ScenarioLimit.xMin)
        {
            Vector3 dir = startPos - transform.localPosition;
            dir = dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * (2 * GetComponent<Rigidbody2D>().mass), ForceMode2D.Impulse);
            
        }
        #endregion

        #region Tiro
        if(copy)
        {
            timeRemaining -= 1;
            if(timeRemaining < 0)
            {
                shooting = true;
            }
            else
            {
                timeRemaining = 60 * Time.deltaTime;
                shooting = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                shooting = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                shooting = false;
            }
        }

        #endregion

        Statics.Life = life.life;
    }

    private void AnimateMotor()
    {
        if(MotorAnimation.activeSelf != (Input.GetAxis("Vertical") != 0))
        {
            MotorAnimation.SetActive(Input.GetAxis("Vertical") != 0);
        }
    }


    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Shots[Statics.ShootingSelected].ShootingPeriod);
            if (shooting)
            {
                Statics.Damage = Shots[Statics.ShootingSelected].Damage;
                GameObject goShoot = Instantiate(Shots[Statics.ShootingSelected].Prefab, Vector3.zero, Quaternion.identity);
                goShoot.transform.parent = transform;
                goShoot.transform.localPosition = Shots[Statics.ShootingSelected].Weapon.transform.localPosition;
                goShoot.GetComponent<Rigidbody2D>().AddForce(transform.up * ((Shots[Statics.ShootingSelected].SpeedShooter * 12000f) * Time.deltaTime), ForceMode2D.Impulse);
                goShoot.AddComponent<BoxCollider2D>();
                goShoot.transform.parent = transform.parent;

                if(Shots[Statics.ShootingSelected].TypeShooter == Statics.TYPE_SHOT.DOUBLE)
                {
                    GameObject goShoot2 = Instantiate(Shots[Statics.ShootingSelected].Prefab, Vector3.zero, Quaternion.identity);
                    goShoot2.transform.parent = transform;
                    goShoot2.transform.localPosition = Shots[Statics.ShootingSelected].Weapon2.transform.localPosition;
                    goShoot2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((Shots[Statics.ShootingSelected].SpeedShooter * 12000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShoot2.AddComponent<BoxCollider2D>();
                    goShoot2.transform.parent = transform.parent;
                }

                if (Shots[Statics.ShootingSelected].TypeShooter == Statics.TYPE_SHOT.TRIPLE)
                {
                    GameObject goShoot2 = Instantiate(Shots[Statics.ShootingSelected].Prefab, Vector3.zero, Quaternion.identity);
                    goShoot2.transform.parent = transform;
                    goShoot2.transform.localPosition = Shots[Statics.ShootingSelected].Weapon2.transform.localPosition;
                    goShoot2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((Shots[Statics.ShootingSelected].SpeedShooter * 12000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShoot2.AddComponent<BoxCollider2D>();
                    goShoot2.transform.parent = transform.parent;

                    GameObject goTiro3 = Instantiate(Shots[Statics.ShootingSelected].Prefab, Vector3.zero, Quaternion.identity);
                    goTiro3.transform.parent = transform;
                    goTiro3.transform.localPosition = Shots[Statics.ShootingSelected].Weapon3.transform.localPosition;
                    goTiro3.GetComponent<Rigidbody2D>().AddForce(transform.up * ((Shots[Statics.ShootingSelected].SpeedShooter * 12000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goTiro3.AddComponent<BoxCollider2D>();
                    goTiro3.transform.parent = transform.parent;
                }
            }

            CallShield();
        }
    }

    private void CallShield()
    {

        if(Shield.activeSelf != Statics.WithShield)
        {
            Shield.SetActive(Statics.WithShield);
        }
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
       
        if (obj.gameObject.tag == "Enemy")
        {
            Instantiate(Explosion, transform);
            
            if (Statics.WithShield)
            {
                lifeShield--;
            }
            else
            {
                life.TakesLife(obj.gameObject.GetComponent<Enemy>().Damage);
            }

        }

        if ( obj.gameObject.tag == "Shot" || obj.gameObject.tag == "EnemyShot")
        {
            Instantiate(Explosion, transform);
 
            if (Statics.WithShield)
            {
                lifeShield--;
            }
            else
            {
                life.TakesLife(1);
            }
            Destroy(obj.gameObject);
        }
        
        if(lifeShield <= 0)
        {
            Statics.WithShield = false;
        }
    }
   
}


