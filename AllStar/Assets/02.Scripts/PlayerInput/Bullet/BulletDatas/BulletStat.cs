using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStat
{
    public string name;
    public float bulletSpeed;
    public float fireSpeed;
    public float removeTimer;
    public float totalDamage;
    [Header("����ü Ÿ��")]
    public bulletTypeEnum bulletType;
    public ProjectileClass projectileStat;
    [Header("�߻� Ÿ��")]
    public ShotType shotType;
    public ShotStatus shotStatus;
    public bool isEmptySlot()
    {
        if (bulletSpeed == 0&&removeTimer == 0&&totalDamage==0)
        {
            return true;
        }
        return false;
    }
}
