using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class Life : MonoBehaviour
{
    public int life = 10;
    public UnityEvent RunWhenDies;
    public bool ControlCollision = false;
    public GameObject Explosion;

    public void TakesLife(int Qtd)
    {
       
        life -= Qtd;
       
        if(life <= 0)
        {
            if (RunWhenDies != null)
            {
                RunWhenDies.Invoke();//Chama o Evento anexado se ele existir
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }

   
    //Colisão
    private void OnTriggerEnter2D(Collider2D _object)
    {
        if (!ControlCollision) return;
        
        if(_object.gameObject.tag == "Shot")
        {
            Instantiate(Explosion, transform).transform.parent = transform.parent;
            TakesLife(1);
            Destroy(_object.gameObject);
        }
        if (_object.gameObject.tag == "Player")
        {
            _object.GetComponent<Life>().TakesLife(1);

        }
    }




}
