using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileClass
{
    public abstract float GetTotalDamage(float playerDamage);
}
public class explosionType : ProjectileClass
{
    public float explosionRange;
    public float explosionDamage;
    public void SetExplosionValue(float exRange,float exDamage)
    {
        explosionRange = exRange;
        explosionDamage = exDamage;
    }
    public override float GetTotalDamage(float playerDamage)
    {
        //�÷��̾� �������� ���ߵ�������, �浹�������� �Ʒ����� ���� ���
        return explosionDamage + playerDamage;
    }
}
public class basicBulletType : ProjectileClass
{
    public override float GetTotalDamage(float playerDamage)
    {
        return playerDamage;
    }
}
