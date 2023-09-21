using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemTypeEnum itemType;
    public void UseItem<T>(ref T targetValue)
    {
        Debug.Log("이거 다시 짜야됨");
        /*switch (itemType)
        {
            case ItemTypeEnum.weapon:
                GunBase Temp = targetValue as GunBase;
                break;
            case ItemTypeEnum.artifacts:
                Debug.Log("추후 구현필요");
                break;
            case ItemTypeEnum.consumer:
                Debug.Log("추후 구현필요");
                break;

        }*/
    }
}