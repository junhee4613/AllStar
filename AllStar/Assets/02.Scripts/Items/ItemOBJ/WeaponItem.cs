using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : IItemBase
{
    public GunBase gunbase;
    public override void UseItem<T>(ref T t)
    {
        gunbase = t as GunBase;
        gunbase.SetBasicValue(itemIndex);
        Managers.Pool.Push(this.gameObject);
    }
}
