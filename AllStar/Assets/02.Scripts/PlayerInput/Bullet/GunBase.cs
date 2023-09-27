using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;
    //세팅 
    public virtual void SetBasicValue(byte weaponIndex, Action doneCheck = null)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        WeaponData tempData = Managers.DataManager.weaponTable[weaponIndex];
        stat.codeName = tempData.codename;
        stat.name = tempData.name;
        switch (tempData.bullettype)
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
                //기능 추가 필요
                break;
            case ShotType.singleShot:
                //기능 추가 필요
                break;
        }
        stat.bulletSpeed = tempData.bulletspeed;
        stat.fireSpeed = tempData.firespeed;
        stat.removeTimer = tempData.removetimer;
        stat.bulletDamage = tempData.collisiondamage;
        doneCheck?.Invoke();
    }
    public float GetTotalDamage(float playerDMG,float criDamage,float criChance)
    {
        float totalDMG = stat.bulletDamage + playerDMG;
        if (UnityEngine.Random.Range((float)0,1) <= criChance/100)
        {
            Debug.Log("크리발동");
            return totalDMG+(totalDMG * (criDamage/100));
        }
        Debug.Log("크리아님");
        return totalDMG;
    }
}
