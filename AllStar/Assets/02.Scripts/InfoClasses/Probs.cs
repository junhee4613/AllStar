using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public float moveSpeed;
    public float HP;
    public float attackSpeed;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public Animator animator;
    public Dictionary<string,BaseState> states = new Dictionary<string, BaseState>();
    public BaseState nowState;
}
[System.Serializable]
public class PlayerOnlyStatus : Status
{
    public float dodgeCooltime;
    public float dodgeDistance;
    
}
public enum statusType
{
    moveSpeed,HP,attackSpeed,attackDamage,criticalChance,criticalDamage
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
public enum GeneralMonsters_Type        //일반몬스터 타입들 이 타입들로 몬스터 공격 방법을 정해줌
{
    NEAR,
    RANGED,
    SPECIAL
}
public enum bulletTypeEnum
{
    explosion, basicBullet
}
public enum ItemTypeEnum
{
    weapon, artifacts,consumer
}