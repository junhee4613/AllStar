using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : IItemBase
{

    public override void UseItem<T>(ref T t)
    {
        GunBase gunbase;
        gunbase = t as GunBase;
        gunbase.SetBasicValue(itemIndex);
        Managers.Pool.Push(this.gameObject);
    }
}
