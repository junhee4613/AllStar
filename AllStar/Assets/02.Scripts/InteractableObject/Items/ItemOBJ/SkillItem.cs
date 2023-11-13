using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : IItemBase
{
    protected override void Start()
    {
        GetItemBouce();
        base.itemIndex = (byte)Random.Range(0, Managers.DataManager.skillTable.Count);
        /*        SetItemModel(Managers.DataManager.Datas[Managers.DataManager.skillTable[randomValue].codeName + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[Managers.DataManager.skillTable[randomValue].codeName + "_Item_Mesh"] as Mesh, (byte)randomValue);*/

    }
    public override void UseItem<T>(ref T t)
    {
        PlayerSkills.Skills.SkillBase tempSkillbase;
        tempSkillbase = t as PlayerSkills.Skills.SkillBase;
        tempSkillbase.SkillSetting(Managers.DataManager.skillTable[itemIndex]);
        OBJPushOnly();
    }
    public void UseItemToUpGrade(PlayerSkills.Skills.SkillBase tempSkill)
    {
        tempSkill.SkillUpGrade();
        Managers.Pool.Push(this.gameObject);
    }
}
