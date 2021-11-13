using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaminho;


public class GhostShip : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField] float DelayFollow = .15f;
    [SerializeField] float delayShot = .25f;

    [SerializeField] Transform ShotRef;
    [SerializeField] GameObject MotorAnimation;

    ControlShip m_player;

    bool shooting = false;

    float nextShotTime = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        Statics.GhostPlayer = transform;
        _rb = GetComponent<Rigidbody2D>();

        m_player = Statics.Player.GetComponent<ControlShip>();

    }

    Vector2 smthDampRef_pos;
    Quaternion smthDampRef_rot;

    // Update is called once per frame
    void LateUpdate()
    {

        //Here i just apply a SmoothDump to the GhostPlayer too, for a good effect... i realy like the result..
        if(!shooting && Statics.Player)
        {
            m_player = Statics.Player.GetComponent<ControlShip>();
            StartCoroutine(Shoot());
            shooting = true;
        }

        AnimateMotor();

        _rb.MovePosition(
            Vector2.SmoothDamp(
                transform.position, Statics.Player.position, ref smthDampRef_pos, DelayFollow * Time.deltaTime)
            );

        transform.localRotation = 
            Statics.SmoothDamp(
                transform.localRotation, Statics.Player.localRotation,ref smthDampRef_rot, 2*DelayFollow * Time.deltaTime
             );
    }

    private void AnimateMotor()
    {
        if (MotorAnimation.activeSelf != (Input.GetAxis("Vertical") != 0))
        {
            MotorAnimation.SetActive(Input.GetAxis("Vertical") != 0);
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_player.ActiveProjectile_Delay+delayShot);

            m_player.SetBulletDamage();
                
                BulletInfos goShoot = Instantiate(m_player.ActiveProjectile_Prefab, Vector3.zero, Quaternion.identity).GetComponent<BulletInfos>();
                goShoot.transform.SetParent(transform);
                goShoot.transform.rotation = transform.rotation;

                goShoot.transform.localPosition = ShotRef.localPosition;
                goShoot.GetComponent<Rigidbody2D>().AddForce(transform.up * ((m_player.ActiveProjectile_Speed * 12000f) * Time.deltaTime), ForceMode2D.Impulse);
                goShoot.gameObject.AddComponent<BoxCollider2D>();
                goShoot.transform.SetParent(transform.parent);

                goShoot.caster = transform;
                
            

            
        }
    }
}
