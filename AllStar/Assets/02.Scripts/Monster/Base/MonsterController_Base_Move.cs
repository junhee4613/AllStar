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
    protected bool Original_spot = true;
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
        if (!action_start)      //지금 드라군이 공격하고 잠시 딜레이 시간에 플레이어가 벗어나면 달리는 상태로 멈춤 -> 이거 수정
        {
            Debug.Log(2);
            if (monsterStatus.nowState != monsterStatus.states["idle"] && monsterStatus.nowState != monsterStatus.states["attack"])
            {
                fsmChanger(monsterStatus.states["idle"]);
            }
            action_delay -= Time.deltaTime;
            if (action_delay <= 0)
            {
                //agent.isStopped = false;
                action_start = true;
                action_delay = action_delay_init;
            }
            return;
        }
        if (chase_player && !Original_spot)       //감지 외로 벗어나면서 아래에 잇는 로직이 안돌아서 생기는 듯?? 수정했다
        {
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
    /*public float TargetRotation(Transform oneself, Transform other)
    {
        float result = Mathf.Atan2(other.position.z - oneself.position.z, other.position.x - oneself.position.x) * Mathf.Rad2Deg;
        return result;
    }*/
    protected virtual void AttackStart()
    {

    }
    protected override void Perceive_player()
    {
        base.Perceive_player();
        Original_spot = false;
    }
    protected override void Status_Init()
    {
        base.Status_Init();
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
    protected override void fsmChanger(BaseState BS)
    {
        base.fsmChanger(BS);
    }
}
