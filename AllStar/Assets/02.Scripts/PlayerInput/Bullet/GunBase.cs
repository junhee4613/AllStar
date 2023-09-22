using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;

    //���� 
    public virtual void SetBasicValue(string weaponName)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        //Ȯ�� ��� Ŭ���� �̸��� ������ �� ������ ���������̺� �Ѿ� ������ ��ųʸ�ȭ �Ͽ� �Ѿ� Ŭ���� �̸��� ��ȹ���� ���� �����͸� �ҷ��½����� ����
        foreach (BulletSCRData target in Managers.DataManager.testTDataBase.Data)
        {
            if (target.name.Contains(weaponName))
            {
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
                stat.removeTimer = target.removeTimer;
                stat.totalDamage = target.totalDamage;
            }
        }

    }
}

public enum ShotType
{
    multiShot,singleShot
}
