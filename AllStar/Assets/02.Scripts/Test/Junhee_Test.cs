using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public Material test;
    public float test_x;
    public float test_y;
    //public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    [Header("X�� ĭ ��")]
    public float size_x;
    [Header("Y�� ĭ ��")]
    public float size_y;
    [Header("Z�� ��������?")]
    public float size_z;
    [Header("�ִ� �νİŸ�?")]
    public float dis;

    public Status monsterStat = new Status();
    private void Awake()
    {
    }
    private void Update()
    {
        if (Physics.BoxCast(transform.position, new Vector3(size_x, size_y, size_z), transform.forward, Quaternion.identity, dis, 1 << 7))
        {
            Debug.Log("������ ��");
        }
        /*for (int i = 0; i < 8; i++)
        {
            Debug.Log("������ ��ȯ");
            switch (i)
            {
                case 0:
                    Debug.Log(gameObject.transform.forward);
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + transform.forward * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 1:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 2:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + transform.right * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 3:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + (transform.forward * -1 + transform.right).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 4:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + transform.forward * -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 5:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right).normalized * -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 6:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + transform.right * -2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;
                case 7:
                    Debug.Log(gameObject.transform.forward);
                    temp.transform.position = gameObject.transform.position + (transform.forward + transform.right * -1).normalized * 2;
                    temp.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 45 * i, gameObject.transform.rotation.z);
                    break;

                default:
                    break;
            }
        }*/
        //Debug.Log(gameObject.transform.forward);
        //Debug.Log(gameObject.transform.position + transform.forward * 2);
    }

    /*private void Update()
    {
        test.mainTextureOffset = new Vector2(Time.time * test_x, Time.time * test_y);
    }*/

}
