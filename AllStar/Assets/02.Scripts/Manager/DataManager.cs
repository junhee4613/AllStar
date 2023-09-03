using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager
{
    public Dictionary<string, UnityEngine.Object> Datas = new Dictionary<string, UnityEngine.Object>();
    public void Init()
    {
        Util.LoadLavelToAsync<UnityEngine.Object>("PreLoad");
    }
}
