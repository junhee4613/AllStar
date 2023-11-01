using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Static : MonoBehaviour
{
    public bool pattern_Start_bool = true;              //������ �����ϱ� ���� �Ұ�
    //pattern_loop�� ���߿� ���ִ� ��
    public bool pattern_loop;
    public string motion_Type;                          //������ ������ �����ϱ� ���� string��
    public int randomNum;
    public Boss_Simple_Pattern pattern;
    public bool barrage_start = false;
    public float time;
    public float[] standby_time = new float[2] {1f, 2f};
    public GameObject player;
    public Status state;
    [Header("���� hp")]
    public float current_hp;
    public float non_hit_time;
    [Header("�ȸ¾Ƽ� ������ ������ �ð� ����")]
    public float heal_pattern_start_time;
    public bool heal_pattern_start;
    [Header("����")]
    public float heal_quantity;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {                                   //this�� ���� Ŭ������ ��Ÿ��

    }

    // Update is called once per frame
    private void Update()
    {
        /*if (!heal_pattern_start)
        {
            non_hit_time += Time.deltaTime;
        }
        //���� ��ü������ �պ��ߵɵ�
        if (state.nowHP == current_hp && non_hit_time > heal_pattern_start_time)
        {
            HealPattern();
        }
        else
        {
            current_hp = state.nowHP;
        }*/
        if (pattern_Start_bool && standby_time[Random.Range(0, 1)] < time)
        {
            Pattern();
        }
        time += Time.deltaTime;
    }
    public void Pattern()       //�ڷ�ƾ���� �������� �ð� üũ�ؼ� 
    {
        pattern_Start_bool = false;
        pattern_loop = true;
        randomNum = Random.Range(0, 5);
        motion_Type = $"Simple_Pattern{randomNum + 1}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
        pattern = (Boss_Simple_Pattern)randomNum;

        switch (pattern)             //���� ��������(���� ���� �ܿ�)
        {
            case Boss_Simple_Pattern.BARRAGE1: //������ �ֱ�
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
        if (state.nowHP < state.maxHP / 10 * 3.5)
        {
            //���߿�
        }
        else{ }
        
    }
    public void HealPattern()
    {
        heal_pattern_start = true;
        non_hit_time = 0;
        fsmChanger(state.states["heal"]);
        if (state.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            state.nowHP += Time.deltaTime * heal_quantity;
        }
        else
        {
            heal_pattern_start = false;
        }
        //���߿� �� ������ ������
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
    #region ���� ���� HP35�� �ʰ��� ��
    IEnumerator Simple_Pattern1()              //ź������
    {

        if (state.states.ContainsKey("pattern1") && state.nowState != state.states["pattern1"])
        {
            //fsmChanger(monsterStatus.states["pattern1"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern2()              //��������
    {
        if (state.states.ContainsKey("pattern2") && state.nowState != state.states["pattern2"])
        {
            //fsmChanger(monsterStatus.states["pattern2"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����2");

            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern3()              //��������
    {
        while (pattern_loop)
        {
            Debug.Log("����3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern4()              //������ ����
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
    IEnumerator Simple_Pattern5()              //�����̴� ����
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
    IEnumerator Simple_Pattern6()      //������ ����
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
        pattern_loop = false;
        time = 0;
        /*if (time > 5)
        {
            
        }*/
    }
    public void AttackAnimator_Run()
    {
        if (state.states.ContainsKey("attack") && state.nowState != state.states["attack"])
        {
            //fsmChanger(monsterStatus.states["attack"]);
        }
    }
    #endregion
    #region ���� ���� HP35�� ������ ��
    IEnumerator Hard_Pattern1()              //ź������
    {

        if (state.states.ContainsKey("pattern1") && state.nowState != state.states["pattern1"])
        {
            //fsmChanger(monsterStatus.states["pattern1"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����1");
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern2()              //��������
    {
        if (state.states.ContainsKey("pattern2") && state.nowState != state.states["pattern2"])
        {
            //fsmChanger(monsterStatus.states["pattern2"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����2");

            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern3()              //��������
    {
        if (state.states.ContainsKey("pattern3") && state.nowState != state.states["pattern3"])
        {
            //fsmChanger(monsterStatus.states["pattern3"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator Hard_Pattern4()              //������ ����
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
    IEnumerator Hard_Pattern5()              //�����̴� ����
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
    IEnumerator Hard_Pattern6()      //������ ����
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
    #endregion
}
