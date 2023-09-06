using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class Util
{
    public static void LoadToAsync<T>(string keyValue,Action callback) where T : UnityEngine.Object
    {
        var loadHandler = Addressables.LoadAssetAsync<T>(keyValue);
        loadHandler.Completed += ((DT) =>
        {
            Managers.DataManager.Datas.Add(keyValue, DT.Result);
        });
    }
    public static void LoadLavelToAsync<T>(string labelName) where T : UnityEngine.Object
    {
        var operationHandle = Addressables.LoadResourceLocationsAsync(labelName, typeof(T));
        operationHandle.Completed += ((DT) =>
        {
            foreach (var item in DT.Result)
            {
                Debug.Log(item.PrimaryKey);
                Managers.DataManager.Datas.Add(item.PrimaryKey , item as T);
            }
        });
    }

    public static T Load<T>(string keyValue) where T : UnityEngine.Object
    {
        if (Managers.DataManager.Datas.TryGetValue(keyValue,out UnityEngine.Object resourceResult))
        {
            return resourceResult as T;
        }
        else
        {
            Debug.LogError("·Îµå ¾ÈµÊ");
            
            return null;
        }
    }
}
