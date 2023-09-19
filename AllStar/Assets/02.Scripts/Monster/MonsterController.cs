using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Status monsterStatus;
    public MonsterPaattern monster_Motion;
    public float Detect_Range_Free;
    public float Detect_Range_Fix;
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
    public void MonsterPattern()
    {
        switch (monster_Motion)
        {
            case MonsterPaattern.STOP:
                rb.velocity = Vector3.zero;
                break;
            case MonsterPaattern.RUN:
                /*if(Mathf.Abs(rb.velocity.z) < 5 &&  Mathf.Abs(rb.velocity.x) < 5)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) + transform.forward;
                }*/

                break;
            case MonsterPaattern.ATTACK:
                break;
            default:
                break;
        }
    }
    public void Follow()
    {
        test = Physics.OverlapSphere(transform.position, Detect_Range_Free, 128);
        foreach (var item in test)
        {
            Debug.Log("µµ´Â Áß");
            if (item.name == "Player")
            {
                monster_Motion = MonsterPaattern.RUN;
                gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(item), transform.rotation.z);
                break;
            }
            
        }
        if (test.Length == 0)
        {
            monster_Motion = MonsterPaattern.STOP;
        }
    }
    public float LookPlayer(Collider hit)
    {

        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        float distanceX = hit.transform.position.x - transform.position.x;
        float distancez = hit.transform.position.z - transform.position.z;
        if(Mathf.Abs(distanceX) < 1 && Mathf.Abs(distancez) < 1)
        {
            monster_Motion = MonsterPaattern.STOP;
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
}
