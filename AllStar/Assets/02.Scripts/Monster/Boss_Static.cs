using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Static : MonoBehaviour
{
                 //패턴이 시작하기 위한 불값
    //pattern_loop는 나중에 없애는 값
    public GameObject player;
    public GameObject[] simple_barrage_patterns;
    public GameObject[] hard_barrage_patterns;
    public Nomal_monster state;
    [Header("초당 회전 각도")]
    public float rotateSpeed;
    [Header("안맞아서 힐패턴 나오는 시간 기준")]
    public float heal_pattern_start_time;
    [Header("true여야 힐패턴 시작")]
    public bool heal_pattern_start;
    [Header("힐량 = 초당 피의 ?퍼센트 ")]
    public float heal_quantity;
    [Header("패턴 끝난 후 쉬는 시간들")]
    public float[] standby_time_standard = new float[2] { 0.7f, 1.5f };
    [Header("체력 회복하는 시간")]
    public int heal_time;
    [Header("탄막패턴 지속 시간 기준")]
    public int barrage_pattern_operation_time;
    [Header("따라가는 레이저 쏠 때 플레이어 쳐다보는 속도")]
    public int follow_laser_rotation_speed;
    public GameObject[] laser = new GameObject[8];
    //모든 패턴이 시작된 시간
    public float Pattern_last_time;
    [Header("처음에 true여야 패턴 시작 - 테스트용(이건 안건드려도 됨)")]
    public bool action_start = true;
    [Header("안때린 시간 - 테스트용(이건 안건드려도 됨)")]
    public float non_hit_time;
    [Header("현재 hp - 테스트용(이건 안건드려도 됨)")]
    public float current_hp;
    [Header("홀딩 레이저 데미지 들어가는 주기")]
    public float damage_cycle;
    [Header("하드 패턴 레이저 공격 횟수")]
    public int laser_attack_num;
    [Header("하드 패턴 레이저 최대 각도")]
    public float hard_laser_rotation;
    public GameObject simple_core;
    public GameObject hard_core;
    public GameObject portal;
    bool die = false;

    float standby_time;
    bool hard_pattern_start = false;
    bool pattern_loop;
    public string motion_Type;                          //랜덤한 패턴을 시작하기 위한 string값
    public int randomNum;
    Boss_Hard_Pattern hard_pattern;
    public Boss_Simple_Pattern simple_pattern;
    public AnimatorStateInfo test;
    public bool look_target;
    public bool attack;


    //테스트용
    public float test2 = 15;
    


    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.monstersInScene.Add(this.gameObject.name, state);
        Managers.Pool.MonsterPop("Boss1", this.gameObject);
        state.states.SetBossFSMDefault(ref state.animator, this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("#보스 피통 UI 나중에 수정");
        state.hp_bar = GameObject.FindWithTag("Monster_hp_bar");
        state.hp_bar.SetActive(false);
    }
    private void Start()
    {                                   //this는 현재 클래스를 나타냄
        fsmChanger(state.states["idle"]);
    }

    // Update is called once per frame
    private void Update()
    {
        if(state.nowHP <= 0)
        {
            if (die)
            {
                return;
            }
            else
            {
                die = true;
                Die();
            }
        }
        if (state.hp_bar.activeSelf)
        {
            state.hit_time += Time.deltaTime;
            if (state.hit_time > 5)
            {
                state.hp_bar.SetActive(false);
            }
        }
        if (state.nowHP >= state.maxHP / 100 * 35)
        {
            if (!heal_pattern_start)
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
        }
        else
        {
            hard_pattern_start = true;
            hard_core.SetActive(true);
            simple_core.SetActive(false);
        }

        if (!pattern_loop || look_target)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);
            Debug.Log("회전");
        }



        if (action_start)
        {
            if (Pattern_Start())
            {
                Pattern();
            }
            else
            {
                standby_time += Time.deltaTime;
            }
        }
    }
    
    public void Pattern()       //코루틴으로 하지말고 시간 체크해서 
    {
        action_start = false;
        pattern_loop = true;
        //나중에 패턴에 하드모드로 변하는 모습을 넣어야되는데 어떻게 처리할지 고민중
        if (hard_pattern_start)
        {
            randomNum = Random.Range(2, 5);             
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
                randomNum = Random.Range(1, 6);             //나중에 5로 늘려야됨 randomNum = Random.Range(0, n);까지 설정하면 n-1까지만 숫자가 나옴  
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
            if (hard_pattern_start)
            {
                state.animator.speed = 2;
            }
        }
    }
    
    #region 보스 패턴 HP35퍼 초과일 때
    IEnumerator Simple_Pattern1()               //탄막패턴   
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(simple_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if(Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop();
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!simple_barrage_patterns[0].activeSelf)
                {
                    simple_barrage_patterns[0].SetActive(true);
                }
            }
            
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern2()              //탄막패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(simple_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop();
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!simple_barrage_patterns[1].activeSelf)
                {
                    simple_barrage_patterns[1].SetActive(true);
                }
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern3()              //탄막패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if(Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(simple_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!simple_barrage_patterns[2].activeSelf)
                {
                    simple_barrage_patterns[2].SetActive(true);
                }
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern4()              //레이저 짧게 한번 쏘는 패턴 이때는 따라가는 레이저보다 더 빠르게 쏨
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
                attack = true;
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if(Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            else if (attack)
            {
                GameObject temp = Managers.Pool.Pop(Managers.DataManager.Datas["Boss_Laser"] as GameObject);
                temp.transform.position = gameObject.transform.position + transform.forward * 2;
                temp.transform.rotation = gameObject.transform.rotation;
                attack = false;
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern5()              //레이저 쏘면서 플레이어를 천천히 쳐다보는 패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern5"]);
                attack = true;
            }
            else if (Current_anim_Up_and_time("simple_pattern5", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
                test2 = 1;
            }
            else if(Current_anim_Up_and_time("simple_pattern5", 0.4f) && state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6 && state.nowState == state.states["simple_pattern5"])
            {
                //2의 6승으로 계산
                test2 += Time.deltaTime * 2;
                //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, gameObject.transform.rotation.y + 45, 0), Time.deltaTime);
                gameObject.transform.Rotate(0, (test2 * test2 * test2 * test2 * test2 * test2) * Time.deltaTime, 0);
            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            else if (attack)
            {
                Laser_various_aspects();
                attack = false;
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
            if (Current_anim_Up_and_time("simple_pattern6", 1))
            {
                fsmChanger(state.states["heal_idle"]);
            }
            else if (Current_anim_Up_and_time("heal_idle", heal_time))
            {
                fsmChanger(state.states["return"]);
            }
            else if(Current_anim_Up_and_time("return", 1))
            {
                heal_pattern_start = false;
                Pattern_Stop();
            }
            else
            {
                state.nowHP = Mathf.Clamp(state.nowHP + Time.deltaTime * state.maxHP / 100 * heal_quantity, 0, state.maxHP);
                if (state.hp_bar != null && Managers.UI.monster_hp_bar.Item2 != null)
                {
                    Managers.UI.monster_hp_bar.Item2.text = state.name;
                    Managers.UI.monster_hp_bar.Item1.value = (state.nowHP / state.maxHP) * 100;
                }
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
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(hard_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop();
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!hard_barrage_patterns[0].activeSelf)
                {
                    hard_barrage_patterns[0].SetActive(true);
                }
            }

            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Hard_Pattern2()              //공격패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(hard_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop();
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!hard_barrage_patterns[1].activeSelf)
                {
                    hard_barrage_patterns[1].SetActive(true);
                }
            }

            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Hard_Pattern3()              //공격패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.6f))
            {
                Barrage_Pattern_Stop(hard_barrage_patterns);
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.45f))
            {
                if (barrage_pattern_operation_time >= Pattern_last_time)
                {
                    state.animator.speed = 0;
                    Pattern_last_time += Time.deltaTime;
                }
                else
                {
                    state.animator.speed = 1;
                }

            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop();
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 0.31f))
            {
                if (!hard_barrage_patterns[2].activeSelf)
                {
                    hard_barrage_patterns[2].SetActive(true);
                }
            }

            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Hard_Pattern4()              //가만히 패턴
    {
        fsmChanger(state.states["idle_to_attack"]);
        yield return null;
        while (pattern_loop)
        {
            if (Current_anim_Up_and_time("idle_to_attack", 1))
            {
                fsmChanger(state.states["simple_pattern4"]);
                attack = true;
            }
            else if (Current_anim_Up_and_time("simple_pattern4", 1))
            {
                fsmChanger(state.states["attack_to_idle"]);
            }
            else if (Current_anim_Up_and_time("attack_to_idle", 1))
            {
                Pattern_Stop(); // 나중에 else문에 넣기
            }
            else if (attack)
            {
                state.animator.speed = 0;
                for (int i = 0; i < laser_attack_num; i++)      //여기에 레이저 반복 횟수 써주기
                {
                    float range = Random.Range(-hard_laser_rotation, hard_laser_rotation);
                    gameObject.transform.rotation = Quaternion.Euler(0, LookPlayer(player) + range, 0);
                    Debug.Log(transform.rotation);
                    GameObject temp = Managers.Pool.Pop(Managers.DataManager.Datas["Boss_Laser_hard"] as GameObject);
                    temp.transform.position = gameObject.transform.position + transform.forward * 2;
                    temp.transform.rotation = gameObject.transform.rotation;
                    yield return new WaitForSeconds(1);
                }
                attack = false;
                state.animator.speed = 2f;
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    #endregion
    public void Barrage_Pattern_Stop(GameObject[] temp)
    {
        foreach (var item in temp)
        {
            item.SetActive(false);
        }
        Pattern_last_time = 0;
        Debug.Log("반응");
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
    public bool Current_anim_Up_and_time(string anim, float time)
    {
        if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= time && state.nowState == state.states[anim])
        {
            return true;
        }
        else
            return false;
    }
    public void Die()
    {
        portal.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Laser_various_aspects()
    {
        Debug.Log("#나중에 수정");
        for (int i = 0; i < 8; i++)
        {
            laser[i].SetActive(true);
           /* GameObject temp = Managers.Pool.Pop(Managers.DataManager.Datas["Boss_Laser"] as GameObject);
            switch (i)
            {
                case 0:
                    temp.transform.position = gameObject.transform.position + transform.forward * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 1:
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 2:
                    temp.transform.position = gameObject.transform.position + transform.right * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 3:
                    temp.transform.position = gameObject.transform.position + (transform.forward * -1 + transform.right).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 4:
                    temp.transform.position = gameObject.transform.position + transform.forward * -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 5:
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right).normalized * -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 6:
                    temp.transform.position = gameObject.transform.position + transform.right* -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;
                case 7:
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right * -1).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 45 * i, gameObject.transform.rotation.z);
                    break;

                default:
                    break;
            }*/
        }
    }


}
