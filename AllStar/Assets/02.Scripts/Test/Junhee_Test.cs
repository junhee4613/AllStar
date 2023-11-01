using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    public float size;
    public float dis;
    
    public Status monsterStat = new Status();
    public GameObject monster = null;
    private void Awake()
    {
       /* Managers.Pool.MonsterPop("Ranged", monster);
        monster.transform.position = this.transform.position;*/
    }
    private void Update()
    {
        if (Physics.BoxCast(transform.position, Vector3.one * size, transform.forward, Quaternion.identity, dis, 1 << 7))
        {
            Debug.Log("데미지 들어감");
        }
    }

}
