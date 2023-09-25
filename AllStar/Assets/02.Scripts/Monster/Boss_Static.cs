using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonsterBase_Static
{
    public bool pattern_Start_bool = true;              //패턴이 시작하기 위한 불값
    public bool pattern_loop;
    public string motion_Type;                          //랜덤한 패턴을 시작하기 위한 string값
    public int randomNum;
    public BossAttackPattern pattern;
    public bool barrage_start = false;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {                                   //this는 현재 클래스를 나타냄
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
    public void Pattern()       //코루틴으로 하지말고 시간 체크해서 
    {
        pattern_Start_bool = false;
        pattern_loop = true;
        randomNum = Random.Range(0, 5);
        motion_Type = $"BosePattern{randomNum + 1}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
        pattern = (BossAttackPattern)randomNum;

        switch (pattern)             //여긴 공격패턴
        {
            case BossAttackPattern.BARRAGE1: //가만히 있기
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
    #region 보스 패턴들
    IEnumerator BosePattern1()              //탄막패턴
    {

        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern2()              //공격패턴
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴2");

            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern3()              //공격패턴
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator BosePattern4()              //가만히 패턴
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴4");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern5()              //움직이는 패턴
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴5");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BossPattern6()      //가만히 패턴
    {
        AttackAnimator_Run();
        while (pattern_loop)
        {
            Debug.Log("패턴6");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    public void Pattern_Stop()  //이건 패턴이 끝나게 하는 함수
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
