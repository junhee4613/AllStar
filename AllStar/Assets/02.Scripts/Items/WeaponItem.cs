using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour,IItemBase
{
    public string weaponName;
    public GunBase gunbase;
    public void UseItem<T>(ref T t)
    {
        gunbase = t as GunBase;
        gunbase.SetBasicValue(weaponName);
    }
}
