using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController_Static : MonsterBase_Static
{
    public Collider[] playerSence;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (monsterStatus.nowState != monsterStatus.states["die"])
        {
            playerSence = Physics.OverlapSphere(transform.position, Detect_Range, 128);
            if (playerSence.Length == 0)
            {
                if (monsterStatus.nowState != monsterStatus.states["idle"])
                {
                    fsmChanger(monsterStatus.states["idle"]);
                }
            }
            else
            {
                foreach (var item in playerSence)
                {
                    if (item.name == "Player")
                    {
                        Follow();
                        break;
                    }
                }
            } 
        }
    }
}
