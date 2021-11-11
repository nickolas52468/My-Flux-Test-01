﻿using Gaminho;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallScene : MonoBehaviour {


    public void Call(string sname)
    {
        GameObject.Instantiate(Resources.Load(Statics.PREFAB_LOAD) as GameObject);
        SceneManager.LoadScene(sname);
    }
}
