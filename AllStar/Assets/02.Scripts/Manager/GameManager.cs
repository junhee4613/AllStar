using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class GameManager
{
    public PlayerOnlyStatus PlayerStat = new PlayerOnlyStatus();
    public GunBase[] playerWeapons = new GunBase[3];
    public void BasicPlayerStats(Action done)
    {
        //���� ���������̺��� �ҷ��;ߵǹǷ� �ݹ����� �۾�
        Managers.DataManager.Init(()=> {
            PlayerStat.moveSpeed = 2;
            PlayerStat.HP = 10;
            PlayerStat.attackSpeed = 0.65f;
            PlayerStat.attackDamage = 10;
            PlayerStat.criticalChance = 10;
            PlayerStat.criticalDamage = 10;
            PlayerStat.dodgeCooltime = 1;
            done?.Invoke();
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
                PlayerStat.HP = sumOper( PlayerStat.HP, addValue);
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
                PlayerStat.criticalDamage = multipleOper( PlayerStat.criticalDamage, addValue,10);
                break;
        }
    }
    private float sumOper( float nowValue,float addValue)
    {
        //�տ��� ������
        nowValue += addValue;
        return nowValue;
    }
    private float multipleOper( float nowValue, float addValue, float defaultValue)
    {
        //������ ����
        //�ۼ�Ʈ��ŭ 

        nowValue += (defaultValue * ((addValue/100f)+1))-defaultValue;
        return nowValue;
    }
    public void ReduceStatus(statType type, float addValue)
    {
        switch (type)
        {
            case statType.moveSpeed:
                PlayerStat.moveSpeed = divisionOper(PlayerStat.moveSpeed, addValue,2);
                break;
            case statType.HP:
                PlayerStat.HP = minusOper(PlayerStat.HP, addValue);
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
                PlayerStat.criticalDamage = divisionOper(PlayerStat.criticalDamage, addValue,10);
                break;
        }
    }
    private float divisionOper( float nowValue, float reduceValue, float defaultValue)
    {
        //������ �ɷ�ġ ������ ����
        //defaultValue���� �⺻�ɷ�ġ �����ؾ���
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
        //�տ��� �ɷ�ġ ����
        if (nowValue - reduceValue >0)
        {
            return nowValue -reduceValue;
        }
        else
        {
            return nowValue;
        }
    }
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
}
