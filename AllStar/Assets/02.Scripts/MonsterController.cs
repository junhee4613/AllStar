using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float hp = 100;
    public Status monsterStat = new Status();
    public void Awake()
    {
        this.gameObject.name = this.gameObject.GetHashCode().ToString();
        Managers.GameManager.monstersInScene.Add(this.gameObject.name, monsterStat);
    }
}
