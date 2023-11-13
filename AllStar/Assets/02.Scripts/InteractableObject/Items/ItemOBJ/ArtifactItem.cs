using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItem : IItemBase
{
    protected override void Start()
    {
        int randomValue = Random.Range(0, Managers.DataManager.artifactTable.Count);
        GetItemBouce();
        ArtifactData tempDataArt = Managers.DataManager.artifactTable[randomValue];
        if (!Managers.DataManager.Datas.TryGetValue(tempDataArt.codename + "_Item_Mat", out Object aa))
        {
            Debug.Log("¿À·ù : " + tempDataArt.codename + "_Item_Mat");
        }
        SetItemModel(Managers.DataManager.Datas[tempDataArt.codename + "_Item_Mat"] as Material,
            Managers.DataManager.Datas["Artifact" + "_Item_Mesh"] as Mesh, (byte)randomValue);
    }
    public override void UseItem<T>(ref T t)
    {
        Managers.Pool.Push(this.gameObject);
    }
}
