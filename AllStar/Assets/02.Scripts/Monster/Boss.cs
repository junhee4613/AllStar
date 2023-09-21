using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool pattern_Start_bool = true;              //패턴이 시작하기 위한 불값
    /*public float standby_time;                          //모든 패턴 중지 시간*/
    public bool pattern_loop;
    public string motion_Type;                          //랜덤한 패턴을 시작하기 위한 string값
    public int randomNum;
    public BossAttackPattern pattern;
    Animator an;
    public Status stat;

    // Start is called before the first frame update
    void Start()
    {                                   //this는 현재 클래스를 나타냄
        stat.states.SetGeneralFSMDefault(ref stat.animator, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (pattern_Start_bool)
        {
            Pattern();
        }
        
        
    }
    public void Pattern()
    {
        pattern_Start_bool = false;         
        randomNum = Random.Range(0, 4);
        motion_Type = $"BosePattern{randomNum + 1}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
        pattern = (BossAttackPattern)randomNum;
        
        switch (pattern)             //여긴 공격패턴
        {
            case (BossAttackPattern)1: //가만히 있기
                StartCoroutine(motion_Type);
                break;
            case (BossAttackPattern)2:
                StartCoroutine(motion_Type);
                break;
            case (BossAttackPattern)3:
                StartCoroutine(motion_Type);
                break;
            case (BossAttackPattern)4:
                StartCoroutine(motion_Type);
                break;
            case (BossAttackPattern)5:
                StartCoroutine(motion_Type);
                break;
            default:
                break;
        }


    }
    #region 보스 패턴들
    IEnumerator BosePattern1()              //공격패턴
    {
        
        AttackAnimator_Run();
        while (pattern_loop)
        {
            gameObject.transform.Rotate(360 * Time.deltaTime, 360 * Time.deltaTime, 360 * Time.deltaTime) ;
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
            gameObject.transform.position += Vector3.one * Time.deltaTime;
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
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern4()              //가만히 패턴
    {
        if (stat.nowState != stat.states["idle"] && stat.states.ContainsKey("idle"))
        {
            fsmChanger(stat.states["idle"]);
        }
        while (pattern_loop)
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern5()              //움직이는 패턴
    {
        if(stat.nowState != stat.states["run"] && stat.states.ContainsKey("run"))
        {
            fsmChanger(stat.states["run"]);
        }
        while (pattern_loop)
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
        pattern_Start_bool = true;
    }
    public void Pattern_Stop()  //이건 패턴이 끝나게 하는 함수
    {
        if (stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            pattern_loop = false;
    }
    public void AttackAnimator_Run()
    {
        if (stat.states.ContainsKey("attack") && stat.nowState != stat.states["attack"])
        {
            fsmChanger(stat.states["attack"]);
        }
    }
    #endregion
    public void fsmChanger(BaseState BS)
    {
        if (BS != stat.nowState)
        {
            stat.nowState.OnStateExit();
            stat.nowState = BS;
            stat.nowState.OnStateEnter();
            
            if(BS == stat.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }
    
    public IEnumerator animTimer()
    {
        yield return null;
        Debug.Log(stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        fsmChanger(stat.states["idle"]);
    }
}
