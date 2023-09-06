using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public float moveSpeed;
    public float HP;
    public float attackSpeed;
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
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
