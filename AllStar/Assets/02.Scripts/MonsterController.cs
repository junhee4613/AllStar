using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    public Status monsterStat = new Status();
    public void Awake()
    {
        this.gameObject.name = this.gameObject.GetHashCode().ToString();
        Managers.GameManager.monstersInScene.Add(this.gameObject.name, monsterStat);
        Debug.Log(Managers.GameManager.monstersInScene.Values);
        
    }
}
