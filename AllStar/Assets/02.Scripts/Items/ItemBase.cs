using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemTypeEnum itemType;
    public void UseItem<T>(ref T targetValue)
    {
        Debug.Log("�̰� �ٽ� ¥�ߵ�");
        /*switch (itemType)
        {
            case ItemTypeEnum.weapon:
                GunBase Temp = targetValue as GunBase;
                break;
            case ItemTypeEnum.artifacts:
                Debug.Log("���� �����ʿ�");
                break;
            case ItemTypeEnum.consumer:
                Debug.Log("���� �����ʿ�");
                break;

        }*/
    }
}