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
    STOP,
    ROTATE,
    MOVE,
    SCALE_UP,
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