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
                        stat.projectileStat.SetDefaultValue(target.explosionDamage, target.explosionRange);
                        break;
                    case bulletTypeEnum.basicBullet:
                        stat.projectileStat = new basicBullet();
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
                stat.totalDamage = target.totalDamage;
            }
        }
        doneCheck?.Invoke();
    }
}

public enum ShotType
{
    multiShot,singleShot
}
