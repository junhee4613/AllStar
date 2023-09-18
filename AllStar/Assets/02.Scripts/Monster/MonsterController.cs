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
                Debug.Log("쫒아감");
                //rb.velocity = transform.forward;
                break;
            case MonsterPaattern.ATTACK:
                break;
            default:
                break;
        }
    }
    public void Follow()
    {

        if (Physics.SphereCast(transform.position, Detect_Range_Free, Vector3.one, out RaycastHit hit, Detect_Range_Fix, 128))
        {
            Debug.Log(hit.collider.gameObject.name);
            monster_Motion = MonsterPaattern.RUN;
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(hit), transform.rotation.z);
        }
        else
        {
            monster_Motion = MonsterPaattern.STOP;
        }
    }
    public float LookPlayer(RaycastHit hit)
    {           //-90을 해준다 인스펙터 창에서 전역 기준으로 앞을 볼 때 Y축 회전이 0인데 0.1의 아크탄젠트를 각도로 표현하면 90이 나오기 때문이다.
        float target = Mathf.Atan2(hit.transform.position.z, hit.transform.position.x) * Mathf.Rad2Deg - 90;
        return target;
    }
}
