using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPop : MonoBehaviour
{
    public GameObject monster = null;
    private void Awake()
    {
        Managers.Pool.MonsterPop("SG", monster);
        monster.transform.position = this.transform.position;
    }
}
