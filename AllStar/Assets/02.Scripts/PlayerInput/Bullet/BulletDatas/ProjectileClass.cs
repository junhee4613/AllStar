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
        //�̰� ���������̺� �����ؾ���

    }
    public override float GetTotalDamage(float playerDamage)
    {
        //�÷��̾� �������� ���ߵ�������, �浹�������� �Ʒ����� ���� ���
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
