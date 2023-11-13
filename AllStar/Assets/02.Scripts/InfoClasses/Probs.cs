using PlayerSkills.SkillProbs;
using PlayerSkills.SkillProbs.BuffCon;
using PlayerSkills.SkillProbs.DeffenceCon;
using PlayerSkills.SkillProbs.OffenceCon;
using System;
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
    public Dictionary<string, BaseState> states = new Dictionary<string, BaseState>();
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
        Managers.UI.hpbar.Item1.maxValue = maxHP;
        Managers.UI.hpbar.Item1.value = nowHP;
        Managers.UI.hpbar.Item2.text = nowHP + "/" + maxHP;
    }
}
[System.Serializable]
public class Nomal_monster : Status
{
    public bool hit;
    public override void GetDamage(float Damage)
    {
        base.GetDamage(Damage);
        hit = true;
    }
}


public enum statType
{
    none, moveSpeed, HP, attackSpeed, attackDamage, criticalChance, criticalDamage
}
public class BulletStatus
{
    public float damage;
    public int bulletCount;
}
public enum Boss_Simple_Pattern
{
    BARRAGE1,
    BARRAGE2,
    BARRAGE3,
    LASER,
    FOLLOW_LASER,
    HEAL,
}
public enum Boss_Hard_Pattern
{
    BARRAGE1,
    BARRAGE2,
    BARRAGE3,
    LASER,
}

public enum MonsterPaattern_Base
{
    STOP,
    RUN,
    ATTACK
}
public enum GeneralMonsters_Type        //일반몬스터 타입들 이 타입들로 몬스터 공격 방법을 정해줌
{
    NEAR,
    RANGED,
    SPECIAL
}
public enum bulletTypeEnum
{
    none, explosion, basicBullet
}
public enum ItemTypeEnum
{
    weapon, artifacts, consumAble, skill
}
[System.Serializable]
public class ArtifactLevelTable
{
    public string SkillName;
    public float Value;
}

public enum ShotType
{
    none, multiShot, singleShot
}
public enum DraggingState
{
    none, weapon/*, artifact*/, skills
}

namespace PlayerSkills
{
    namespace Skills
    {
        [Serializable]
        public class SkillBase
        {
            [SerializeField]
            public SkillInfomation skillInfo = new SkillInfomation();
            public Transform playerTR;
            [SerializeField]
            public TypeClasses DetailTypes;

            public void SkillSetting(SkillDataJson jsonData)
            {
                skillInfo.ValueSetting(jsonData);
                switch (jsonData.skillType)
                {
                    case SkillType.offence:
                        DetailTypes = new OffenceSkillData();
                        OffenceSkillData tempAttackSkillData = DetailTypes as OffenceSkillData;
                        tempAttackSkillData.pc = playerTR.GetComponent<PlayerControler>();
                        break;
                    case SkillType.deffence:
                        DetailTypes = new DeffenceSkillData();
                        DeffenceSkillData tempData = DetailTypes as DeffenceSkillData;
                        tempData.pc = playerTR.GetComponent<PlayerControler>();
                        break;
                    case SkillType.buff:
                        DetailTypes = new BuffSkillData();
                        break;
                }
                DetailTypes.TypeValueSetting(jsonData);
            }
            public void SkillUpGrade()
            {
                skillInfo.skillLevel++;

            }
            public void UseSkill()
            {
                DetailTypes.UseSkill(skillInfo);
            }
        }
    }
    namespace SkillProbs
    {
        [System.Serializable]
        public class SkillDataJson
        {
            public uint skillIndex;
            public string skillName;
            public string codeName;
            public short skillLevel;
            public float skillValue;
            public float secondValue;
            public float coolTime;
            public string flavorText;
            public SkillType skillType;
            public OffenceSkillType OffenceSkillType;
            public OffenceRangeType OffenceRangeType;
            public DeffenceSkillType DeffenceSkillType;
            public statType BuffSkillType;
        }
        [System.Serializable]
        public class SkillInfomation
        {
            public uint skillIndex;
            public string skillName;
            public string codeName;
            public short skillLevel;
            public float skillValue;
            public float secondValue;
            public float coolTime;
            public string flavorText;
            public SkillType skillType;
            public void ValueSetting(SkillDataJson data)
            {
                Debug.Log(data.ToString());
                skillIndex = data.skillIndex;
                skillName = data.skillName;
                codeName = data.codeName;
                skillLevel = data.skillLevel;
                skillValue = data.skillValue;
                secondValue = data.secondValue;
                coolTime = data.coolTime;
                flavorText = data.flavorText;
                skillType = data.skillType;
            }

        }
        public abstract class TypeClasses
        {
            public abstract void TypeValueSetting(SkillDataJson data);
            public abstract void UseSkill(SkillInfomation skillValue);
        }

        public enum SkillType
        {
            offence, deffence, buff
        }
        namespace OffenceCon
        {
            public class OffenceSkillData : TypeClasses
            {
                public OffenceSkillType DamageType;
                public OffenceRangeType RangeType;
                public PlayerControler pc;
                public override void TypeValueSetting(SkillDataJson data)
                {
                    DamageType = data.OffenceSkillType;
                    RangeType = data.OffenceRangeType;
                }
                public override void UseSkill(SkillInfomation skillValue)
                {
                    switch (RangeType)
                    {
                        case OffenceRangeType.none:
                            break;
                        case OffenceRangeType.Ranged:
                            Debug.Log("원거리 공격 구현 필요");
                            break;
                        case OffenceRangeType.Melee:
                            Debug.Log("근거리 공격 구현 필요");
                            break;
                        default:
                            break;
                    }
                }
            }
            public enum OffenceSkillType
            {
                none, Single, Area
                //단일,원거리
            }
            public enum OffenceRangeType
            {
                none, Ranged, Melee
            }
        }
        namespace DeffenceCon
        {
            public class DeffenceSkillData : TypeClasses
            {
                public DeffenceSkillType DeffSkillType;
                public PlayerControler pc;
                public override void TypeValueSetting(SkillDataJson data)
                {
                    DeffSkillType = data.DeffenceSkillType;
                }
                public override void UseSkill(SkillInfomation skillValue)
                {
                    switch (DeffSkillType)
                    {
                        case DeffenceSkillType.none:
                            break;
                        case DeffenceSkillType.dodge:
                            pc.fsmChanger(pc.stat.states["dodge"], skillValue.skillValue, skillValue.secondValue);
                            break;
                        case DeffenceSkillType.telleport:
                            pc.fsmChanger(pc.stat.states["tellleport"]);
                            //텔레포트 스테이트 딕셔너리에 추가 필요
                            break;
                        case DeffenceSkillType.deffence:
                            pc.fsmChanger(pc.stat.states["deffence"]);
                            break;
                        default:
                            break;
                    }

                }
            }

            public enum DeffenceSkillType
            {
                none, dodge, telleport, deffence
            }
        }
        namespace BuffCon
        {
            using System.Threading.Tasks;
            public class BuffSkillData : TypeClasses
            {
                private statType buffSkillType;
                public override void TypeValueSetting(SkillDataJson data)
                {
                    buffSkillType = data.BuffSkillType;
                }
                public override void UseSkill(SkillInfomation skillValue)
                {
                    Managers.GameManager.AddStatus(buffSkillType, skillValue.skillValue);
                    Debug.Log("실행");
                    var result = Timer(buffSkillType, skillValue.skillValue, skillValue.secondValue);
                }
                private async Task Timer(statType type, float value, float time)
                {
                    float beforeConvert = time * 1000;
                    int timeConvert = (int)beforeConvert;
                    await Task.Delay(timeConvert);
                    Managers.GameManager.ReduceStatus(type, value);
                }
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