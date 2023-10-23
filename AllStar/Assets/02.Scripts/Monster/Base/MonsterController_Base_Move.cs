using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController_Base_Move : MonsterBase
{
    

    public bool ranged = false;
    public NavMeshAgent agent;
    public bool sense;
    public Vector3 pos_init;
    public bool Original_spot = false;
    float init_pos_dis;
    public float rotateSpeed = 180f;
    public float attack_Distance;
    Rigidbody rb;
    public float dropForce;
    [Header("포션 드랍 퍼센트 조절")]
    public int potionDropProbability = 0;
    [Header("아이템(유물) 드랍 퍼센트 조절")]
    public int itemDropProbability = 0;
    [Header("무기 드랍 퍼센트 조절")]
    public int weaponDropProbability = 0;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        pos_init = transform.position;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        if(monsterStatus.nowState == monsterStatus.states["die"])
        {
            return;
        }
        base.Update();
        if (chase_player && monsterStatus.nowState != monsterStatus.states["attack"] && Original_spot)
        {
            if (!action_start)
            {
                action_delay -= Time.deltaTime;
                if(action_delay <= 0)
                {
                    action_start = true;
                    action_delay = action_delay_init;
                }
                return;
            }
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if (dis <= Mathf.Abs(attack_Distance))
            {
                sense = Physics.Raycast(transform.position, transform.forward, attack_Distance, 128);
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                if (sense)
                {
                    if (monsterStatus.nowState != monsterStatus.states["attack"]/* && monsterStatus.attackSpeed <= attack_time*/)
                    {
                        AttackStart();
                        fsmChanger(monsterStatus.states["attack"]);
                    }
                }
                else
                {
                    if (TargetRotation(gameObject.transform, player.transform) >= 0)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
                    }
                    else if (TargetRotation(gameObject.transform, player.transform) < 0)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }
                if(monsterStatus.nowState != monsterStatus.states["run"])
                {
                    fsmChanger(monsterStatus.states["run"]);
                }
                agent.SetDestination(player.transform.position);
            }
        }
        else if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
            Original_spot = false;
            if (transform.position != pos_init && !Original_spot)
            {
                if (monsterStatus.nowState != monsterStatus.states["idle"])
                {
                    fsmChanger(monsterStatus.states["idle"]);
                }
                agent.SetDestination(pos_init);
                init_pos_dis = Vector3.Distance(transform.position, pos_init);
                if (init_pos_dis < Mathf.Abs(1f))
                {
                    Original_spot = true;
                }
            }
        }
    }
    public float TargetRotation(Transform oneself, Transform other)
    {
        float result = Mathf.Atan2(other.position.z - oneself.position.z, other.position.x - oneself.position.x) * Mathf.Rad2Deg;
        return result;
    }
    public virtual void AttackStart()
    {

    }
    public override void MonsterDie()
    {
        Debug.Log("죽음1");
        agent.isStopped = true;
        base.MonsterDie();
        int num1 = Random.Range(1, 100);
        int num2 = Random.Range(1, 100);
        int num3 = Random.Range(1, 100);
        if (num1 == Mathf.Clamp(num1, 1, potionDropProbability))
        {
            GameObject potion = Managers.Pool.Pop(Managers.DataManager.Datas["Potion_Hp_Item"] as GameObject);
            potion.transform.position = transform.position;
            potion.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
        if (num2 == Mathf.Clamp(num2, 1, itemDropProbability))
        {
            GameObject tempArtifact = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
            tempArtifact.transform.position = transform.position;
            tempArtifact.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
        if(num3 == Mathf.Clamp(num3, 1, weaponDropProbability))
        {
            WeaponDropKind();
        }
    }
    
}
