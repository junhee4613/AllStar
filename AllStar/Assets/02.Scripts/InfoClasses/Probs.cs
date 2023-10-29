using PlayerSkills.SkillProbs;
using PlayerSkills.SkillProbs.DeffenceCon;
using PlayerSkills.SkillProbs.OffenceCon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public float moveSpeed;
    public float maxHP;
    public float nowHP;
    public float attackSpeed;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public Animator animator;
    public Dictionary<string,BaseState> states = new Dictionary<string, BaseState>();
    public BaseState nowState;
    public virtual void GetDamage(float Damage)
    {
        nowHP -= Damage;
    }
}
[System.Serializable]
public class PlayerOnlyStatus : Status
{
    public float dodgeCooltime;
    public override void GetDamage(float Damage)
    {
        nowHP -= Damage;
        Managers.UI.hpbar.maxValue = maxHP;
        Managers.UI.hpbar.value = nowHP;
    }
}

public enum statType
{
    none,moveSpeed,HP,attackSpeed,attackDamage,criticalChance,criticalDamage
}
public class BulletStatus
{
    public float damage;
    public int bulletCount;
}
public enum BossAttackPattern
{
    BARRAGE1,
    BARRAGE2,
    BARRAGE3,
    SIMPLE_ATTACK,
    SIMPLE_BULLET,
    STOP,

}
public enum MonsterPaattern_Base
{
    STOP,
    RUN,
    ATTACK
}
public enum GeneralMonsters_Type        //�Ϲݸ��� Ÿ�Ե� �� Ÿ�Ե�� ���� ���� ����� ������
{
    NEAR,
    RANGED,
    SPECIAL
}
public enum bulletTypeEnum
{
    none,explosion, basicBullet
}
public enum ItemTypeEnum
{
    weapon, artifacts,consumAble
}

public enum ShotType
{
    none,multiShot, singleShot
}
public enum DraggingState
{
    none,weapon,artifact
}

namespace PlayerSkills
{
    namespace Skills
    {
        public abstract class SkillBase
        {
            [SerializeField]
            public SkillInfomation skillInfo;
            [SerializeField]
            public TypeClasses DetailTypes;
        }
    }
    namespace SkillProbs
    {
        public class SkillSet
        {
            public ItemUI.ItemIconSet UISets;
        }
        [System.Serializable]
        public class SkillInfomation
        {
            [SerializeField]
            public uint skillIndex;
            [SerializeField]
            public string skillName;
            [SerializeField]
            public float skilValue;
            [SerializeField]
            public string flavorText;
            [SerializeField]
            public SkillType SkillType;

            
        }
        public abstract class TypeClasses
        {
            public abstract void Setting();
        }

        public enum SkillType
        {
            offence,deffence,buff
        }
        namespace OffenceCon
        {
            public class OffenceSkillData : TypeClasses
            {
                public OffenceSkillType DamageType;
                public OffenceRangeType RangeType;
                public override void Setting()
                {

                }
            }
            public enum OffenceSkillType
            {
                Single,Area
                    //����,���Ÿ�
            }
            public enum OffenceRangeType
            {
                Ranged,Melee
            }
        }
        namespace DeffenceCon
        {
            public class DeffenceSkillData : TypeClasses
            {
                public DeffenceSkillType DamageType;
                public override void Setting()
                {

                }
            }
            public enum DeffenceSkillType
            {
                dodge,telleport,deffence
            }
        }
        namespace BuffCon
        {
            public class BuffSkillData : TypeClasses
            {
                public BuffSkillType buffSkillType;
                public override void Setting()
                {

                }
            }
            public enum BuffSkillType
            {
                changeBulletType,AD,criChance,criDamage,moveSpeed,heal,HP
            }
        }

    }
}

namespace ItemUI
{
    using TMPro;
    using UnityEngine.UI;
    [System.Serializable]
    public class ItemIconSet
    {
        public Image IconIMG;
        public TextMeshProUGUI AmountText;
    }
}