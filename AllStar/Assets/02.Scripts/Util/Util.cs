using System;
using UnityEngine;

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
