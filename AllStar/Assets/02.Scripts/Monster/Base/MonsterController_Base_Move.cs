using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController_Base_Move : MonsterBase
{
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
        if(die)
        {
            return;
        }
        base.Update();
        if (chase_player)
        {
            Original_spot = false;
            Debug.Log("여기");
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
                Debug.Log("인식");
                sense = Physics.Raycast(transform.position, transform.forward, attack_Distance, 128);
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                if (sense)
                {
                    if (monsterStatus.nowState != monsterStatus.states["attack"])
                    {
                        Debug.Log("공격");
                        AttackStart();
                        fsmChanger(monsterStatus.states["attack"]);
                    }
                }
                else
                {
                    Debug.Log("공격안함");
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
                if(monsterStatus.nowState != monsterStatus.states["run"] && !Original_spot)
                {
                    fsmChanger(monsterStatus.states["run"]);
                }
                agent.SetDestination(player.transform.position);
            }
        }
        else if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
            //여기가 문제임 Original_spot 이게 문제임
            
            if (transform.position != pos_init)
            {
                
                agent.SetDestination(pos_init);
                init_pos_dis = Vector3.Distance(transform.position, pos_init);
                if (init_pos_dis < Mathf.Abs(0.1f))
                {
                    if (monsterStatus.nowState != monsterStatus.states["idle"])
                    {
                        fsmChanger(monsterStatus.states["idle"]);
                    }
                    Original_spot = true;
                }
                else
                {
                    if (monsterStatus.nowState != monsterStatus.states["run"])
                    {
                        fsmChanger(monsterStatus.states["run"]);
                    }
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
        agent.isStopped = true;
        int num1 = Random.Range(1, 100);
        int num2 = Random.Range(1, 100);
        int num3 = Random.Range(1, 100);
        if (num1 == Mathf.Clamp(num1, 1, potionDropProbability))
        {
            GameObject potion = Managers.Pool.Pop(Managers.DataManager.Datas["Potion_Hp_Item"] as GameObject);
            potion.transform.position = transform.position;
        }
        if (num2 == Mathf.Clamp(num2, 1, itemDropProbability))
        {
            GameObject tempArtifact = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);

            tempArtifact.transform.position = transform.position;
        }
        if(num3 == Mathf.Clamp(num3, 1, weaponDropProbability))
        {
            WeaponDropKind();
        }
        base.MonsterDie();
        MonsterPush();
    }
    protected virtual void MonsterPush()
    {
    }
}
