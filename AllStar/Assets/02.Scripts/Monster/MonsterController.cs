using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Status monsterStatus;
    public MonsterPaattern_Base monster_Motion;
    public float Detect_Range_Free;
    public float Detect_Range_Fix;
    public float attack_Distance;
    public Collider[] test;
    Rigidbody rb;
    public void getDamage(float damage)
    {
        if (monsterStatus.HP-damage <=0)
        {
            monsterStatus.HP -= damage;
            monsterStatus.nowState = monsterStatus.states["die"];
        }
        else
        {
            monsterStatus.HP -= damage;
            monsterStatus.nowState = monsterStatus.states["damaged"];
        }
    }
    void Start()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator,this.gameObject);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        MonsterPattern();
    }
    public void MonsterPattern()        //이거 fsm적용시켜야됨
    {
        switch (monster_Motion)
        {
            case MonsterPaattern_Base.STOP:
                rb.velocity = Vector3.zero;
                break;
            case MonsterPaattern_Base.RUN:
                /*if(Mathf.Abs(rb.velocity.z) < 5 &&  Mathf.Abs(rb.velocity.x) < 5)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) + transform.forward;
                }*/

                break;
            case MonsterPaattern_Base.ATTACK:
                break;
            default:
                break;
        }
    }
    #region 플레이어 따라가며 공격 로직
    public void Follow()
    {
        test = Physics.OverlapSphere(transform.position, Detect_Range_Free, 128);
        foreach (var item in test)
        {
            if (item.name == "Player")
            {
                monster_Motion = MonsterPaattern_Base.RUN;
                gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(item), transform.rotation.z);
                break;
            }
            
        }
        if (test.Length == 0)
        {
            monster_Motion = MonsterPaattern_Base.STOP;
        }
    }
    public float LookPlayer(Collider hit)
    {

        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        float distanceX = hit.transform.position.x - transform.position.x;
        float distancez = hit.transform.position.z - transform.position.z;
        if(Mathf.Abs(distanceX) < attack_Distance && Mathf.Abs(distancez) < attack_Distance)
        {
            monster_Motion = MonsterPaattern_Base.STOP;
        }
        else
        {
            transform.position +=  transform.forward * 5 * Time.deltaTime;
        }
        /*if (Mathf.Abs(gameObject.transform.position.z - hit.transform.position.z) < 1
            && Mathf.Abs(hit.transform.position.x - transform.position.x) < 1)
        {
        }*/
        return target;
    }
    #endregion
}
