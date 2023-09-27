using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ItemInstantiater : MonoBehaviour
{
    [SerializeField]private ItemTypeEnum ItemType;
    [SerializeField]private int randomValue;
    // Start is called before the first frame update
    void Start()
    {
        if (!Managers.DataManager.isLoadDone)
        {
            Managers.DataManager.onFunctionDone += ItemSetting;
        }
        else
        {
            ItemSetting();
        }
    }
    void ItemSetting()
    {
        Managers.DataManager.onFunctionDone -= ItemSetting;
        switch (ItemType)
        {
            case ItemTypeEnum.weapon:
                randomValue = Random.Range(0, Managers.DataManager.weaponTable.Count);
                GameObject tempOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["WeaponItem"] as GameObject);
                WeaponData tempDataWeap = Managers.DataManager.weaponTable[randomValue];
                tempOBJ.GetComponent<WeaponItem>().SetItemModel(Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mesh"] as Mesh, (byte)randomValue);
                tempOBJ.transform.position = transform.position;
                break;
            case ItemTypeEnum.artifacts:
                randomValue = Random.Range(0, Managers.DataManager.artifactTable.Count);
                GameObject tempArtifact = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
                ArtifactData tempDataArt = Managers.DataManager.artifactTable[randomValue];
                tempArtifact.GetComponent<ArtifactItem>().SetItemModel(Managers.DataManager.Datas[tempDataArt.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[tempDataArt.codename + "_Item_Mesh"] as Mesh, (byte)randomValue);
                tempArtifact.transform.position = transform.position;
                break;
            case ItemTypeEnum.consumAble:
                randomValue = Random.Range(0, Managers.DataManager.consumableTable.Count);
                GameObject tempConsumOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
                ConsumableData tempConsumeTable = Managers.DataManager.consumableTable[randomValue];
                tempConsumOBJ.GetComponent<ArtifactItem>().SetItemModel(Managers.DataManager.Datas[tempConsumeTable.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[tempConsumeTable.codename + "_Item_Mesh"] as Mesh, (byte)randomValue);
                tempConsumOBJ.transform.position = transform.position;
                break;
        }
    }

}
