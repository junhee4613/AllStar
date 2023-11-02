using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeMonstersController : MonsterController_Base_Move
{
    Animator an;
    [Header("달리기모드로 변할 때의 거리")]
    public float player_dis_run_mode;
    public float run_speed;
    public float walk_speed;
    public bool damage = false;

    protected override void Awake()
    {
        Managers.Pool.MonsterPop(" CloseRanged", this.gameObject);
        monsterStatus.states.SetMonsterFSMDefault(ref monsterStatus.animator, this.gameObject);//여기
        base.Awake();
        an = GetComponent<Animator>();
        Debug.Log(monsterStatus.states["walk"]);
        foreach (var item in monsterStatus.states)
        {
            Debug.Log("여기 " + item);
        }
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void AttackStart()
    {
        if(an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && an.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            if(Physics.BoxCast(transform.position, Vector3.one * 0.1f, transform.forward, Quaternion.identity, 1.5f, 1 << 7) && !damage)
            {
                damage = true;
                Debug.Log("데미지 들감");
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
    protected override void fsmChanger(BaseState BS)
    {
        base.fsmChanger(BS);
        if (BS == monsterStatus.states["walk"] || BS == monsterStatus.states["run"])
        {
            agent.isStopped = false;
        }
        
    }
    protected override void AttackStyle()
    {
        if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
            float dis = Vector3.Distance(transform.position, player.transform.position);
            target_identification = Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, attack_Distance, detection_target);
            agent.SetDestination(player.transform.position);
            if (dis <= player_dis_run_mode)
            {
                if (dis <= Mathf.Abs(attack_Distance))
                {
                    if (target_identification && hit.collider.tag == "Player")
                    {
                        if (dis <= Mathf.Abs(attack_Distance))
                        {
                            if (!agent.isStopped)
                            {
                                agent.isStopped = true;
                            }
                            fsmChanger(monsterStatus.states["attack"]);

                        }
                    }
                    else if (!target_identification || hit.collider.tag != "Adornment")
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (monsterStatus.nowState != monsterStatus.states["run"] && !Original_spot)
                    {
                        Debug.Log(1);
                        agent.speed = run_speed;
                        fsmChanger(monsterStatus.states["run"]);
                    }
                }
            }
            else
            {
                if (monsterStatus.nowState != monsterStatus.states["walk"])
                {
                    agent.speed = walk_speed;
                    fsmChanger(monsterStatus.states["walk"]);
                }
            }
        }
        else
        {
            AttackStart();
        }
    }
}
