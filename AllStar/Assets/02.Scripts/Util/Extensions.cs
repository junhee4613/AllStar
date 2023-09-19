using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static Util;
using System.Linq;

public class Extensions
{

}
public static class FSMExtension
{
    public static void SetGeneralFSMDefault(this Dictionary<string, BaseState> dict,ref Animator anim,GameObject target)
    {
        //�ִϸ����Ͱ� �ִ��� ������ �к�
        if (anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        //�߸�FSM ���°� �߰� �� ���⿡ ����������
        dict.Add("attack", new GeneralFSM.Attack());
        dict.Add("damaged", new GeneralFSM.Damaged());
        dict.Add("run", new GeneralFSM.Run());
        dict.Add("die", new GeneralFSM.Die());
        dict.Add("idle", new GeneralFSM.Idle());
        foreach (var item in dict)
        {
            item.Value.animator = anim;
        }
    }
    public static void SetPlayerFSMDefault(this Dictionary<string, BaseState> dict,Animator anim,GameObject target)
    {
        //�ִϸ����Ͱ� �ִ��� ������ �к�
        if (anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        //�÷��̾�FSM ���°� �߰� �� ���⿡ ����������
        dict.Add("dodge", new PlayerFSM.Dodge());
        foreach (var item in dict)
        {
            item.Value.animator = anim;
        }
    }
}
public static class PhysicsExtension
{
    public static bool OverlapSearchTheOBJ(this Collider[] coll, out GameObject targetOBJ, string name = null)
    {
        //������ ����� ����ҋ� �Ű����� name���� ���� �̸��� ������Ʈ�� out���� ��������
        targetOBJ = null;
        foreach (var item in coll)
        {
            if (item.gameObject.name == name)
            {
                targetOBJ = item.gameObject;
                return true;
            }
        }

        return false;
    }
    public static bool IsChangedInArray(this Collider[] array, float range, Vector3 originPos)
    {
        //overlap �迭 ���� �ִ� �ݶ��̴��� �ֽŰ��� �������� ���Ͽ� �޶����� ������ True�� ��ȯ,�װ� �ƴϸ� false�� ��ȯ
        if (Enumerable.SequenceEqual(array, Physics.OverlapSphere(originPos, range)))
        {
            Debug.Log("������ �޽� ��ȯ");
            return false;
        }
        else
        {
            Debug.Log("������ Ʈ�� ��ȯ");
            return true;
        }
    }
}