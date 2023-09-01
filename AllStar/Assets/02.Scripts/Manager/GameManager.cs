using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public Status PlayerStat = new Status();
    public void BasicPlayerStats()
    {

    }
    public void AddStatus(statusType type, float addValue)
    {
        switch (type)
        {
            case statusType.moveSpeed:
                PlayerStat.moveSpeed = sumOper(PlayerStat.moveSpeed, addValue);
                break;
            case statusType.HP:
                PlayerStat.HP = sumOper(PlayerStat.HP, addValue);
                break;
            case statusType.attackSpeed:
                PlayerStat.attackSpeed = multipleOper(PlayerStat.attackSpeed, addValue);
                break;
            case statusType.attackDamage:
                PlayerStat.attackDamage = sumOper(PlayerStat.attackDamage, addValue);
                break;
            case statusType.criticalChance:
                PlayerStat.criticalChance = sumOper(PlayerStat.criticalChance, addValue);
                break;
            case statusType.criticalDamage:
                PlayerStat.criticalDamage = multipleOper(PlayerStat.criticalDamage, addValue);
                break;
        }
    }
    public float sumOper(float OriginValue,float addValue)
    {
        //합연산 데미지
        return OriginValue + addValue;
    }
    public float multipleOper(float OriginValue, float addValue)
    {
        //곱연산 데미지
        return OriginValue * ((addValue/100f)+1);
    }
}
