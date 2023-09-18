using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool pattern_Start_bool = true;              //������ �����ϱ� ���� �Ұ�
    public float standby_time;                          //��� ���� ���� �ð�
    public string motion_Type;                          //������ ������ �����ϱ� ���� string��
    public int randomNum;
    public BossPattern motion;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (pattern_Start_bool)
        {
            standby_time += Time.deltaTime;
            if (standby_time > randomNum)
                Pattern();
        }
    }
    public void Pattern()
    {
        pattern_Start_bool = false;         
        randomNum = Random.Range(0, 4);
        motion_Type = $"Motion{randomNum}";
        motion = (BossPattern)randomNum;
        switch (motion)
        {
            case (BossPattern)0: //������ �ֱ�
                StartCoroutine(motion_Type);
                break;
            case (BossPattern)1:
                StartCoroutine(motion_Type);
                break;
            case (BossPattern)2:
                StartCoroutine(motion_Type);
                break;
            case (BossPattern)3:
                StartCoroutine(motion_Type);
                break;
            default:
                break;


        }
    }
    IEnumerator Motion0()
    {
        yield return new WaitForSeconds(1);
        standby_time = 0;
        pattern_Start_bool = true;
    }
    IEnumerator Motion1()
    {
        standby_time = 0;
        while (standby_time < randomNum + 2)
        {
            standby_time += Time.deltaTime;
            gameObject.transform.Rotate(360 * Time.deltaTime, 360 * Time.deltaTime, 360 * Time.deltaTime) ;
            yield return null;
        }

        standby_time = 0;
        pattern_Start_bool = true;
    }
    IEnumerator Motion2()
    {
        while (standby_time < randomNum + 1)
        {
            standby_time += Time.deltaTime;
            gameObject.transform.position += Vector3.one * Time.deltaTime;
            yield return null;
        }
        
        standby_time = 0;
        pattern_Start_bool = true;
    }
    IEnumerator Motion3()
    {
        while (standby_time < randomNum)
        {
            standby_time += Time.deltaTime;
            gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
        standby_time = 0;
        pattern_Start_bool = true;
    }
}
