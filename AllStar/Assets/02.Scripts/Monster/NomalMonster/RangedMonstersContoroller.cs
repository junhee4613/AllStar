using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RangedMonstersContoroller : MonsterController_Base_Move
{
    public GameObject bulletPos;
    protected override void Awake()
    {
        Managers.Pool.MonsterPop("Ranged", this.gameObject);
        base.Awake();
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
    
    
    protected override void AttackStyle()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);
        target_identification = Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, attack_Distance, detection_target);

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
                agent.SetDestination(player.transform.position);
            }

        }

    }
    protected override void AttackStart()
    {
        GameObject test = Managers.Pool.Pop(Managers.DataManager.Datas["Monster_Bullet"] as GameObject);
        test.transform.position = bulletPos.transform.position;
        test.transform.rotation = this.transform.rotation;
    }
    protected override void fsmChanger(BaseState BS)
    {
        base.fsmChanger(BS);

        if (BS == monsterStatus.states["attack"])
        {
            action_start = false;
            fsmChanger(monsterStatus.states["idle"]);
        }
    }
    protected override void MonsterPush()
    {
        /*GameObject temp = Managers.Pool.Pop(Managers.DataManager.Datas["MonsterPop_Dragun"] as GameObject);
        temp.transform.position = pos_init;
        if(gameObject.TryGetComponent<DragunPop>(out DragunPop test))
        {
            test.Monstersummon(this.gameObject, "Ranged", pos_init);
        }
        else
        {
            temp.AddComponent<DragunPop>().Monstersummon(this.gameObject, "Ranged", pos_init);
        }*/
        base.MonsterPush();
        Managers.Pool.MobPush(this.gameObject, "Ranged");
    }
    
}
