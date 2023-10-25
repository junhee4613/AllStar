using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController_Static : MonsterBase
{

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
        base.Update();
    }
    public override void Perceive_player()
    {
        base.Perceive_player();
        foreach (var item in playerSence)
        {
            if (item.gameObject.tag == "Player")
            {
                Debug.Log("ÃÄ´Ùº½");
                //¿©±â¿¡ Åº¸· ÆÐÅÏ ³Ö¾î¾ßµÊ
                /*gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z);
                if (monsterStatus.nowState != monsterStatus.states["attack"])
                {
                    //fsmChanger(monsterStatus.states["attack"]);
                }*/
                break;
            }
        }
    }
   
}
