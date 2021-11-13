using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfos : MonoBehaviour
{
    /// <summary>
    /// quem sumonou/quem atirou esse bullet 
    /// usado para identificar se é do player ou nao
    /// </summary>
    
    [HideInInspector] public Transform caster;
}
