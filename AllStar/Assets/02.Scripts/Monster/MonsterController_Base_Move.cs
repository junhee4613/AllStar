using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController_Base_Move : MonsterBase
{
    NavMeshAgent agent;
    public GameObject player = null;
    public bool sense;
    public GameObject pos_init = null;
    public bool Original_spot = false;
    float init_pos_dis;
    public float rotateSpeed = 180f;
    Rigidbody rb;
    /*public float attack_dis;*/
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (chase_player && monsterStatus.nowState != monsterStatus.states["attack"] && Original_spot)
        {
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if (dis <= Mathf.Abs(attack_Distance))
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                sense = Physics.Raycast(transform.position, transform.forward, attack_Distance, 128);
                if (sense)
                {
                    if (monsterStatus.nowState != monsterStatus.states["attack"] && monsterStatus.attackSpeed <= attack_time)
                    {
                        fsmChanger(monsterStatus.states["attack"]);
                    }
                }
                else
                {
                    if(TargetRotation(gameObject.transform, player.transform) >= 0)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
                    }
                    else if(TargetRotation(gameObject.transform, player.transform) < 0)
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

                agent.SetDestination(player.transform.position);
            }
        }
        else if(monsterStatus.nowState != monsterStatus.states["attack"])
        {
            Original_spot = false;
            if(transform.position != pos_init.transform.position && !Original_spot)
            {
                agent.SetDestination(pos_init.transform.position);
                init_pos_dis = Vector3.Distance(transform.position, pos_init.transform.position);
                if (init_pos_dis < Mathf.Abs(1f))
                {
                    Original_spot = true;
                }
            }
        }
    }
    public float LookPlayer(GameObject hit)
    {
        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        return target;
    }

    public float TargetRotation(Transform oneself, Transform other)
    {
        float result = Mathf.Atan2(other.position.z - oneself.position.z, other.position.x - oneself.position.x) * Mathf.Rad2Deg;
        return result;
    }
}
