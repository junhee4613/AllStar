using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonoBehaviour
{
                 //������ �����ϱ� ���� �Ұ�
    //pattern_loop�� ���߿� ���ִ� ��
    [Header("���� ��� ���� �� ���� �ð�")]
    public float standby_time;
    public GameObject player;
    public GameObject[] simple_barrage_patterns;
    public GameObject[] hard_barrage_patterns;
    public Status state;
    [Header("�ʴ� ȸ�� ����")]
    public float rotateSpeed;
    [Header("�ȸ¾Ƽ� ������ ������ �ð� ����")]
    public float heal_pattern_start_time;
    [Header("true���� ������ ����")]
    public bool heal_pattern_start;
    [Header("���� = �ʴ� ���� ?�ۼ�Ʈ ")]
    public float heal_quantity;
    [Header("���� ���� �� ���� �ð���")]
    public float[] standby_time_standard = new float[2] { 1f, 2f };
    [Header("ü�� ȸ���ϴ� �ð�")]
    public int heal_time;
    [Header("������ Ȧ���ð�")]
    public int laser_holding_time;
    [Header("���󰡴� ������ �� �� �÷��̾� �Ĵٺ��� �ӵ�")]
    public int follow_laser_look_rotation;

    float holding_time;
    [Header("ó���� true���� ���� ���� - �׽�Ʈ��(�̰� �Ȱǵ���� ��)")]
    public bool action_start = true;
    [Header("�ȶ��� �ð� - �׽�Ʈ��(�̰� �Ȱǵ���� ��)")]
    public float non_hit_time;
    [Header("���� hp - �׽�Ʈ��(�̰� �Ȱǵ���� ��)")]
    public float current_hp;

    bool pattern_loop;
    public string motion_Type;                          //������ ������ �����ϱ� ���� string��
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
    {                                   //this�� ���� Ŭ������ ��Ÿ��
        fsmChanger(state.states["idle"]);
    }

    // Update is called once per frame
    private void Update()
    {
        if (state.nowHP >= state.maxHP / 100 * 35 && !heal_pattern_start)
        {
            if (current_hp == state.nowHP)//���� hp�� ������ ���� ���
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, LookPlayer(player), transform.rotation.z), rotateSpeed * Time.deltaTime);//���߿� ���� �𵨸� ���� ���� ��ġ�� ���� ������ ���ɼ� ����



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
    
    public void Pattern()       //�ڷ�ƾ���� �������� �ð� üũ�ؼ� 
    {
        action_start = false;
        pattern_loop = true;
        //���߿� ���Ͽ� �ϵ���� ���ϴ� ����� �־�ߵǴµ� ��� ó������ �����
        if (state.nowHP < state.maxHP / 10 * 3.5)
        {
            randomNum = Random.Range(1, 4);             
            motion_Type = $"Hard_Pattern{randomNum}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
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
                randomNum = 6;                  //�������� 6���̱� ����

            }
            else
            {
                return;
                //randomNum = 6;                  //�������� 6���̱� ����
                //randomNum = Random.Range(1, 5);             //���߿� 5�� �÷��ߵ�
            }
            motion_Type = $"Simple_Pattern{randomNum}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
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
                    Debug.Log("�ڷ�ƾ");
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
    
    #region ���� ���� HP35�� �ʰ��� ��
    IEnumerator Simple_Pattern1()               //ź������   
    {

        
        while (pattern_loop)
        {
            Debug.Log("����1");
            Barrage_Pattern_Stop(simple_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Simple_Pattern2()              //ź������
    {
        
        while (pattern_loop)
        {
            Debug.Log("����2");

            Barrage_Pattern_Stop(simple_barrage_patterns);
        }
        Debug.Log("����");
        yield return null;

        action_start = true;
    }
    IEnumerator Simple_Pattern3()              //ź������
    {
        while (pattern_loop)
        {
            Debug.Log("����3");
            Barrage_Pattern_Stop(simple_barrage_patterns);
            yield return null;
        }

        action_start = true;
    }
    IEnumerator Simple_Pattern4()              //������ ª�� �ѹ� ��� ���� �̶��� ���󰡴� ���������� �� ������ ��
    {
        fsmChanger(state.states["simple_pattern4"]);
        yield return null;
        while (pattern_loop)
        {
            if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.306f)
            {
                //������ ������
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
                Pattern_Stop(); // ���߿� else���� �ֱ�
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    IEnumerator Simple_Pattern5()              //������ ��鼭 �÷��̾ õõ�� �Ĵٺ��� ����
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
                Pattern_Stop(); // ���߿� else���� �ֱ�
            }
            yield return null;
        }
        fsmChanger(state.states["idle"]);
        action_start = true;
    }
    
    IEnumerator Simple_Pattern6()      //������
    {
        non_hit_time = 0;
        fsmChanger(state.states["simple_pattern6"]);
        yield return null;
        while (heal_pattern_start)
        {
            test = state.animator.GetCurrentAnimatorStateInfo(0);
            if (Current_anim_and_time("simple_pattern6", 1))
            {
                Debug.Log("�ݺ�");
                fsmChanger(state.states["heal_idle"]);
            }
            else if (Current_anim_and_time("heal_idle", heal_time))
            {
                Debug.Log(state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                Debug.Log("�ݺ�");
                fsmChanger(state.states["return"]);
            }
            else if(Current_anim_and_time("return", 1))
            {
                Debug.Log("�ݺ�");
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
    #region ���� ���� HP35�� ������ ��
    IEnumerator Hard_Pattern1()              //ź������
    {

        
        while (pattern_loop)
        {
            Debug.Log("����1");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern2()              //��������
    {
        
        while (pattern_loop)
        {
            Debug.Log("����2");

            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern3()              //��������
    {
        
        while (pattern_loop)
        {
            Debug.Log("����3");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }

        action_start = true;
    }
    IEnumerator Hard_Pattern4()              //������ ����
    {
        while (pattern_loop)
        {
            Debug.Log("����4");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern5()              //�����̴� ����
    {
        while (pattern_loop)
        {
            Debug.Log("����5");
            Barrage_Pattern_Stop(hard_barrage_patterns);
            yield return null;
        }
        action_start = true;
    }
    IEnumerator Hard_Pattern6()      //������ ����
    {
        while (pattern_loop)
        {
            Debug.Log("����6");
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
    {       //���� ��� �ð��� ������ ����߰ų� idle���¿� �ִϸ��̼��� ������ ������ ���
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
