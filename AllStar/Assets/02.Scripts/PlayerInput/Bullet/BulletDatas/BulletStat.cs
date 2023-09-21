using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStat : MonoBehaviour
{
    
    public float bulletSpeed;
    public float removeTimer;
    public float totalDamage;

    [Header("����ü Ÿ��")]
    public bulletTypeEnum bulletType;
    public ProjectileClass projectileStat;
    [Header("�߻� Ÿ��")]
    public ShotType shotType;
    public ShotStatus shotStatus;

}
