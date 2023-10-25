using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    public GameObject monster = null;
    public Status monsterStat = new Status();
    private void Awake()
    {
        Managers.Pool.MobPush(monster, "Ranged");
        Managers.Pool.MonsterPop("Ranged", monster);
    }

}
