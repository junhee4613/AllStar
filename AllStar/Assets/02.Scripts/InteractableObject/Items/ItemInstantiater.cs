using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ItemInstantiater : MonoBehaviour
{
    [SerializeField]private ItemTypeEnum ItemType;
    // Start is called before the first frame update
    void Start()
    {
        
        if (!Managers.DataManager.isLoadDone)
        {
            Managers.DataManager.onFunctionDone += this.ItemSetting;
        }
        else
        {
            this.ItemSetting();
        }
    }
    void ItemSetting()
    {
        ItemTypeEnum tempType = this.ItemType;
        int randomValue;
        switch (tempType)
        {
            case ItemTypeEnum.weapon:
                randomValue = Random.Range(0, Managers.DataManager.weaponTable.Count);
                GameObject tempOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["WeaponItem"] as GameObject);
                WeaponData tempDataWeap = Managers.DataManager.weaponTable[randomValue];
                Debug.Log("아이템 머테리얼 : "+tempDataWeap.codename + "_Item_Mat");
                Debug.Log("아이템 메쉬 : "+tempDataWeap.codename + "_Item_Mesh");
                tempOBJ.GetComponent<WeaponItem>().SetItemModel(Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas[tempDataWeap.codename + "_Item_Mesh"] as Mesh, (byte)randomValue);
                tempOBJ.transform.position = transform.position;
                break;
            case ItemTypeEnum.artifacts:
                Debug.Log("아티팩트 배열" + Managers.DataManager.artifactTable.Count);
                randomValue = Random.Range(0, Managers.DataManager.artifactTable.Count);
                GameObject tempArtifact = Managers.Pool.Pop(Managers.DataManager.Datas["ArtifactItem"] as GameObject);
                ArtifactData tempDataArt = Managers.DataManager.artifactTable[randomValue];
                if (!Managers.DataManager.Datas.TryGetValue(tempDataArt.codename + "_Item_Mat",out Object aa))
                {
                    Debug.Log("오류 : "+tempDataArt.codename + "_Item_Mat");
                }
                tempArtifact.GetComponent<ArtifactItem>().SetItemModel(Managers.DataManager.Datas[tempDataArt.codename + "_Item_Mat"] as Material,
                    Managers.DataManager.Datas["Artifact" + "_Item_Mesh"] as Mesh, (byte)randomValue);
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
            case ItemTypeEnum.skill:
                randomValue = Random.Range(0, Managers.DataManager.skillTable.Count);
                GameObject tempSkillOBJ = Managers.Pool.Pop(Managers.DataManager.Datas["SkillItem"] as GameObject);
                tempSkillOBJ.transform.position = transform.position;
                break;
        }
        Managers.DataManager.onFunctionDone -= this.ItemSetting;
    }

}
