using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunStat
{
    public string name;
    public byte weaponIndex = 254;
    public string codeName;
    public float bulletSpeed;
    public float fireSpeed;
    public float removeTimer;
    public float bulletDamage;
    public bool isValueChanged;
    //무기 값이 바뀔때 체크되는 불값
    [Header("투사체 타입")]
    public bulletTypeEnum bulletType;
    public ProjectileClass projectileStat;
    [Header("발사 타입")]
    public ShotType shotType;
    public ShotStatus shotStatus;
    public bool isEmptySlot()
    {
        if (weaponIndex == 254)
        {
            return true;
        }
        return false;
    }
}
