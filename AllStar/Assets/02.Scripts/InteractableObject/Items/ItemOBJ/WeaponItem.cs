using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : IItemBase
{
    public GunStat weaponInfo = null;
    protected override void Start()
    {
        GetItemBouce();
        int randomValue = Random.Range(0, Managers.DataManager.weaponTable.Count);
        WeaponData tempDataWeap = Managers.DataManager.weaponTable[randomValue];
        Debug.Log("아이템 머테리얼 : " + tempDataWeap.codename + "_Item_Mat");
        Debug.Log("아이템 메쉬 : " + tempDataWeap.codename + "_Item_Mesh");
        SetItemModel(Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mat"] as Material,
            Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mesh"] as Mesh, (byte)randomValue);

    }
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
