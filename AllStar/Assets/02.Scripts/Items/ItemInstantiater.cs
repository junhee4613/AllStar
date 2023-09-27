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
        randomValue = Random.Range(0/*�Ҹ����� Length*/, 10/*Mathf.Max(Mathf.Max(��Ƽ��Ʈ�������� Length), Mathf.Max(����������� Length))*/);
        Managers.DataManager.Init(() =>
        {
            GameObject tempItem = Managers.Pool.Pop(Managers.DataManager.Datas["WeaponItem"] as GameObject);
            tempItem.transform.position = transform.position;
        });
        
    }


}
