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
            Debug.Log("�ε��� ���������� ����ϴ� �׼��� �־������");
            
        });

        return loadHandler.Result;
    }
}
