using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonstersContoroller : MonsterController_Base_Move
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
        GameObject test = Managers.Pool.Pop(Managers.DataManager.Datas["여기에 총알"] as GameObject);
        test.transform.rotation = Quaternion.Euler(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        //여기에 원거리 몬스터 공격타입 관련 로직을 넣으면 됨
    }
}
