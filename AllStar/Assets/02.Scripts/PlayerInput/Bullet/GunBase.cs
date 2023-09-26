using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;
    //���� 
    public virtual void SetBasicValue(string weaponName, Action doneCheck = null)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        //Ȯ�� ��� Ŭ���� �̸��� ������ �� ������ ���������̺� �Ѿ� ������ ��ųʸ�ȭ �Ͽ� �Ѿ� Ŭ���� �̸��� ��ȹ���� ���� �����͸� �ҷ��½����� ����
        foreach (BulletSCRData target in Managers.DataManager.testTDataBase.Data)
        {
            if (target.name.Contains(weaponName))
            {
                stat.name = weaponName;
                switch (target.bulletType)
                {
                    case bulletTypeEnum.explosion:
                        stat.projectileStat = new explosionType();
                        explosionType tempEx = stat.projectileStat as explosionType;
                        tempEx.SetExplosionValue(target.explosionRange, target.explosionDamage);
                        break;
                    case bulletTypeEnum.basicBullet:
                        stat.projectileStat = new basicBulletType();
                        break;
                }
                switch (target.shotType)
                {
                    case ShotType.multiShot:
                        //��� �߰� �ʿ�
                        break;
                    case ShotType.singleShot:
                        //��� �߰� �ʿ�
                        break;
                }
                stat.bulletSpeed = target.bulletSpeed;
                stat.fireSpeed = target.fireSpeed;
                stat.removeTimer = target.removeTimer;
                stat.bulletDamage = target.totalDamage;
            }
        }
        doneCheck?.Invoke();
    }
    public float GetTotalDamage(float playerDMG,float criDamage,float criChance)
    {
        float totalDMG = stat.bulletDamage + playerDMG;
        if (UnityEngine.Random.Range((float)0,1) <= criChance/100)
        {
            Debug.Log("ũ���ߵ�");
            return totalDMG+(totalDMG * (criDamage/100));
        }
        Debug.Log("ũ���ƴ�");
        return totalDMG;
    }
}
