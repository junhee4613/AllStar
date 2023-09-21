using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    public BulletStat stat;
    public BulletSCROBJ data;

    //���� 
    private void Start()
    {
        
    }
    public virtual void SetBasicValue(string weaponName)
    {
        Debug.Log(this.GetType());
        BulletSCRData tempData = null;
        //Ȯ�� ��� Ŭ���� �̸��� ������ �� ������ ���������̺� �Ѿ� ������ ��ųʸ�ȭ �Ͽ� �Ѿ� Ŭ���� �̸��� ��ȹ���� ���� �����͸� �ҷ��½����� ����
        foreach (BulletSCRData target in data.Data)
        {
            if (target.name == weaponName)
            {
                tempData = target;
            }
        }
        switch (tempData.bulletType)
        {
            case bulletTypeEnum.explosion:
                stat.projectileStat = new explosionType();
                stat.projectileStat.SetDefaultValue(tempData.explosionDamage, tempData.explosionRange);
                break;
            case bulletTypeEnum.basicBullet:
                stat.projectileStat = new basicBullet();
                break;
        }
        switch (tempData.shotType)
        {
            case ShotType.multiShot:
                //��� �߰� �ʿ�
                break;
            case ShotType.singleShot:
                //��� �߰� �ʿ�
                break;
        }
        stat.bulletSpeed = tempData.bulletSpeed;
        stat.removeTimer = tempData.removeTimer;
        stat.totalDamage = tempData.totalDamage;
    }
}

public enum ShotType
{
    multiShot,singleShot
}
