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
    //���� ���� �ٲ� üũ�Ǵ� �Ұ�
    [Header("����ü Ÿ��")]
    public bulletTypeEnum bulletType;
    public ProjectileClass projectileStat;
    [Header("�߻� Ÿ��")]
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
