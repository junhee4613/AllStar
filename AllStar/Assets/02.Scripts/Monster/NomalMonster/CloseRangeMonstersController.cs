using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloseRangeMonstersController : MonsterController_Base_Move
{
    Animator an;
    [Header("달리기모드로 변할 때의 기준거리")]
    public float dis_criteria_run_mode;
    public float run_speed;
    public float walk_speed;
    public bool damage = false;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        Managers.Pool.MonsterPop("CloseRanged", this.gameObject);
        monsterStatus.states.SetMonsterFSMDefault(ref monsterStatus.animator, this.gameObject);//여기
        base.Start();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void AttackStart()               //얘도 공격 모션중에 감지 외로 도망가면 제자리론 안돌아감  action_start = true임
    {
        if(an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && an.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            if(Physics.BoxCast(transform.position, Vector3.one * 0.1f, transform.forward, Quaternion.identity, 1.5f, 1 << 7) && !damage)
            {
                damage = true;
                Managers.GameManager.PlayerStat.GetDamage(monsterStatus.attackDamage);
            }
        }
        else if(an.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            damage = false;
            action_start = false;
            if (monsterStatus.nowState != monsterStatus.states["idle"])
            {
                //나중에 공격대기모드로 바꿔야됨
                fsmChanger(monsterStatus.states["idle"]);
            }
        }
    }
    
    protected override void AttackStyle()
    {
        if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
            float dis = Vector3.Distance(transform.position, player.transform.position);
            target_identification = Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, attack_Distance, detection_target);
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(player.transform.position);
            }
            if (dis <= dis_criteria_run_mode)
            {
                if (dis <= Mathf.Abs(attack_Distance))
                {
                    if (target_identification && hit.collider.tag == "Player")
                    {
                        fsmChanger(monsterStatus.states["attack"]);
                    }
                    else if (!target_identification || hit.collider.tag != "Adornment")
                    {
                        if (monsterStatus.nowState != monsterStatus.states["idle"])//이거 나중에 공격 대기모드로 바꿔야됨
                        {
                            fsmChanger(monsterStatus.states["idle"]);
                        }
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        if (monsterStatus.nowState != monsterStatus.states["run"])
                        {
                            fsmChanger(monsterStatus.states["run"]);
                        }
                        agent.SetDestination(player.transform.position);
                    }
                }
                else
                {
                    if (monsterStatus.nowState != monsterStatus.states["run"])
                    {
                        fsmChanger(monsterStatus.states["run"]);
                    }
                }
            }
            else
            {
                if (monsterStatus.nowState != monsterStatus.states["walk"])
                {
                    fsmChanger(monsterStatus.states["walk"]);
                }
            }
        }
        else
        {
            AttackStart();
        }
    }
    protected override void fsmChanger(BaseState BS)
    {
        base.fsmChanger(BS);
        if (BS == monsterStatus.states["attack"] || BS == monsterStatus.states["idle"])//이거 나중에 공격 대기모드로 바꿔야됨
        {
            /*agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            agent.avoidancePriority = 50;*/
            if (agent.isActiveAndEnabled)
            {
                agent.isStopped = true;
            }
        }
        else if (BS == monsterStatus.states["run"])
        {
            agent.speed = run_speed;
            /*agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.avoidancePriority = 51;*/
            if (agent.isActiveAndEnabled)
            {
                agent.isStopped = false;
            }
        }
        else if(BS == monsterStatus.states["walk"])
        {
            agent.speed = walk_speed;
            /*agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.avoidancePriority = 51;*/
            if (agent.isActiveAndEnabled)
            {
                agent.isStopped = false;
            }
        }
    }
    protected override void MonsterPush()
    {
        Managers.Pool.MobPush(this.gameObject, "CloseRanged");
    }

}
