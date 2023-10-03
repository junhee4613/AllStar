using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController_Base_Move : MonsterBase
{
    NavMeshAgent agent;
    public GameObject player = null;
    public GameObject pos_init = null;
    public bool Original_spot = false;
    float init_pos_dis;
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
                Debug.Log("공격");
                if(!agent.isStopped)
                {
                    agent.isStopped = true;
                    rb.velocity = Vector3.zero;
                }
                //공격 가능한 거리에 닿으면 잠시 움직임을 멈추고 공격한다음 다시 움직이게 하기
                if (monsterStatus.nowState != monsterStatus.states["attack"])
                {
                    fsmChanger(monsterStatus.states["attack"]);
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
            agent.stoppingDistance = 1;
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
}
