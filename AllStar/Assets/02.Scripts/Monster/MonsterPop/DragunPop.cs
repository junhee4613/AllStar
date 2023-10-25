using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragunPop : MonoBehaviour
{
    public GameObject monster = null;
    private void Awake()
    {
        Managers.Pool.MonsterPop("Ranged", monster);
        monster.transform.position = this.transform.position;
    }
}
