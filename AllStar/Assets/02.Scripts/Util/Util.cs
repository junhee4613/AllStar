using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class Util<T>
{
    public static T LoadToAsync(string KeyValue,Action callback)
    {
        var loadHandler = Addressables.LoadAssetAsync<T>(KeyValue);
        loadHandler.Completed += ((DT) =>
        {
            Debug.Log("로딩이 끝날때까지 대기하는 액션을 넣어줘야함");
            
        });

        return loadHandler.Result;
    }
}
