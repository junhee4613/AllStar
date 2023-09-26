using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;
    //세팅 
    public virtual void SetBasicValue(string weaponName, Action doneCheck = null)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        //확인 결과 클래스 이름을 가져올 수 있으니 데이터테이블에 총알 스텟을 딕셔너리화 하여 총알 클래스 이름을 기획서와 맞춰 데이터를 불러는식으로 진행
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
                        //기능 추가 필요
                        break;
                    case ShotType.singleShot:
                        //기능 추가 필요
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
            Debug.Log("크리발동");
            return totalDMG+(totalDMG * (criDamage/100));
        }
        Debug.Log("크리아님");
        return totalDMG;
    }
}
