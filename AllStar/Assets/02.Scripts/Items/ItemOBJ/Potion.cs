using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour,IPotion
{
    public bool groundCheck = false;
    public float time;
    public int dir = 1;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (groundCheck)
        {
            Debug.Log("�սǵս�");
            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(1, 2 * dir, 2), transform.position.z);
            if (0 >= time - Time.deltaTime)
            {
                dir = -1;
                time = 2;
            }
            else if(1 >= time - Time.deltaTime)
            {
                dir = -1;
            }
            
        }
        else if (!groundCheck)
        {
            Debug.Log("ȸ�� ��");
            transform.Rotate(0, 0, 360 * Time.deltaTime);
        }
        else if (Physics.CheckBox(transform.position, transform.localScale * 4f, transform.rotation, 512))
        {
            //�̰� �����Ǵ� ���� �����ؾߵ�
            Debug.Log("ȸ�� ����");
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            groundCheck = true;
        }
        else
        {
            Debug.Log("�ƹ��͵� �ƴ�");
        }
    }
    public void Use()
    {
        //���⿡ �÷��̾� ȸ����Ű�� ����
    }
    //http://theeye.pe.kr/archives/2764 �̰� Unity RayCast, BoxCast, SphereCast 
}
