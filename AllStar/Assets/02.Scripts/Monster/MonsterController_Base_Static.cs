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
            if (item.name == "Player")
            {
                gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(item), transform.rotation.z);
                break;
            }
        }
    }
    public float LookPlayer(Collider hit)
    {
        if (monsterStatus.nowState != monsterStatus.states["run"])
        {
            fsmChanger(monsterStatus.states["run"]);
        }
        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        float distanceX = hit.transform.position.x - transform.position.x;
        float distancez = hit.transform.position.z - transform.position.z;

        if (Mathf.Abs(distanceX) < attack_Distance && Mathf.Abs(distancez) < attack_Distance)
        {
            if (monsterStatus.nowState != monsterStatus.states["attack"])
            {
                fsmChanger(monsterStatus.states["attack"]);
            }
        }
        return target;
    }
}
