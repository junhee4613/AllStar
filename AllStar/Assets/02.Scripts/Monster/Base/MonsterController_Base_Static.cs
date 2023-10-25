using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController_Static : MonsterBase
{
    public enum SG_pattern 
    {
        PATTERN1,
        PATTERN2
    }
    public SG_pattern pattern;
    public SGBaseShot sGBase;
    [Header("���� ��� �ۼ�Ʈ ����")]
    public int potionDropProbability = 0;
    [Header("������(����) ��� �ۼ�Ʈ ����")]
    public int itemDropProbability = 0;
    [Header("���� ��� �ۼ�Ʈ ����")]
    public int weaponDropProbability = 0;
    public float dropForce;
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
                if(attack_time >= monsterStatus.attackSpeed && !sGBase.attack)
                {
                    sGBase._shooting = false;
                    attack_time = 0;
                    //���� ź�������� �Ÿ��� �־����� �����µ� �̰� ������ ���� ���� �ܿ� ���� �� ź���� �ȳ����� �ؾߵ�
                }
            }
        }
    }
    public override void MonsterDie()
    {
        int num1 = Random.Range(1, 100);
        int num2 = Random.Range(1, 100);
        int num3 = Random.Range(1, 100);
        if (num1 == Mathf.Clamp(num1, 1, potionDropProbability))
        {
            GameObject potion = Managers.Pool.Pop(Managers.DataManager.Datas["Potion_Hp_Item"] as GameObject);
            potion.transform.position = transform.position;
            potion.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
        if (num2 == Mathf.Clamp(num2, 1, itemDropProbability))
        {
            GameObject tempArtifact = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
            tempArtifact.transform.position = transform.position;
            tempArtifact.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
        if (num3 == Mathf.Clamp(num3, 1, weaponDropProbability))
        {
            WeaponDropKind();
        }
        base.MonsterDie();
    }

}
