using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClassManaging
{
    public abstract void BasicValue();
}
public class Status : ClassManaging
{
    public float moveSpeed;
    public float HP;
    public float attackSpeed;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public override void BasicValue()
    {

    }
}
public enum statusType
{
    moveSpeed,HP,attackSpeed,attackDamage,criticalChance,criticalDamage
}
public enum ProjectileType
{
    s
    //Bullet �߻� �θ�Ŭ������ ���Ǵ� Enum��
}
