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

    protected override void Awake()
    {
        Managers.Pool.MonsterPop(" CloseRanged", this.gameObject);
        base.Awake();
        an = GetComponent<Animator>();
        monsterStatus.states.SetMonsterFSMDefault(monsterStatus.animator, this.gameObject);
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
        if(an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 && an.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            if(Physics.BoxCast(transform.position, Vector3.one, transform.forward, Quaternion.identity, 1, 1 << 7))
            {
                Managers.GameManager.PlayerStat.GetDamage(monsterStatus.attackDamage);
            }
        }
    }
    protected override void fsmChanger(BaseState BS)
    {
        base.fsmChanger(BS);
    }
    protected override void AttackStyle()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);
        target_identification = Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, attack_Distance, detection_target);
        if (monsterStatus.nowState != monsterStatus.states["attack"])
        {
            agent.SetDestination(player.transform.position); 
        }
        if (dis <= player_dis_run_mode)
        {
            if (monsterStatus.nowState != monsterStatus.states["attack"] && monsterStatus.nowState != monsterStatus.states["run"])
            {
                agent.speed = run_speed;
                fsmChanger(monsterStatus.states["run"]);
            }
            if (target_identification && hit.collider.tag == "Player")
            {
                if (dis <= Mathf.Abs(attack_Distance))
                {
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                    }
                    if (monsterStatus.nowState != monsterStatus.states["attack"])
                    {
                        fsmChanger(monsterStatus.states["attack"]);
                    }
                    else
                    {
                        AttackStart();
                    }
                }
            }
            else
            {
                if (dis <= Mathf.Abs(attack_Distance) && (!target_identification || hit.collider.tag != "Adornment"))
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
                else
                {
                    if (agent.isStopped)
                    {
                        agent.isStopped = false;
                    }
                    if (monsterStatus.nowState != monsterStatus.states["run"] && !Original_spot)
                    {
                        fsmChanger(monsterStatus.states["run"]);
                    }
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
}
