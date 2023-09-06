using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    public TextAsset JsonFile;
    public Dictionary<string, UnityEngine.Object> Datas = new Dictionary<string, UnityEngine.Object>();
    public void Init()
    {
        Util.LoadLavelToAsync<UnityEngine.Object>("PreLoad");
        LoadJsons();
    }
    public void LoadJsons()
    {
/*        var result = JsonConvert.DeserializeObject<BulletStatus[]>(JsonFile.text);//convert to usable data
        foreach (var i in result)
        {
            *//*Debug.Log($"Id : {i.Id}, Value : {i.Value}, Bool : {i.Boolean}");//Print data;*//*
        }*/
    }
}
