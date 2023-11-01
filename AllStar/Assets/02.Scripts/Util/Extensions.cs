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
    public static void SetMonsterFSMDefault(this Dictionary<string, BaseState> dict,ref Animator anim, GameObject target)
    {
        if(anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        dict.Add("walk", new MonsterFSM.Walk());

    }
}