using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGTurret : Monster_Static
{
    public enum SG_pattern
    {
        PATTERN1,
        PATTERN2
    }
    public SG_pattern pattern;
    //public GameObject portal;
    public GameObject[] pivotSG;
    protected override void Awake()
    {
        Managers.Pool.MonsterPop("SG", gameObject);
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
    public override void AttackWay()
    {
        base.AttackWay();
        pivotSG[0].SetActive(true);
    }
    protected override void Status_Init()
    {
        base.Status_Init();
        pivotSG[0].SetActive(false);
    }
    protected override void Perceive_player()
    {
        base.Perceive_player();
    }
    protected override void MonsterPush()
    {
        base.MonsterPush();
        Managers.Pool.MobPush(this.gameObject, "SG");
    }
    protected override void MonsterDie()
    {
        //Debug.Log("#Æ÷Å¾ Á×Àº µÚ Æ÷Å» »ý¼º");
        //portal.SetActive(true);
        base.MonsterDie();
    }
}
