using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

public class Util
{
    public static T GetOrAddCompo<T>(GameObject GO) where T : Component
    {

        T component = GO.GetComponent<T>();
        if (component == null)
        {
            if (GO.GetComponentInChildren<T>() != null)
            {
                component = GO.GetComponentInChildren<T>();
            }
            else
            {
                GO.AddComponent<T>();
            }
        }
        return component;

    }
}
