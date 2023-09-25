using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ItemInstantiater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.DataManager.Init(() =>
        {
            Managers.Pool.Pop(Managers.DataManager.Datas["WeaponItem"] as GameObject);
        });
    }

}
