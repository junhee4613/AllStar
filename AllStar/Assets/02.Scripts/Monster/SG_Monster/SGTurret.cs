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
    public GameObject portal;
    protected override void Awake()
    {
        
        base.Awake();
        Debug.Log("#��ž ���� UI ���߿� ����");      //�ΰ��� ������Ʈ�� �־��µ� �ϳ��� ���� �Ҵ�ǰ� �ϳ��� �ȵ��� �׷��� �ϳ��� ����� ���� ����� �Ҵ��� ��������
        

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        Managers.Pool.MonsterPop("SG", gameObject);
        base.Start();
        monsterStatus.hp_bar = GameObject.FindGameObjectWithTag("Monster_hp_bar");
        monsterStatus.hp_bar.SetActive(false);
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
        if(monsterStatus.hp_bar != null && Managers.UI.monster_hp_bar.Item2 != null)
        {
            Managers.UI.monster_hp_bar.Item2.text = monsterStatus.name;
            Managers.UI.monster_hp_bar.Item1.value = (monsterStatus.nowHP / monsterStatus.maxHP) * 100;
        }
        pivotSG[0].SetActive(false);
    }
    protected override void Perceive_player()
    {
        base.Perceive_player();
    }
    protected override void MonsterPush()
    {
        base.MonsterPush();
        Debug.Log("��");
        Managers.Pool.MobPush(this.gameObject, "SG");
    }
    protected override void MonsterDie()
    {
        //Debug.Log("#��ž ���� �� ��Ż ����");
        portal.SetActive(true);
        base.MonsterDie();
    }
}
