using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeMonstersController : MonsterController_Base_Move
{
    protected override void Awake()
    {
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
    public override void AttackStart()
    {
        Managers.GameManager.PlayerStat.GetDamage(transform.position,monsterStatus.attackDamage);
        //여기에 근거리 몬스터 공격타입 관련 로직을 넣으면 됨
    }
}
