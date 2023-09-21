using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Status monsterStatus;
    public float Detect_Range;
    public float attack_Distance;
    public Collider[] playerSence;
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
    private void Awake()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator, this.gameObject);
        monsterStatus.nowState = monsterStatus.states["idle"];
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region 플레이어 따라가며 공격 로직
    public void Follow()
    {
        playerSence = Physics.OverlapSphere(transform.position, Detect_Range, 128);
        
        if (playerSence.Length == 0)
        {
            if(monsterStatus.nowState != monsterStatus.states["idle"])
            {
                fsmChanger(monsterStatus.states["idle"]);
            }
        }
        else
        {
            foreach (var item in playerSence)
            {
                if (item.name == "Player")
                {
                    
                    gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(item), transform.rotation.z);
                    break;
                }
            }
        }
    }
    public float LookPlayer(Collider hit)
    {

        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        float distanceX = hit.transform.position.x - transform.position.x;
        float distancez = hit.transform.position.z - transform.position.z;

        if (Mathf.Abs(distanceX) < attack_Distance && Mathf.Abs(distancez) < attack_Distance)
        {
            if(monsterStatus.nowState != monsterStatus.states["attack"])
            {
                fsmChanger(monsterStatus.states["attack"]);
            }
        }
        else
        {
            FollowPlayer();
        }

        return target;
    }
    public void FollowPlayer()
    {
        if (monsterStatus.nowState != monsterStatus.states["run"])
        {
            fsmChanger(monsterStatus.states["run"]);
        }
        transform.position += transform.forward * 5/*나중에 5 대신 moveSpeed로 변경*/ * Time.deltaTime;
    }
    #endregion
    public void fsmChanger(BaseState BS)
    {
        if (BS != monsterStatus.nowState)
        {
            monsterStatus.nowState.OnStateExit();
            monsterStatus.nowState = BS;
            monsterStatus.nowState.OnStateEnter();

            if (BS == monsterStatus.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }

    public IEnumerator animTimer()
    {
        yield return null;
        Debug.Log(monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        fsmChanger(monsterStatus.states["idle"]);
    }
}
