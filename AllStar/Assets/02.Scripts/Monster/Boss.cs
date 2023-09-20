using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool pattern_Start_bool = true;              //������ �����ϱ� ���� �Ұ�
    /*public float standby_time;                          //��� ���� ���� �ð�*/
    public bool pattern_loop;
    public string motion_Type;                          //������ ������ �����ϱ� ���� string��
    public int randomNum;
    public BossAttackPattern pattern;
    public Status stat;
    public bool nonControllable;

    // Start is called before the first frame update
    void Start()
    {
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
        motion_Type = $"BosePattern{randomNum + 1}";     //���߿� ���� ������ �� ���� ��� �ڷ�ƾ�� �ش� ���� �̸����� ����
        pattern = (BossAttackPattern)randomNum;
        if (stat.states.ContainsKey("attack") && stat.nowState != stat.states["attack"])
        {
            fsmChanger(stat.states["attack"]);
        }
        switch (pattern)             //���� ��������
        {
            case (BossAttackPattern)1: //������ �ֱ�
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
    #region ���� ���ϵ�
    IEnumerator BosePattern1()              //��������
    {
        
        while (pattern_loop)
        {
            gameObject.transform.Rotate(360 * Time.deltaTime, 360 * Time.deltaTime, 360 * Time.deltaTime) ;
            Pattern_Stop();
            yield return null;
        }
        IdelAnimator_Run();
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern2()              //��������
    {
        while (pattern_loop)
        {
            gameObject.transform.position += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        IdelAnimator_Run();
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern3()              //��������
    {
        while (pattern_loop)
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern4()              //������ ����
    {
        IdelAnimator_Run();
        while (pattern_loop)
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    IEnumerator BosePattern5()              //�����̴� ����
    {
        while (pattern_loop)
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            Pattern_Stop();
            yield return null;
        }
        pattern_Start_bool = true;
    }
    public void Pattern_Stop()  //�̰� ������ ������ �ϴ� �Լ�
    {
        if (stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            pattern_loop = false;
    }
    public void IdelAnimator_Run()
    {
        if (stat.states.ContainsKey("idle") && stat.nowState != stat.states["idle"])
        {
            fsmChanger(stat.states["idle"]);
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
