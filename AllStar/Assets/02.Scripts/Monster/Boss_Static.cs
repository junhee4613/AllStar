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
    public Boss_Simple_Pattern simple_pattern;
    private Boss_Hard_Pattern hard_pattern;
    public bool barrage_start = false;
    public float standby_time;
    public float[] standby_time_standard = new float[2] {1f, 2f};
    public GameObject player;
    public GameObject[] barrage_patterns;
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
        player = GameObject.FindGameObjectWithTag("Player");
        barrage_patterns = GameObject.FindGameObjectsWithTag("Barrage");
    }
    private void Start()
    {                                   //this�� ���� Ŭ������ ��Ÿ��

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
                TargetRotation(transform, player.gameObject.transform);//���߿� ���� �𵨸� ���� ���� ��ġ�� ���� ������ ���ɼ� ����

        }
        else
        {
            standby_time += Time.deltaTime;
        }
    }
    public void Pattern()       //�ڷ�ƾ���� �������� �ð� üũ�ؼ� 
    {
        pattern_Start_bool = false;
        pattern_loop = true;

        //���߿� ���Ͽ� �ϵ���� ���ϴ� ����� �־�ߵǴµ� ��� ó������ �����
        if (state.nowHP < state.maxHP / 10 * 3.5)
        {
            randomNum = Random.Range(0, 3);             
            motion_Type = $"Hard_Pattern{randomNum + 1}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
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
            randomNum = Random.Range(0, 4);             //���߿� 5�� �÷��ߵ�
            motion_Type = $"Simple_Pattern{randomNum + 1}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
            simple_pattern = (Boss_Simple_Pattern)randomNum;
            switch (simple_pattern)             //���� ��������(���� ���� �ܿ�)
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
    IEnumerator Simple_Pattern2()              //ź������
    {
        if (state.states.ContainsKey("pattern2") && state.nowState != state.states["pattern2"])
        {
            //fsmChanger(monsterStatus.states["pattern2"]);
        }
        while (pattern_loop)
        {
            Debug.Log("����2");

            Pattern_Stop();
        }
        Debug.Log("����");
        yield return null;

        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern3()              //ź������
    {
        while (pattern_loop)
        {
            Debug.Log("����3");
            Pattern_Stop();
            yield return null;
        }

        pattern_Start_bool = true;
    }
    IEnumerator Simple_Pattern4()              //������ ª�� �ѹ� ��� ���� �̶��� ���󰡴� ���������� �� ������ ��
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
    IEnumerator Simple_Pattern5()              //������ ��鼭 �÷��̾ õõ�� �Ĵٺ��� ����
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
    IEnumerator Simple_Pattern6()      //������
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
            pattern_loop = false;//########################���߿� ����
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
    public float TargetRotation(Transform oneself, Transform other)
    {
        float result = Mathf.Atan2(other.position.z - oneself.position.z, other.position.x - oneself.position.x) * Mathf.Rad2Deg;
        return result;
    }
}
