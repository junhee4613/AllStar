using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullets : ClassManaging
{
    public ProjectileType bulletType;
    //���� 
    public override void BasicValue()
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

