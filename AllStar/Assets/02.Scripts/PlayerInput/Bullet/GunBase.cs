using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunBase
{
    public BulletStat stat;

    //세팅 
    public virtual void SetBasicValue(string weaponName)
    {
        stat = new BulletStat();
        Debug.Log(this.GetType());
        //확인 결과 클래스 이름을 가져올 수 있으니 데이터테이블에 총알 스텟을 딕셔너리화 하여 총알 클래스 이름을 기획서와 맞춰 데이터를 불러는식으로 진행
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
                        //기능 추가 필요
                        break;
                    case ShotType.singleShot:
                        //기능 추가 필요
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
