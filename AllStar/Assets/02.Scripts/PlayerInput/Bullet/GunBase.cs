using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    public BulletStat stat;
    public BulletSCROBJ data;

    //세팅 
    private void Start()
    {
        
    }
    public virtual void SetBasicValue(string weaponName)
    {
        Debug.Log(this.GetType());
        BulletSCRData tempData = null;
        //확인 결과 클래스 이름을 가져올 수 있으니 데이터테이블에 총알 스텟을 딕셔너리화 하여 총알 클래스 이름을 기획서와 맞춰 데이터를 불러는식으로 진행
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
                //기능 추가 필요
                break;
            case ShotType.singleShot:
                //기능 추가 필요
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
