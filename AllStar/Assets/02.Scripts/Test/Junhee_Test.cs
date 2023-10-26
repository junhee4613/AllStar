using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    
    public Status monsterStat = new Status();
    public GameObject monster = null;
    private void Awake()
    {
        Managers.Pool.MonsterPop("Ranged", monster);
        monster.transform.position = this.transform.position;
    }

}
