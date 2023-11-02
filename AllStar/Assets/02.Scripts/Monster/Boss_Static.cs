using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonoBehaviour
{
    public bool pattern_Start_bool = true;              //패턴이 시작하기 위한 불값
    //pattern_loop는 나중에 없애는 값
    public bool pattern_loop;
    public string motion_Type;                          //랜덤한 패턴을 시작하기 위한 string값
    public int randomNum;
    public Boss_Simple_Pattern simple_pattern;
    private Boss_Hard_Pattern hard_pattern;
    public bool barrage_start = false;
    public float standby_time;
    public float[] standby_time_standard = new float[2] {1f, 2f};
    public GameObject player;
    public GameObject[] barrage_patterns;
    public Status state;
    [Header("현재 hp")]
    public float current_hp;
    public float non_hit_time;
    [Header("안맞아서 힐패턴 나오는 시간 기준")]
    public float heal_pattern_start_time;
    public bool heal_pattern_start;
    [Header("힐량")]
    public float heal_quantity;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        barrage_patterns = GameObject.FindGameObjectsWithTag("Barrage");
    }
    private void Start()
    {                                   //this는 현재 클래스를 나타냄

    }

    // Update is called once per frame
    private void Update()
    {
        if (current_hp == state.nowHP)
        {
            if(!heal_pattern_start)
                non_hit_time += Time.deltaTime;
        }
        else
        {
            current_hp = state.nowHP;
        }

        if (pattern_Start_bool)
        {
            if (standby_time_standard[Random.Range(0, 1)] < standby_time)
                Pattern();
            else
                TargetRotation(transform, player.gameObject.transform);//나중에 보스 모델링 보고 로직 위치나 조건 수정할 가능성 높음

        }
        else
        {
            standby_time += Time.deltaTime;
        }
    }
    public void Pattern()       //코루틴으로 하지말고 시간 체크해서 
    {
        pattern_Start_bool = false;
        pattern_loop = true;

        //나중에 패턴에 하드모드로 변하는 모습을 넣어야되는데 어떻게 처리할지 고민중
        if (state.nowHP < state.maxHP / 10 * 3.5)
        {
            randomNum = Random.Range(0, 3);             
            motion_Type = $"Hard_Pattern{randomNum + 1}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
            hard_pattern = (Boss_Hard_Pattern)randomNum;
            switch (hard_pattern)             
            {
                case Boss_Hard_Pattern.BARRAGE1: 
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Hard_Pattern.BARRAGE2:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Hard_Pattern.BARRAGE3:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Hard_Pattern.LASER:
                    StartCoroutine(motion_Type);
                    break;
                default:
                    break;
            }
        }
        else
        {
            randomNum = Random.Range(0, 4);             //나중에 5로 늘려야됨
            motion_Type = $"Simple_Pattern{randomNum + 1}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
            simple_pattern = (Boss_Simple_Pattern)randomNum;
            switch (simple_pattern)             //여긴 공격패턴(근접 공격 외에)
            {
                case Boss_Simple_Pattern.BARRAGE1: //가만히 있기
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Simple_Pattern.BARRAGE2:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Simple_Pattern.BARRAGE3:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Simple_Pattern.LASER:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Simple_Pattern.FOLLOW_LASER:
                    StartCoroutine(motion_Type);
                    break;
                case Boss_Simple_Pattern.HEAL:
                    StartCoroutine(motion_Type);
                    break;
                default:
                    break;
            }
        }
        
    }
    
    public void fsmChanger(BaseState BS)
    {
        if (BS != state.nowState)
        {
            state.nowState.OnStateExit();
            state.nowState = BS;
            state.nowState.OnStateEnter();
        }
    }
    #region 보스 패턴 HP35퍼 초과일 때
    IEnumerator Simple_Pattern1()              //탄막패턴
    {

        if (state.states.ContainsKey("pattern1") && state.nowState != state.states["pattern1"])
        {
            //fsmChanger(monsterStatus.states["pattern1"]);
        }
        while (pattern_loop)
        {
            Debug.Log("패턴1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern2()              //탄막패턴
    {
        if (state.states.ContainsKey("pattern2") && state.nowState != state.states["pattern2"])
        {
            //fsmChanger(monsterStatus.states["pattern2"]);
        }
        while (pattern_loop)
        {
            Debug.Log("패턴2");

            Pattern_Stop();
        }
        Debug.Log("여기");
        yield return null;

        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern3()              //탄막패턴
    {
        while (pattern_loop)
        {
            Debug.Log("패턴3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern4()              //레이저 짧게 한번 쏘는 패턴 이때는 따라가는 레이저보다 더 빠르게 쏨
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
    IEnumerator Simple_Pattern5()              //레이저 쏘면서 플레이어를 천천히 쳐다보는 패턴
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
    IEnumerator Simple_Pattern6()      //힐패턴
    {
        heal_pattern_start = true;
        non_hit_time = 0;
        fsmChanger(state.states["heal"]);
        while (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            state.nowHP += Time.deltaTime * heal_quantity;
            yield return null;
        }
        heal_pattern_start = false;
        pattern_Start_bool = true;
    }
    public void Pattern_Stop()  
    {
        if (standby_time > 3)
        {
            foreach (var item in barrage_patterns)
            {
                item.SetActive(false);
            }
            pattern_loop = false;//########################나중에 삭제
            standby_time = 0;
        }
    }
    public void AttackAnimator_Run()
    {
        if (state.states.ContainsKey("attack") && state.nowState != state.states["attack"])
        {
            //fsmChanger(monsterStatus.states["attack"]);
        }
    }
    #endregion
    #region 보스 패턴 HP35퍼 이하일 때
    IEnumerator Hard_Pattern1()              //탄막패턴
    {

        if (state.states.ContainsKey("pattern1") && state.nowState != state.states["pattern1"])
        {
            //fsmChanger(monsterStatus.states["pattern1"]);
        }
        while (pattern_loop)
        {
            Debug.Log("패턴1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern2()              //공격패턴
    {
        if (state.states.ContainsKey("pattern2") && state.nowState != state.states["pattern2"])
        {
            //fsmChanger(monsterStatus.states["pattern2"]);
        }
        while (pattern_loop)
        {
            Debug.Log("패턴2");

            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern3()              //공격패턴
    {
        if (state.states.ContainsKey("pattern3") && state.nowState != state.states["pattern3"])
        {
            //fsmChanger(monsterStatus.states["pattern3"]);
        }
        while (pattern_loop)
        {
            Debug.Log("패턴3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern4()              //가만히 패턴
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
    IEnumerator Hard_Pattern5()              //움직이는 패턴
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
    IEnumerator Hard_Pattern6()      //가만히 패턴
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
    #endregion
    public float TargetRotation(Transform oneself, Transform other)
    {
        float result = Mathf.Atan2(other.position.z - oneself.position.z, other.position.x - oneself.position.x) * Mathf.Rad2Deg;
        return result;
    }
}
