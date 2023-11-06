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
    public static void SetBossFSMDefault(this Dictionary<string, BaseState> dict, ref Animator anim, GameObject target)
    {
        
        if (anim == null)
        {
            anim = GetOrAddCompo<Animator>(target);
        }
        
        dict.Add("idle", new GeneralFSM.Idle());
        dict.Add("simple_pattern1", new BossFSM.Simple_pattern1("simple_barrage1"));
        dict.Add("simple_pattern2", new BossFSM.Simple_pattern2("simple_barrage2"));
        dict.Add("simple_pattern3", new BossFSM.Simple_pattern3("simple_barrage3"));
        dict.Add("simple_pattern4", new BossFSM.Simple_pattern4("simple_laser"));
        dict.Add("simple_pattern5", new BossFSM.Simple_pattern5("simple_follow_laser")); ;
        dict.Add("simple_pattern6", new BossFSM.Simple_pattern6("simple_heal"));
        dict.Add("return", new BossFSM.Form_return("simple_heal_return"));
        dict.Add("heal_idle", new BossFSM.Heal_idle());
        dict.Add("hard_pattern1", new BossFSM.Hard_pattern1("hard_barrage1"));
        dict.Add("hard_pattern2", new BossFSM.Hard_pattern2("hard_barrage2"));
        dict.Add("hard_pattern3", new BossFSM.Hard_pattern3("hard_barrage3"));
        dict.Add("hard_pattern4", new BossFSM.Hard_pattern4("hard_laser"));

        foreach (var item in dict)
        {
            item.Value.animator = anim;
        }
    }
}