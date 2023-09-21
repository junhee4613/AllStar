using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BulletSCROBJ : ScriptableObject
{
    [SerializeField]
    public BulletSCRData[] Data;
}
[System.Serializable]
public class BulletSCRData
{
    public string name;
    public float bulletSpeed;
    public float removeTimer;
    public float totalDamage;

    public float explosionRange;
    public float explosionDamage;
    public ParticleSystem explosionEffect;
    public bulletTypeEnum bulletType;
    public ShotType shotType;
}
