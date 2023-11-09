using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public Material test;
    public float test_x;
    public float test_y;
    //public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    [Header("X축 칸 수")]
    public float size_x;
    [Header("Y축 칸 수")]
    public float size_y;
    [Header("Z축 시작지점?")]
    public float size_z;
    [Header("최대 인식거리?")]
    public float dis;

    public Status monsterStat = new Status();
    public GameObject monster = null;
    private void Awake()
    {
        Managers.Pool.MonsterPop("Ranged", monster);
        monster.transform.position = this.transform.position;
    }
    private void Update()
    {
        if (Physics.BoxCast(transform.position, new Vector3(size_x, size_y, size_z), transform.forward, Quaternion.identity, dis, 1 << 7))
        {
            Debug.Log("데미지 들어감");
        }
    }

    /*private void Update()
    {
        test.mainTextureOffset = new Vector2(Time.time * test_x, Time.time * test_y);
    }*/

}
