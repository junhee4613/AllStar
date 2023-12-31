using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GunBase
{
    public GunStat stat;
    public Mesh gunMesh;
    public Material gunMat;
    [SerializeField]public (MeshFilter, MeshRenderer) playerWeapon;
    //세팅 
    public void StartSetting()
    {
        stat = new GunStat();
        stat.weaponIndex = 254;
        //인덱스 254는 무기가 없는 상태
    }
    public void GunShot(Vector3 firePos,float rotTemp)
    {
        if (Managers.DataManager.Datas.TryGetValue(stat.codeName + "_Bullet", out UnityEngine.Object Result))
        {
            if (stat.shotType == ShotType.singleShot)
            {
                Debug.Log(Result.ToString());
                GameObject bulletTemp = Managers.Pool.Pop(Result as GameObject);
                /*                    bulletTemp.GetComponent<Bullet>().BulletSetting(in playerWeapons[nowWeapon].stat, playerWeapons[nowWeapon].GetTotalCollDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance));
                                    if (playerWeapons[nowWeapon].stat.bulletType == bulletTypeEnum.explosion)
                                    {
                                        bulletTemp.GetComponent<Bullet>().BulletSetting(in playerWeapons[nowWeapon].stat, playerWeapons[nowWeapon].GetTotalCollDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance), playerWeapons[nowWeapon].GetTotalExDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance));
                                    }*/
                bulletTemp.transform.position = firePos;
                bulletTemp.transform.rotation = Quaternion.Euler(bulletTemp.transform.rotation.eulerAngles.x, rotTemp, bulletTemp.transform.rotation.eulerAngles.z);
            }
            else if (stat.shotType == ShotType.multiShot)
            {
                MultiShot tempShotType = stat.shotStatus as MultiShot;
                float rootedCase = MathF.Sqrt(tempShotType.fragmentCount);
                int bulletCount = (int)tempShotType.fragmentCount;
                for (float i = -rootedCase / 2; i < rootedCase / 2; i++)
                {
                    for (float E = -rootedCase / 2; E < rootedCase / 2; E++)
                    {
                        bulletCount -= 1;
                        if (bulletCount >= 0)
                        {
                            Debug.Log("총알생성");
                            GameObject tempBullet = Managers.Pool.Pop(Result as GameObject);
                            tempBullet.transform.position = firePos;
                            tempBullet.transform.rotation = Quaternion.Euler((i * tempShotType.fireAngle), (E * tempShotType.fireAngle) + rotTemp, 0);
                        }

                    }
                }
            }
        }
        Managers.Sound.SFX_Sound(Managers.DataManager.Datas[stat.codeName + "_Fire_Sound"] as AudioClip);
    }
    public void ResetGunSlot(Vector3 pos)
    {
        GameObject tempOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["WeaponItem"] as GameObject);
        byte tempIndex = stat.weaponIndex;
        WeaponData tempDataWeap = Managers.DataManager.weaponTable[tempIndex];
        WeaponItem tempItemCompo = tempOBJ.GetComponent<WeaponItem>();
        tempItemCompo.SetItemModel(Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mat"] as Material,
            Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mesh"] as Mesh, tempIndex);
        tempOBJ.transform.position = pos;
        tempItemCompo.itemIndex = tempIndex;
        tempItemCompo.type = ItemTypeEnum.weapon;
        if (stat.isValueChanged)
        {
            if (tempItemCompo.weaponInfo == null)
            {
                tempItemCompo.weaponInfo = new GunStat();
            }
            tempItemCompo.weaponInfo.name = stat.name;
            tempItemCompo.weaponInfo.weaponIndex = stat.weaponIndex;
            tempItemCompo.weaponInfo.codeName = stat.codeName;
            tempItemCompo.weaponInfo.bulletSpeed = stat.bulletSpeed;
            tempItemCompo.weaponInfo.fireSpeed = stat.fireSpeed;
            tempItemCompo.weaponInfo.removeTimer = stat.removeTimer;
            tempItemCompo.weaponInfo.bulletDamage = stat.bulletDamage;
            tempItemCompo.weaponInfo.shotType = stat.shotType;
            tempItemCompo.weaponInfo.bulletType = stat.bulletType;
            tempItemCompo.weaponInfo.isValueChanged = stat.isValueChanged;
            tempItemCompo.weaponInfo.projectileStat = stat.projectileStat;
            tempItemCompo.weaponInfo.shotStatus = stat.shotStatus;
        }
        stat.name = default;
        stat.weaponIndex = 254;
        stat.codeName = default;
        stat.bulletSpeed = default;
        stat.fireSpeed = default;
        stat.removeTimer = default;
        stat.bulletDamage = default;
        stat.shotType = ShotType.none;
        stat.bulletType = bulletTypeEnum.none;
        stat.isValueChanged = default;
        GunBase[] tempGunbase = Managers.GameManager.playerWeapons;
        byte indexCheck = 0;
        foreach (var item in tempGunbase)
        {
            if (item == this)
            {
                Managers.UI.WeaponInventoryImageChanges(indexCheck, stat.codeName);
                break;
            }
            indexCheck++;
        }
        gunMat = null;
        gunMesh = null;
        //254는 무기가 없는 상태

    }
    public void SetBasicValue(byte weaponIndex, Action doneCheck = null)
    {
        Debug.Log(this.GetType());
        WeaponData tempData = Managers.DataManager.weaponTable[weaponIndex];
        stat.isValueChanged = false;
        stat.weaponIndex = tempData.itemnum;
        stat.flavorText = tempData.flavorText;
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
        switch (stat.shotType)
        {
            case ShotType.multiShot:
                MultiShot tempShot = new MultiShot();
                tempShot.fireAngle = tempData.fireAngle;
                tempShot.fragmentCount = tempData.fragmentCount;
                stat.shotStatus = tempShot;
                //기능 추가 필요
                break;
            case ShotType.singleShot:
                stat.shotStatus = new SingleShot();
                //기능 추가 필요
                break;
        }
        stat.bulletSpeed = tempData.bulletspeed;
        stat.fireSpeed = tempData.firespeed;
        stat.removeTimer = tempData.removetimer;
        stat.bulletDamage = tempData.collisiondamage;
        GunBase[] tempGunbase = Managers.GameManager.playerWeapons;
        byte indexCheck = 0;
        foreach (var item in tempGunbase)
        {
            if (item == this)
            {
                Managers.UI.WeaponInventoryImageChanges(indexCheck,stat.codeName);
                break;
            }
            indexCheck++;
        }
        gunMesh = Managers.DataManager.Datas[stat.codeName + "_Mesh"] as Mesh;
        gunMat = Managers.DataManager.Datas[stat.codeName+"_Mat"] as Material;
        doneCheck?.Invoke();
    }
    public void ChangeWeaponModel()
    {
        playerWeapon.Item1.mesh = gunMesh;
        playerWeapon.Item2.material = gunMat;
        
    }
    public float GetTotalCollDamage(in float playerDMG, in float criDamage, in float criChance, ref bool isCritical)
    {
        float totalDMG = stat.bulletDamage + playerDMG;
        if (UnityEngine.Random.Range((float)0,1) <= criChance/100)
        {
            Debug.Log("크리발동");
            isCritical = true;
            return totalDMG+(totalDMG * (criDamage/100));
        }
        Debug.Log("크리아님");
        isCritical = false;
        return totalDMG;
    }
    public Vector2 GetTotalExDamage(in float playerDMG,  in float criDamage,in float criChance,ref bool isCritical)
    {
        //x값 데미지,Y값 범위
        explosionType tempPro = stat.projectileStat as explosionType;
        if (UnityEngine.Random.Range((float)0,1) <= criChance/100)
        {
            Debug.Log("크리발동");
            isCritical = true;
            return new Vector2(tempPro.explosionDamage + playerDMG + ((tempPro.explosionDamage + playerDMG) * (criDamage / 100)), tempPro.explosionRange);
        }
        Debug.Log("크리아님");
        isCritical = false;
        return new Vector2(tempPro.explosionDamage + playerDMG, tempPro.explosionRange);
    }
}
