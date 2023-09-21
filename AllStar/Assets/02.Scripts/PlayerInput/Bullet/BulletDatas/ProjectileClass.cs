using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileClass
{
    public ParticleSystem explosionEffect;
    public abstract void SetDefaultValue(float weaponDMG, float explosionRange);
    public abstract float GetTotalDamage(float playerDamage);
}
public class explosionType : ProjectileClass
{
    public float explosionRange;
    public float explosionDamage;
    public override void SetDefaultValue(float exDamage,float exRange)
    {
        explosionDamage = exDamage;
        explosionRange = exRange;
        //이거 데이터테이블에 연결해야함

    }
    public override float GetTotalDamage(float playerDamage)
    {
        //플레이어 데미지와 폭발데미지만, 충돌데미지는 아래에서 따로 계산
        return explosionDamage + playerDamage;
    }
}
public class basicBullet : ProjectileClass
{
    public override void SetDefaultValue(float weaponDMG, float explosionRange)
    {
        
    }
    public override float GetTotalDamage(float playerDamage)
    {
        return playerDamage;
    }
}
