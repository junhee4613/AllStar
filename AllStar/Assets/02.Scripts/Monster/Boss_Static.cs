using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonsterBase_Static
{
    public bool pattern_Start_bool = true;              //������ �����ϱ� ���� �Ұ�
    public bool pattern_loop;
    public string motion_Type;                          //������ ������ �����ϱ� ���� string��
    public int randomNum;
    public BossAttackPattern pattern;
    public bool barrage_start = false;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {                                   //this�� ���� Ŭ������ ��Ÿ��
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (monsterStatus.nowState != monsterStatus.states["die"] && monsterStatus.nowState != monsterStatus.states["attack"])
        {
            Follow();
        }
        if (pattern_Start_bool && (monsterStatus.nowState == monsterStatus.states["attack"] || barrage_start))
        {
            Pattern();
        }


    }
    public void Pattern()       //�ڷ�ƾ���� �������� �ð� üũ�ؼ� 
    {
        pattern_Start_bool = false;
        pattern_loop = true;
        randomNum = Random.Range(0, 5);
        motion_Type = $"BosePattern{randomNum + 1}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
        pattern = (BossAttackPattern)randomNum;

        switch (pattern)             //���� ��������
        {
            case BossAttackPattern.BARRAGE1: //������ �ֱ�
                StartCoroutine(motion_Type);
                break;
            case BossAttackPattern.BARRAGE2:
                StartCoroutine(motion_Type);
                break;
            case BossAttackPattern.BARRAGE3:
                StartCoroutine(motion_Type);
                break;
            case BossAttackPattern.SIMPLE_ATTACK:
                StartCoroutine(motion_Type);
                break;
            case BossAttackPattern.SIMPLE_BULLET:
                StartCoroutine(motion_Type);
                break;
            case BossAttackPattern.STOP:
                StartCoroutine(motion_Type);
                break;
            default:
                break;
        }


    }
    #region ���� ���ϵ�
    IEnumerator BosePattern1()              //ź������
    {

        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern2()              //��������
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����2");

            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern3()              //��������
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator BosePattern4()              //������ ����
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����4");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern5()              //�����̴� ����
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����5");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BossPattern6()      //������ ����
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("����6");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    public void Pattern_Stop()  //�̰� ������ ������ �ϴ� �Լ�
    {
        if (monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            pattern_loop = false;
    }
    public void AttackAnimator_Run()
    {
        if (monsterStatus.states.ContainsKey("attack") && monsterStatus.nowState != monsterStatus.states["attack"])
        {
            fsmChanger(monsterStatus.states["attack"]);
        }
    }
    #endregion
}
