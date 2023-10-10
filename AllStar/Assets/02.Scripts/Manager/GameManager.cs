using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class GameManager
{
    public PlayerOnlyStatus PlayerStat = new PlayerOnlyStatus();
    public GunBase[] playerWeapons = new GunBase[3];
    public ArtifactSlot[] playerArtifacts = new ArtifactSlot[20];
    public Dictionary<string,Status> monstersInScene = new Dictionary<string, Status>();
    public delegate void statChangeEvent();
    public event statChangeEvent OnIconChange;

    #region 스텟관련
    public void BasicPlayerStats(Action done)
    {
        //추후 데이터테이블에서 불러와야되므로 콜백으로 작업
        Managers.DataManager.Init(()=> {
            PlayerStat.maxHP = 100;
            PlayerStat.nowHP = 100;
            PlayerStat.moveSpeed = 2;
            PlayerStat.attackSpeed = 0.65f;
            PlayerStat.attackDamage = 10;
            PlayerStat.criticalChance = 10;
            PlayerStat.criticalDamage = 198.5f;
            PlayerStat.dodgeCooltime = 1;
            done?.Invoke();
            OnIconChange();
        });
    }
    public void AddStatus(statType type, float addValue)
    {
        switch (type)
        {
            case statType.moveSpeed:
                PlayerStat.moveSpeed = multipleOper( PlayerStat.moveSpeed, addValue,2);
                break;
            case statType.HP:
                PlayerStat.maxHP = sumOper( PlayerStat.maxHP, addValue);
                break;
            case statType.attackSpeed:
                PlayerStat.attackSpeed = multipleOper( PlayerStat.attackSpeed, addValue,0.65f);
                break;
            case statType.attackDamage:
                PlayerStat.attackDamage = sumOper( PlayerStat.attackDamage, addValue);
                break;
            case statType.criticalChance:
                PlayerStat.criticalChance = sumOper( PlayerStat.criticalChance, addValue);
                break;
            case statType.criticalDamage:
                PlayerStat.criticalDamage = multipleOper( PlayerStat.criticalDamage, addValue, 198.5f);
                break;
        }
        OnIconChange();
    }
    public void ReduceStatus(statType type, float addValue)
    {
        switch (type)
        {
            case statType.moveSpeed:
                PlayerStat.moveSpeed = divisionOper(PlayerStat.moveSpeed, addValue,2);
                break;
            case statType.HP:
                PlayerStat.nowHP = minusOper(PlayerStat.nowHP, addValue);
                break;
            case statType.attackSpeed:
                PlayerStat.attackSpeed = divisionOper(PlayerStat.attackSpeed, addValue,0.65f);
                break;
            case statType.attackDamage:
                PlayerStat.attackDamage = minusOper(PlayerStat.attackDamage, addValue);
                break;
            case statType.criticalChance:
                PlayerStat.criticalChance = minusOper(PlayerStat.criticalChance, addValue);
                break;
            case statType.criticalDamage:
                PlayerStat.criticalDamage = divisionOper(PlayerStat.criticalDamage, addValue, 198.5f);
                break;
        }
        OnIconChange();
    }
    private float sumOper( float nowValue,float addValue)
    {
        //합연산 데미지
        nowValue += addValue;
        return nowValue;
    }
    private float multipleOper( float nowValue, float addValue, float defaultValue)
    {
        //곱연산 스텟
        //퍼센트만큼 

        nowValue += (defaultValue * ((addValue/100f)+1))-defaultValue;
        return nowValue;
    }
    private float divisionOper( float nowValue, float reduceValue, float defaultValue)
    {
        //곱연산 능력치 데미지 감소
        //defaultValue에는 기본능력치 대입해야함
        if (nowValue- ((defaultValue * ((reduceValue / 100f) + 1)) - defaultValue) > defaultValue)
        {
            nowValue -= ((defaultValue*((reduceValue / 100f) + 1))-defaultValue);
            return nowValue;
        }
        else
        {
            return defaultValue;
        }

    }
    private float minusOper(float nowValue, float reduceValue)
    {
        //합연산 능력치 감소
        if (nowValue - reduceValue >0)
        {
            return nowValue -reduceValue;
        }
        else
        {
            return nowValue;
        }
    }
    #endregion
    #region 무기관련
    public void SetBullet(in string bulletName,ref Bullet target)
    {
        byte slotNum = GetWeaponSlot(bulletName);
       
        if (playerWeapons[slotNum].stat.bulletType == bulletTypeEnum.explosion)
        {
            target.BulletSetting(in playerWeapons[slotNum].stat, playerWeapons[slotNum].GetTotalCollDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance), 
                playerWeapons[slotNum].GetTotalExDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance));
        }
        else if (playerWeapons[slotNum].stat.bulletType == bulletTypeEnum.basicBullet)
        {
            target.BulletSetting(in playerWeapons[slotNum].stat, playerWeapons[slotNum].GetTotalCollDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance));
        }
    }
    public byte GetWeaponSlot(in string bulletName)
    {
        for (byte i = 0; i < playerWeapons.Length; i++)
        {
            if (bulletName.Contains(playerWeapons[i].stat.codeName))
            {
                return i;
            }
        }
        return 255;
    }
    #endregion
    #region 아티펙트 관련
    public void ArtifactEquipOnly(byte itemIndex)
    {
        //앞에서부터 채움
        //254는 빈칸 255는 어레이가 빈곳이 없을경우 artifactArray에 반환
        byte artifactArray = 255;
        for (byte i = 0; i < playerArtifacts.Length; i++)
        {
            artifactArray = 254 == playerArtifacts[i].data.itemnum ? i : (byte)255;
            if (artifactArray == i) break;
        }
        if (artifactArray != 255)
        {
            playerArtifacts[artifactArray].SetArtifact(Managers.DataManager.artifactTable[itemIndex]);
            AddStatus(playerArtifacts[artifactArray].data.statustype, playerArtifacts[artifactArray].data.value);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray,playerArtifacts[artifactArray].data.codename);
        }

    }
    public void ChangeWeaponArray(byte weaponIndex,byte weaponIndex2)
    {
        GunBase tempGunbase = playerWeapons[weaponIndex];
        GunBase tempGunbase2 = playerWeapons[weaponIndex2];
        playerWeapons[weaponIndex] = tempGunbase2;
        playerWeapons[weaponIndex2] = tempGunbase;
        Managers.UI.WeaponInventoryImageChanges(weaponIndex, playerWeapons[weaponIndex].stat.codeName);
        Managers.UI.WeaponInventoryImageChanges(weaponIndex2, playerWeapons[weaponIndex2].stat.codeName);
    }
    public void ArtifactRemoveOnly(byte artifactArray)
    {
        ReduceStatus(playerArtifacts[artifactArray].data.statustype, playerArtifacts[artifactArray].data.value);
        playerArtifacts[artifactArray].ResetArtifact(false);
        Managers.UI.ArtifactInventoryImageChanges(artifactArray,"Null");
    }
    public void ChageArtifact(byte itemIndex,byte artifactArray)
    {
        ReduceStatus(playerArtifacts[artifactArray].data.statustype, playerArtifacts[artifactArray].data.value);
        playerArtifacts[artifactArray].data = Managers.DataManager.artifactTable[itemIndex];
        AddStatus(playerArtifacts[artifactArray].data.statustype, playerArtifacts[artifactArray].data.value);
        Managers.UI.ArtifactInventoryImageChanges(artifactArray, playerArtifacts[artifactArray].data.codename);
    }
    public void BothChageArtifact(byte itemIndex1,byte artifactArray1,byte itemIndex2,byte artifactArray2)
    {
        if (itemIndex1 != 254&& itemIndex2 != 254)
        {
            ReduceStatus(playerArtifacts[artifactArray1].data.statustype, playerArtifacts[artifactArray1].data.value);
            ReduceStatus(playerArtifacts[artifactArray2].data.statustype, playerArtifacts[artifactArray2].data.value);

            AddStatus(playerArtifacts[artifactArray1].data.statustype, playerArtifacts[artifactArray1].data.value);
            AddStatus(playerArtifacts[artifactArray2].data.statustype, playerArtifacts[artifactArray2].data.value);

            playerArtifacts[artifactArray1].SetArtifact(Managers.DataManager.artifactTable[itemIndex2]);
            playerArtifacts[artifactArray2].SetArtifact(Managers.DataManager.artifactTable[itemIndex1]);

            Managers.UI.ArtifactInventoryImageChanges(artifactArray1, playerArtifacts[artifactArray1].data.codename);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray2, playerArtifacts[artifactArray2].data.codename); 
        }
        else if (itemIndex1 != 254 || itemIndex2 != 254)
        {
            MoveArtifact(itemIndex1, artifactArray1, itemIndex2, artifactArray2);
        }
    }
    public void MoveArtifact(byte itemIndex1, byte artifactArray1, byte itemIndex2, byte artifactArray2)
    {
        if (itemIndex1 == 254)
        {
            ReduceStatus(playerArtifacts[artifactArray2].data.statustype, playerArtifacts[artifactArray2].data.value);
            AddStatus(playerArtifacts[artifactArray2].data.statustype, playerArtifacts[artifactArray2].data.value);
            playerArtifacts[artifactArray2].ResetArtifact(false);
            playerArtifacts[artifactArray1].SetArtifact(Managers.DataManager.artifactTable[itemIndex2]);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray1, playerArtifacts[artifactArray1].data.codename);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray2, "Null");
        }
        else if (itemIndex2 == 254)
        {
            ReduceStatus(playerArtifacts[artifactArray1].data.statustype, playerArtifacts[artifactArray1].data.value);
            AddStatus(playerArtifacts[artifactArray1].data.statustype, playerArtifacts[artifactArray1].data.value);
            playerArtifacts[artifactArray1].ResetArtifact(false);
            playerArtifacts[artifactArray2].SetArtifact(Managers.DataManager.artifactTable[itemIndex1]);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray2, playerArtifacts[artifactArray2].data.codename);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray1, "Null");
        }
    }
    #endregion
}
