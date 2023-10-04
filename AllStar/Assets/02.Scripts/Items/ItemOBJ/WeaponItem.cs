using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : IItemBase
{
    public GunStat weaponInfo = null;
    public override void UseItem<T>(ref T t)
    {
        GunBase gunbase;
        gunbase = t as GunBase;
        if (!weaponInfo.isValueChanged)
        {
            gunbase.SetBasicValue(itemIndex);
        }
        else
        {
            gunbase.stat = weaponInfo;
        }
        Managers.Pool.Push(this.gameObject);
    }
}
