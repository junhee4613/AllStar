using System;
using UnityEngine;

public class Util
{
    public static T GetOrAddCompo<T>(GameObject GO) where T : Component
    {
        T compo = default;
        if (GO.TryGetComponent<T>(out T result))
        {
            GO.GetComponent<T>();
            compo = result;
        }
        else
        {
            if (GO.GetComponentInChildren<T>() != null)
            {
                compo = GO.GetComponentInChildren<T>();
            }
            else
            {
                compo = GO.AddComponent<T>();
            }
        }
        return compo;

    }
}
