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
    public SGBaseShot sGBase;
    public GameObject portal;
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
        if (!sGBase.continuous_attack)
        {
            attack_time += Time.deltaTime;
            if (attack_time >= monsterStatus.attackSpeed)
            {
                sGBase._shooting = false;
                attack_time = 0;
            }
        }
    }
    public override void Status_Init()
    {
        base.Status_Init();
        sGBase.attack = false;
    }
    public override void Perceive_player()
    {
        base.Perceive_player();
        sGBase.attack = true;
    }
    protected override void MonsterPush()
    {
        base.MonsterPush();
        Managers.Pool.MobPush(this.gameObject, "SG");
    }
    public override void MonsterDie()
    {
        Debug.Log("#��ž ���� �� ��Ż ����");
        portal.SetActive(true);
        base.MonsterDie();
        MonsterPush();
    }
}
