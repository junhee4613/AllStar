using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController_Base_Move : MonsterBase
{
    Rigidbody rb;
    float init_pos_dis;
    protected NavMeshAgent agent;
    protected Vector3 pos_init;
    [SerializeField]
    protected bool Original_spot = false;
    [Header("플레이어와 장애물 감지")]
    public bool target_identification;
    public LayerMask detection_target;
    [Header("초당 회전하는 각도")]
    public float rotateSpeed = 180f;
    [Header("사거리")]
    public float attack_Distance;
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
            if (!action_start)
            {
                if (monsterStatus.nowState != monsterStatus.states["idle"] && monsterStatus.nowState != monsterStatus.states["attack"])
                {
                    fsmChanger(monsterStatus.states["idle"]);
                }
                action_delay -= Time.deltaTime;
                if(action_delay <= 0)
                {
                    action_start = true;
                    action_delay = action_delay_init;
                }
                return;
            }
            AttackStyle();
        }
        else if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
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
                        Debug.Log(3);
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
    protected virtual void AttackStart()
    {

    }
    protected override void MonsterDie()
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
    protected virtual void AttackStyle()
    {

    }
}
