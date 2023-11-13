using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayerSkills.Skills;
using JetBrains.Annotations;

[System.Serializable]
public class GameManager
{
    public PlayerOnlyStatus PlayerStat = new PlayerOnlyStatus();
    public GunBase[] playerWeapons = new GunBase[3];
    public SkillBase[] playerSkills = new SkillBase[5];
    public float[] playerCooltimes;
    public ArtifactSlot[] playerArtifacts = new ArtifactSlot[6];
    public Dictionary<string,Status> monstersInScene = new Dictionary<string, Status>();
    public delegate void statChangeEvent();
    public event statChangeEvent OnIconChange;

    #region 스텟관련
    public void BasicPlayerStats(Action done)
    {
        //추후 데이터테이블에서 불러와야되므로 콜백으로 작업
        PlayerStat.maxHP = 100;
        PlayerStat.nowHP = 100;
        PlayerStat.moveSpeed = 2;
        PlayerStat.attackSpeed = 0.65f;
        PlayerStat.attackDamage = 10;
        PlayerStat.criticalChance = 10;
        PlayerStat.criticalDamage = 198.5f;
        PlayerStat.dodgeCooltime = 1;
        Managers.UI.hpbar.Item2.text = PlayerStat.nowHP + "/" + PlayerStat.maxHP;
        done?.Invoke();
        OnIconChange();
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
                Managers.UI.hpbar.Item1.maxValue = PlayerStat.maxHP;
                Managers.UI.hpbar.Item2.text = PlayerStat.nowHP + "/" + PlayerStat.maxHP;
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
                PlayerStat.maxHP = minusOper(PlayerStat.maxHP, addValue);
                Managers.UI.hpbar.Item1.maxValue = PlayerStat.maxHP;
                Managers.UI.hpbar.Item2.text = PlayerStat.nowHP + "/" + PlayerStat.maxHP;
                Managers.UI.hpbar.Item1.value = PlayerStat.nowHP;
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
            target.BulletSetting(in playerWeapons[slotNum].stat, playerWeapons[slotNum].GetTotalCollDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance,ref target.isCritical), 
                playerWeapons[slotNum].GetTotalExDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance, ref target.isExplosionCritical));
        }
        else if (playerWeapons[slotNum].stat.bulletType == bulletTypeEnum.basicBullet)
        {
            target.BulletSetting(in playerWeapons[slotNum].stat, playerWeapons[slotNum].GetTotalCollDamage(PlayerStat.attackDamage, PlayerStat.criticalDamage, PlayerStat.criticalChance, ref target.isCritical));
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
        if (playerArtifacts[itemIndex].artifactAmount == 0)
        {
            playerArtifacts[itemIndex].artifactAmount++;
            playerArtifacts[itemIndex].SetArtifact(Managers.DataManager.artifactTable[itemIndex]);
            AddStatus(playerArtifacts[itemIndex].data.statustype, playerArtifacts[itemIndex].data.value);
            Managers.UI.ArtifactInventoryImageChanges(itemIndex, playerArtifacts[itemIndex].data.codename);
        }
        else
        {
            Debug.Log(playerArtifacts[itemIndex].data.codename + "_" + playerArtifacts[itemIndex].artifactAmount);
            ReduceStatus(playerArtifacts[itemIndex].data.statustype, Managers.DataManager.artifactLevelTable[playerArtifacts[itemIndex].data.codename + "_" + playerArtifacts[itemIndex].artifactAmount].Value);
            playerArtifacts[itemIndex].artifactAmount++;
            AddStatus(playerArtifacts[itemIndex].data.statustype, Managers.DataManager.artifactLevelTable[playerArtifacts[itemIndex].data.codename + "_" + playerArtifacts[itemIndex].artifactAmount].Value);
            Managers.UI.artifactSet[itemIndex].AmountText.text = playerArtifacts[itemIndex].artifactAmount.ToString();
        }

/*        bool artInOtherPos = false;
        short testIndex = -1;
        for (byte i = 0; i < playerArtifacts.Length; i++)
        {
            if (playerArtifacts[i].data.itemnum == itemIndex&& playerArtifacts[i] != playerArtifacts[itemIndex])
            {
                testIndex = i;
                artInOtherPos = true;
                break;
            }
        }
        if ((playerArtifacts[itemIndex].data.itemnum == itemIndex|| playerArtifacts[itemIndex].artifactAmount == 0)&&!artInOtherPos)
        {
  
        }
        else if((playerArtifacts[itemIndex].data.itemnum != itemIndex&& playerArtifacts[itemIndex].data.itemnum != 254)||artInOtherPos)
        {
            if (testIndex != -1)
            {
                if (testIndex !=itemIndex)
                {
                    playerArtifacts[testIndex].artifactAmount++;
                    AddStatus(playerArtifacts[testIndex].data.statustype, playerArtifacts[testIndex].data.value);
                    Managers.UI.artifactSet[testIndex].AmountText.text = playerArtifacts[testIndex].artifactAmount.ToString();
                }
                else
                {
                    for (byte i = 0; i < playerArtifacts.Length; i++)
                    {
                        if (playerArtifacts[i].data.itemnum == 254)
                        {
                            playerArtifacts[i].artifactAmount++;
                            playerArtifacts[i].SetArtifact(Managers.DataManager.artifactTable[itemIndex]);
                            AddStatus(playerArtifacts[i].data.statustype, playerArtifacts[i].data.value);
                            Managers.UI.ArtifactInventoryImageChanges(i, playerArtifacts[itemIndex].data.codename);
                            break;
                        }
                    }
                }
            }
            for (byte i = 0; i < playerArtifacts.Length; i++)
            {
                if (playerArtifacts[i].data.itemnum == itemIndex)
                {

                }
                else if(playerArtifacts[i].data.itemnum == 254&&i != playerArtifacts[itemIndex].data.itemnum)
                {
                    playerArtifacts[i].artifactAmount++;
                    playerArtifacts[i].SetArtifact(Managers.DataManager.artifactTable[itemIndex]);
                    AddStatus(playerArtifacts[i].data.statustype, playerArtifacts[i].data.value);
                    Managers.UI.ArtifactInventoryImageChanges(i, playerArtifacts[itemIndex].data.codename);
                    break;
                }
            }
        }*/

    }

    public void ChangeSkillArray(byte skillIndex, byte skillIndex2)
    {
        SkillBase tempSkillbase = playerSkills[skillIndex];
        SkillBase tempSkillbase2 = playerSkills[skillIndex2];
        float tempCooltime = playerCooltimes[skillIndex];
        float tempCooltime2 = playerCooltimes[skillIndex2];

        playerCooltimes[skillIndex] = tempCooltime2;
        playerCooltimes[skillIndex2] = tempCooltime;
        playerSkills[skillIndex] = tempSkillbase2;
        playerSkills[skillIndex2] = tempSkillbase;
        //이건 그냥 제네릭으로 작업해도 되겠는데?
        Managers.UI.SetSkillIcons(skillIndex, playerSkills[skillIndex].skillInfo.codeName);
        Managers.UI.SetSkillIcons(skillIndex2, playerSkills[skillIndex2].skillInfo.codeName);
        
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
    public void ArtifactWaste(byte artifactArray,Vector3 PlayerPosition)
    {
        for (int i = 0; i < playerArtifacts[artifactArray].artifactAmount; i++)
        {
            GameObject tempOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
            byte tempIndex = playerArtifacts[artifactArray].data.itemnum;
            ArtifactData tempDataWeap = Managers.DataManager.artifactTable[tempIndex];
            ArtifactItem tempItemCompo = tempOBJ.GetComponent<ArtifactItem>();
            tempItemCompo.type = ItemTypeEnum.artifacts;
            tempItemCompo.itemIndex = tempIndex;
            Debug.Log("메쉬 받으면 밑에있는 주석 풀어야함");
            tempOBJ.transform.position = PlayerPosition;
            ReduceStatus(playerArtifacts[artifactArray].data.statustype, playerArtifacts[artifactArray].data.value);

        }
        playerArtifacts[artifactArray].artifactAmount = 0;
        /*        tempItemCompo.SetItemModel(Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mesh"] as Mesh, tempIndex);*/
        ArtifactRemoveOnly(artifactArray);
    }
    public void ArtifactRemoveOnly(byte artifactArray)
    {
        playerArtifacts[artifactArray].ResetArtifact(false);
        Managers.UI.ArtifactInventoryImageChanges(artifactArray,"Null");
    }
/*    public void ChageArtifact(byte itemIndex,byte artifactArray)
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
            int tempAmount;
            tempAmount = playerArtifacts[artifactArray1].artifactAmount;
            playerArtifacts[artifactArray1].artifactAmount = playerArtifacts[artifactArray2].artifactAmount;
            playerArtifacts[artifactArray2].artifactAmount = tempAmount;

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
        int tempAmount;
        if (itemIndex1 == 254)
        {
            tempAmount = playerArtifacts[artifactArray2].artifactAmount;
            playerArtifacts[artifactArray2].ResetArtifact(false);
            playerArtifacts[artifactArray1].SetArtifact(Managers.DataManager.artifactTable[itemIndex2]);

            playerArtifacts[artifactArray2].artifactAmount = 0;
            playerArtifacts[artifactArray1].artifactAmount = tempAmount;

            Managers.UI.ArtifactInventoryImageChanges(artifactArray1, playerArtifacts[artifactArray1].data.codename);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray2, "Null");
        }
        else if (itemIndex2 == 254)
        {
            tempAmount = playerArtifacts[artifactArray1].artifactAmount;
            playerArtifacts[artifactArray1].ResetArtifact(false);
            playerArtifacts[artifactArray2].SetArtifact(Managers.DataManager.artifactTable[itemIndex1]);
            
            

            playerArtifacts[artifactArray1].artifactAmount = 0;
            playerArtifacts[artifactArray2].artifactAmount = tempAmount;
            Managers.UI.ArtifactInventoryImageChanges(artifactArray2, playerArtifacts[artifactArray2].data.codename);
            Managers.UI.ArtifactInventoryImageChanges(artifactArray1, "Null");
        }
    }*/
    #endregion
}
