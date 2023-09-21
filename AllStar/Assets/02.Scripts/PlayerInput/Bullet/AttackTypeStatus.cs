using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTypeStatus
{
    public abstract void SetDefaultValue();
    public abstract float GetTotalDamage(float playerDamage);
}
public class explosionType : AttackTypeStatus
{
    public float explosionRange;
    public float explosionDamage;
    public ParticleSystem explosionEffect;
    public override void SetDefaultValue()
    {
        explosionDamage = 100;
        explosionRange = 3;
        //이거 데이터테이블에 연결해야함

    }
    public override float GetTotalDamage(float playerDamage)
    {
        return explosionDamage + playerDamage;
    }
}
public class basicBullet : AttackTypeStatus
{
    public override void SetDefaultValue()
    {
        
    }
    public override float GetTotalDamage(float playerDamage)
    {
        return playerDamage;
    }
}
public enum BulletType
{
    explosion, basicBullet
}