using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullets
{
    public Mesh bulletModel;
    public Material bulletMat;
    public Bullets init()
    {
        BasicValue();
        Debug.Log(this+"��");
        return this;
    }
    //���� 
    public void BasicValue()
    {
        Debug.Log(this.GetType());
        //Ȯ�� ��� Ŭ���� �̸��� ������ �� ������ ���������̺� �Ѿ� ������ ��ųʸ�ȭ �Ͽ� �Ѿ� Ŭ���� �̸��� ��ȹ���� ���� �����͸� �ҷ��½����� ����
    }
}
public class Santan : Bullets
{
    
}
public class MachineGun : Bullets
{

}

