using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonstersContoroller : MonsterController_Base_Move
{
    public GameObject bulletPos;
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
        Debug.Log("반응");
        GameObject test = Managers.Pool.Pop(Managers.DataManager.Datas["Monster_Bullet"] as GameObject);
        test.transform.position = bulletPos.transform.position;
        test.transform.rotation = this.transform.rotation;
        attack_time = 0;
        //여기에 원거리 몬스터 공격타입 관련 로직을 넣으면 됨
    }
}
