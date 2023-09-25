using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase_Static : MonoBehaviour
{
    public Status monsterStatus;
    public float Detect_Range;
    public float attack_Distance;
    GameObject playerLocation = null;
    public void getDamage(float damage)
    {
        if (monsterStatus.HP - damage <= 0)
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
    protected virtual void Awake()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator, this.gameObject);
        monsterStatus.nowState = monsterStatus.states["idle"];
        playerLocation = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (monsterStatus.nowState != monsterStatus.states["die"])
        {
            Follow();
        }
    }
    public void Follow()
    {
        if(playerLocation != null)
        {
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(playerLocation), transform.rotation.z);
        }
    }
    public float LookPlayer(GameObject player)
    {
        float target = Mathf.Atan2(transform.position.z - player.transform.position.z, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        float distanceX = player.transform.position.x - transform.position.x;
        float distancez = player.transform.position.z - transform.position.z;

        if (Mathf.Abs(distanceX) < attack_Distance && Mathf.Abs(distancez) < attack_Distance)
        {
            if (monsterStatus.nowState != monsterStatus.states["attack"])
            {
                fsmChanger(monsterStatus.states["attack"]);
            }
        }

        return target;
    }
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
