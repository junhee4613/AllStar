using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonoBehaviour
{
                 //패턴이 시작하기 위한 불값
    //pattern_loop는 나중에 없애는 값
    [Header("패턴 모션 끝난 후 쉬는 시간")]
    public float standby_time;
    public GameObject player;
    public GameObject[] simple_barrage_patterns;
    public GameObject[] hard_barrage_patterns;
    public Status state;
    [Header("초당 회전 각도")]
    public float rotateSpeed;
    [Header("안맞아서 힐패턴 나오는 시간 기준")]
    public float heal_pattern_start_time;
    [Header("true여야 힐패턴 시작")]
    public bool heal_pattern_start;
    [Header("힐량 = 초당 피의 ?퍼센트 ")]
    public float heal_quantity;
    [Header("패턴 끝난 후 쉬는 시간들")]
    public float[] standby_time_standard = new float[2] { 1f, 2f };
    [Header("체력 회복하는 시간")]
    public int heal_time;
    [Header("레이저 홀딩시간")]
    public int laser_holding_time;
    [Header("따라가는 레이저 쏠 때 플레이어 쳐다보는 속도")]
    public int follow_laser_look_rotation;

    float holding_time;
    [Header("처음에 true여야 패턴 시작 - 테스트용(이건 안건드려도 됨)")]
    public bool action_start = true;
    [Header("안때린 시간 - 테스트용(이건 안건드려도 됨)")]
    public float non_hit_time;
    [Header("현재 hp - 테스트용(이건 안건드려도 됨)")]
    public float current_hp;

    bool pattern_loop;
    public string motion_Type;                          //랜덤한 패턴을 시작하기 위한 string값
    public int randomNum;
    Boss_Hard_Pattern hard_pattern;
    public Boss_Simple_Pattern simple_pattern;
    bool barrage_start = false;
    public AnimatorStateInfo test;
    public bool look_target;
    


    // Start is called before the first frame update
    private void Awake()
    {
        state.states.SetBossFSMDefault(ref state.animator, this.gameObject);
        Debug.Log(state.states.Count);
        Debug.Log(state.animator);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {                                   //this는 현재 클래스를 나타냄
        fsmChanger(state.states["idle"]);
    }

    // Update is called once per frame
    private void Update()
    {
        if (state.nowHP >= state.maxHP / 100 * 35 && !heal_pattern_start)
        {
            if (current_hp == state.nowHP)//현재 hp가 변동이 없을 경우
            {
                non_hit_time += Time.deltaTime;
            }
            else
            {
                current_hp = state.nowHP;
                non_hit_time = 0;
            }
            //
            if (non_hit_time >= heal_pattern_start_time)
            {
                heal_pattern_start = true;
            }
        }
        else
        {
            //heal_pattern_start = false;
        }

        if((action_start && Pattern_Start()) || look_target)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);//나중에 보스 모델링 보고 로직 위치나 조건 수정할 가능성 높음



        if (action_start)
        {
            if (Pattern_Start())
            {
                Pattern();
            }
        }
        else
        {
            
            standby_time += Time.deltaTime;
        }
    }
    
    public void Pattern()       //코루틴으로 하지말고 시간 체크해서 
    {
        action_start = false;
        pattern_loop = true;
        //나중에 패턴에 하드모드로 변하는 모습을 넣어야되는데 어떻게 처리할지 고민중
        if (state.nowHP < state.maxHP / 10 * 3.5)
        {
            randomNum = Random.Range(1, 4);             
            motion_Type = $"Hard_Pattern{randomNum}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
            hard_pattern = (Boss_Hard_Pattern)randomNum - 1;
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
            if (heal_pattern_start)
            {
                randomNum = 6;                  //힐패턴이 6번이기 때문

            }
            else
            {
                return;
                //randomNum = 6;                  //힐패턴이 6번이기 때문
                //randomNum = Random.Range(1, 5);             //나중에 5로 늘려야됨
            }
            motion_Type = $"Simple_Pattern{randomNum}";     //나중에 패턴 나오면 이 변수 대신 코루틴에 해당 패턴 이름으로 변경
            simple_pattern = (Boss_Simple_Pattern)randomNum - 1;
            switch (simple_pattern)             
            {
                case Boss_Simple_Pattern.BARRAGE1:
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
                    Debug.Log("코루틴");
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
            if(state.nowState != null)
            {
                state.nowState.OnStateExit();
            }
            state.nowState = BS;
            state.nowState.OnStateEnter();
        }
    }
    
    #region 보스 패턴 HP35퍼 초과일 때
    IEnumerator Simple_Pattern1()               //탄막패턴   
    {

        
        while (pattern_loop)
        {
            Debug.Log("패턴1");
            Barrage_Pattern_Stop(simple_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Simple_Pattern2()              //탄막패턴
    {
        
        while (pattern_loop)
        {
            Debug.Log("패턴2");

            Barrage_Pattern_Stop(simple_barrage_patterns);
        }
        Debug.Log("여기");
        yield return null;

        action_start = true;
    }
    IEnumerator Simple_Pattern3()              //탄막패턴
    {
        while (pattern_loop)
        {
            Debug.Log("패턴3");
            Barrage_Pattern_Stop(simple_barrage_patterns);
            yield return null;
        }

        action_start = true;
    }
    IEnumerator Simple_Pattern4()              //레이저 짧게 한번 쏘는 패턴 이때는 따라가는 레이저보다 더 빠르게 쏨
    {
        fsmChanger(state.states["simple_pattern4"]);
        yield return null;
        while (pattern_loop)
        {
            if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.306f)
            {
                //레이저 모으기
            }
            else if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.61f)
            {

            }
            else if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.999f)
            {
                if (laser_holding_time <= holding_time)
                {
                    holding_time = 0;
                }
                else
                {
                    holding_time += Time.deltaTime;
                }

            }
            else
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern5()              //레이저 쏘면서 플레이어를 천천히 쳐다보는 패턴
    {
        fsmChanger(state.states["simple_pattern5"]);
        yield return null;
        while (pattern_loop)
        {
            if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.306f)
            {
                rotateSpeed = follow_laser_look_rotation;
            }
            else if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.61f)
            {
                if(state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                {
                    state.animator.speed = 0;
                    if (laser_holding_time <= holding_time)
                    {
                        holding_time = 0;
                        Managers.Pool.Push(temp);
                    }
                    else
                    {
                        holding_time += Time.deltaTime;
                        GameObject temp = Managers.Pool.Pop(Managers.DataManager.Datas["Boss_Laser"] as GameObject);
                    }
                }
                else
                {
                    state.animator.speed = 0;
                }
                
            }
            else if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    
    IEnumerator Simple_Pattern6()      //힐패턴
    {
        non_hit_time = 0;
        fsmChanger(state.states["simple_pattern6"]);
        yield return null;
        while (heal_pattern_start)
        {
            test = state.animator.GetCurrentAnimatorStateInfo(0);
            if (Current_anim_and_time("simple_pattern6", 1))
            {
                Debug.Log("반복");
                fsmChanger(state.states["heal_idle"]);
            }
            else if (Current_anim_and_time("heal_idle", heal_time))
            {
                Debug.Log(state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                Debug.Log("반복");
                fsmChanger(state.states["return"]);
            }
            else if(Current_anim_and_time("return", 1))
            {
                Debug.Log("반복");
                Debug.Log(state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                heal_pattern_start = false;
            }
            else
            {
                state.nowHP += Time.deltaTime * state.maxHP / 100 * heal_quantity;
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    

    #endregion
    #region 보스 패턴 HP35퍼 이하일 때
    IEnumerator Hard_Pattern1()              //탄막패턴
    {

        
        while (pattern_loop)
        {
            Debug.Log("패턴1");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern2()              //공격패턴
    {
        
        while (pattern_loop)
        {
            Debug.Log("패턴2");

            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern3()              //공격패턴
    {
        
        while (pattern_loop)
        {
            Debug.Log("패턴3");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }

        action_start = true;
    }
    IEnumerator Hard_Pattern4()              //가만히 패턴
    {
        while (pattern_loop)
        {
            Debug.Log("패턴4");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern5()              //움직이는 패턴
    {
        while (pattern_loop)
        {
            Debug.Log("패턴5");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern6()      //가만히 패턴
    {
        while (pattern_loop)
        {
            Debug.Log("패턴6");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    #endregion
    public void Barrage_Pattern_Stop(GameObject[] temp)
    {
        if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            foreach (var item in temp)
            {
                item.SetActive(false);
            }
            Pattern_Stop();
        }
    }
    public void Pattern_Stop()
    {
        pattern_loop = false;
        standby_time = 0;
    }
    public float LookPlayer(GameObject hit)
    {
        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        return target;
    }
    public bool Pattern_Start()
    {       //현재 대기 시간의 기준을 경과했거나 idle상태에 애니메이션이 완전히 끝났을 경우
        if (standby_time_standard[Random.Range(0, standby_time_standard.Length - 1)] < standby_time
            && state.nowState == state.states["idle"] && state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            return true;
        else
        {
            return false;
        }

    }
    public bool Current_anim_and_time(string anim, int time)
    {
        if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= time && state.nowState == state.states[anim])
        {
            return true;
        }
        else
            return false;
    }
}
