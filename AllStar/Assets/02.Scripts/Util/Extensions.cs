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
        //애니메이터가 있는지 없는지 분별
        if (anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        //중립FSM 상태값 추가 시 여기에 등록해줘야함
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
        //애니메이터가 있는지 없는지 분별
        if (anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        //플레이어FSM 상태값 추가 시 여기에 등록해줘야함
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
        //오버랩 기능을 사용할떄 매개변수 name값과 같은 이름의 오브젝트를 out으로 내보내줌
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
        //overlap 배열 내에 있는 콜라이더를 최신값과 이전값을 비교하여 달라진게 있으면 True를 반환,그게 아니면 false를 반환
        if (Enumerable.SequenceEqual(array, Physics.OverlapSphere(originPos, range)))
        {
            Debug.Log("시퀀스 펄스 반환");
            return false;
        }
        else
        {
            Debug.Log("시퀀스 트루 반환");
            return true;
        }
    }
}