using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;
    //���� 
    public virtual void SetBasicValue(byte weaponIndex, Action doneCheck = null)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        WeaponData tempData = Managers.DataManager.weaponTable[weaponIndex];
        stat.bulletType = tempData.bullettype;
        stat.shotType = tempData.shottype;
        stat.codeName = tempData.codename;
        stat.name = tempData.name;
        switch (stat.bulletType)
        {
            case bulletTypeEnum.explosion:
                stat.projectileStat = new explosionType();
                explosionType tempEx = stat.projectileStat as explosionType;
                tempEx.SetExplosionValue(tempData.explosionrange, tempData.explosiondamage);
                break;
            case bulletTypeEnum.basicBullet:
                stat.projectileStat = new basicBulletType();
                break;
        }
        switch (tempData.shottype)
        {
            case ShotType.multiShot:
                //��� �߰� �ʿ�
                break;
            case ShotType.singleShot:
                //��� �߰� �ʿ�
                break;
        }
        stat.bulletSpeed = tempData.bulletspeed;
        stat.fireSpeed = tempData.firespeed;
        stat.removeTimer = tempData.removetimer;
        stat.bulletDamage = tempData.collisiondamage;
        doneCheck?.Invoke();
    }
    public float GetTotalCollDamage(in float playerDMG, in float criDamage, in float criChance)
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
    public Vector2 GetTotalExDamage(in float playerDMG,  in float criDamage,in float criChance)
    {
        //x�� ������,Y�� ����
        explosionType tempPro = stat.projectileStat as explosionType;
        if (UnityEngine.Random.Range((float)0,1) <= criChance/100)
        {
            Debug.Log("ũ���ߵ�");
            return new Vector2(tempPro.explosionDamage + playerDMG + ((tempPro.explosionDamage + playerDMG) * (criDamage / 100)), tempPro.explosionRange);
        }
        Debug.Log("ũ���ƴ�");
        return new Vector2(tempPro.explosionDamage + playerDMG, tempPro.explosionRange);
    }
}
