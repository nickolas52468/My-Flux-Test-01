using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaminho;
using System;

public class Enemy : MonoBehaviour {
    Collider2D m_col,p_col;Rigidbody2D m_rb;


    public Statics.TYPE_ENEMY MyType;
    public Life _life;
    public GameObject Explosion;
    public int Damage = 1;
    public Shot shot;
    public GameObject ItemDrop;
    public bool dodgeAbility = true;
    public bool normal = true;

    

    [SerializeField] bool dodging = false;
    [SerializeField] float dodgeSpeed = .5f;

    // Use this for initialization
    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
        m_col = GetComponent<Collider2D>();
        p_col = Statics.Player.GetComponent<Collider2D>();

        if (MyType == Statics.TYPE_ENEMY.SHIP)
        {
            //StartCoroutine(Shoot());
        }
    }

    Vector2 lerpref,dodgeDir,destPos;

    RaycastHit2D predictedImpact;

    private void Update()
    {
        if (!Statics.Player) return;
        else
        {
            //garante que tera o colisor do Player deedfinido para usar de referencia abaixo
            if (p_col) p_col = Statics.Player.GetComponent<Collider2D>();
        }
        if(MyType == Statics.TYPE_ENEMY.SHIP )
        {
            Quaternion q = Statics.FaceObject(Statics.Player.localPosition, transform.localPosition, Statics.FacingDirection.Right);
            transform.rotation = q;
        }

        float detectionRange = Vector2.Distance(transform.position,Statics.Player.position);
        float _speedToDodge =  (m_col. bounds.size.x * dodgeSpeed);


        RaycastHit2D[] _shotsOnRange = Physics2D.CircleCastAll
            (transform.position, detectionRange,transform.up)
                .Where(
            s=> s.transform.CompareTag("Shot") 
            && 
            (s.transform.GetComponent<BulletInfos>().caster== Statics.Player 
                || s.transform.GetComponent<BulletInfos>().caster== Statics.GhostPlayer))
                .OrderBy(i=> Vector2.Distance(transform.localPosition,i.transform.localPosition))
            .ToArray();

        if (!normal && _shotsOnRange.Length > 0) {

            normal = true;
            Debug.Log("achou um tiro");
        }

        foreach (var _shot in _shotsOnRange)
        {

            if (dodgeAbility && _shotsOnRange.Length > 0)
            {


                Rigidbody2D mostNearShot_rb = _shot.rigidbody;
                Collider2D mostNearShot_col = _shot.collider;



                if (!dodging)
                    predictedImpact = Physics2D.Raycast(mostNearShot_rb.transform.position + mostNearShot_rb.transform.up * mostNearShot_col.bounds.extents.y,
                    mostNearShot_rb.transform.up);

                RaycastHit2D nearBulletCheck = Physics2D.CircleCast(mostNearShot_rb.transform.position + mostNearShot_rb.transform.up * 5f,
                        5f, mostNearShot_rb.transform.up);


                
                Debug.Log($"on tragetory: {(predictedImpact.transform != null?predictedImpact.transform.name: "nothing")}");
                
                    if (predictedImpact.transform!=null && predictedImpact.transform == transform)
                    {
                        //if (!dodging)
                        //{
                            dodgeDir = transform.InverseTransformPoint(predictedImpact.point);
                            Debug.Log($"{transform.name} na trajetoria do tiro, iniciando Dodge...");

                        //}

                    

                    destPos = transform.position +
                           transform.right * (p_col.bounds.size.x * 2f)
                           +
                           (transform.up * 1.35f)
                           ;




                    
                    destPos = transform.parent.InverseTransformPoint(destPos);
                        

                        dodging = true;

                    }
                    else if (nearBulletCheck.transform == transform)
                    {
                        //if (!dodging)
                        //{
                            dodgeDir = transform.InverseTransformPoint(nearBulletCheck.point);
                            Debug.Log($"{transform.name} na trajetoria do tiro, iniciando Dodge...");

                        //}

                    


                    destPos = transform.position +
                           transform.right * (p_col.bounds.size.x * 2f)
                           +
                           (transform.up * 1.35f)
                           ;


                    destPos = transform.parent.InverseTransformPoint(destPos);
                        //}

                        dodging = true;
                    }
                //}


            }
            else dodging = false;
        }

        if (dodging)
        {
            transform.localPosition =
                            Vector2.SmoothDamp(
                                transform.localPosition,
                                destPos, ref lerpref,
                                _speedToDodge * Time.deltaTime);

            Transform _e = Physics2D.CircleCast(predictedImpact.point, 2f, transform.up).transform;
            dodging = _e != null && _e.transform == transform;

        }
        else predictedImpact = new RaycastHit2D();
    }

    private void OnCollisionEnter2D(Collision2D objeto)
    {
        
        if (objeto.gameObject.tag == "Shot")
        {
            if (MyType == Statics.TYPE_ENEMY.SHIP) Debug.Log("Morri!");
            Destroy(objeto.gameObject);
            _life.TakesLife(Statics.Damage);
            Instantiate(Explosion, transform).transform.SetParent(transform.parent); 
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
                BulletInfos goShooter = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity).GetComponent<BulletInfos>();
                goShooter.transform.SetParent(transform);
            goShooter.transform.localEulerAngles = Vector2.zero;
            goShooter.caster = transform;

            goShooter.transform.transform.rotation = transform.rotation;
            goShooter.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                goShooter.gameObject.AddComponent<BoxCollider2D>();
                goShooter.transform.SetParent(transform.parent);

                if (shot.TypeShooter == Statics.TYPE_SHOT.DOUBLE)
                {
                    BulletInfos goShooter2 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity).GetComponent<BulletInfos>();
                    goShooter2.transform.SetParent(transform);
                goShooter2.transform.rotation = transform.rotation;
                goShooter2.caster = transform;


                goShooter2.transform.localPosition = shot.Weapon2.transform.localPosition;
                    goShooter2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter2.gameObject.AddComponent<BoxCollider2D>();
                goShooter2.transform.SetParent(transform.parent);
            }

                if (shot.TypeShooter == Statics.TYPE_SHOT.TRIPLE)
                {
                    BulletInfos goShooter2 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity).GetComponent<BulletInfos>();
                goShooter2.transform.SetParent(transform);
                goShooter2.transform.rotation = transform.rotation;
                goShooter2.caster = transform;


                goShooter2.transform.localPosition = shot.Weapon2.transform.localPosition;
                    goShooter2.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter2.gameObject.AddComponent<BoxCollider2D>();
                goShooter2.transform.SetParent(transform.parent);


                BulletInfos goShooter3 = Instantiate(shot.Prefab, Vector3.zero, Quaternion.identity).GetComponent<BulletInfos>();
                goShooter3.transform.SetParent(transform);

                goShooter3.transform.rotation = transform.rotation;

                goShooter3.transform.localPosition = shot.Weapon3.transform.localPosition;
                goShooter3.caster = transform;

                goShooter3.GetComponent<Rigidbody2D>().AddForce(transform.up * ((shot.SpeedShooter * 82000f) * Time.deltaTime), ForceMode2D.Impulse);
                    goShooter3.gameObject.AddComponent<BoxCollider2D>();
                goShooter3.transform.SetParent(transform.parent);
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
