using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class GameManager
{
    public Status PlayerStat = new Status();
    public void BasicPlayerStats(Action done)
    {
        //���� ���������̺��� �ҷ��;ߵǹǷ� �ݹ����� �۾�
        Managers.DataManager.Init(()=> {
            PlayerStat.moveSpeed = 10;
            PlayerStat.HP = 10;
            PlayerStat.attackSpeed = 0.65f;
            PlayerStat.attackDamage = 10;
            PlayerStat.criticalChance = 10;
            PlayerStat.criticalDamage = 10;
            PlayerStat.skillCoolTime = 2;
            done?.Invoke();
        });
    }
    public void AddStatus(statusType type, float addValue)
    {
        switch (type)
        {
            case statusType.moveSpeed:
                PlayerStat.moveSpeed = sumOper( PlayerStat.moveSpeed, addValue);
                break;
            case statusType.HP:
                PlayerStat.HP = sumOper( PlayerStat.HP, addValue);
                break;
            case statusType.attackSpeed:
                PlayerStat.attackSpeed = multipleOper( PlayerStat.attackSpeed, addValue,0.65f);
                break;
            case statusType.attackDamage:
                PlayerStat.attackDamage = sumOper( PlayerStat.attackDamage, addValue);
                break;
            case statusType.criticalChance:
                PlayerStat.criticalChance = sumOper( PlayerStat.criticalChance, addValue);
                break;
            case statusType.criticalDamage:
                PlayerStat.criticalDamage = multipleOper( PlayerStat.criticalDamage, addValue,10);
                break;
        }
    }
    public float sumOper( float nowValue,float addValue)
    {
        //�տ��� ������
        nowValue += addValue;
        return nowValue;
    }
    public float multipleOper( float nowValue, float addValue, float defaultValue)
    {
        //������ ������

        nowValue += (defaultValue * ((addValue/100f)+1))-defaultValue;
        return nowValue;
    }
    public void ReduceStatus(statusType type, float addValue)
    {
        switch (type)
        {
            case statusType.moveSpeed:
                PlayerStat.moveSpeed = minusOper(PlayerStat.moveSpeed, addValue);
                break;
            case statusType.HP:
                PlayerStat.HP = minusOper(PlayerStat.HP, addValue);
                break;
            case statusType.attackSpeed:
                PlayerStat.attackSpeed = divisionOper(PlayerStat.attackSpeed, addValue,0.65f);
                break;
            case statusType.attackDamage:
                PlayerStat.attackDamage = minusOper(PlayerStat.attackDamage, addValue);
                break;
            case statusType.criticalChance:
                PlayerStat.criticalChance = minusOper(PlayerStat.criticalChance, addValue);
                break;
            case statusType.criticalDamage:
                PlayerStat.criticalDamage = divisionOper(PlayerStat.criticalDamage, addValue,10);
                break;
        }
    }
    public float divisionOper( float nowValue, float reduceValue, float defaultValue)
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
    public float minusOper(float nowValue, float reduceValue)
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
}
