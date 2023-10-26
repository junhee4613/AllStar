using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class DataManager
{
    public TextAsset JsonFile;
    public Dictionary<string, Object> Datas = new Dictionary<string, Object>();
    public List<WeaponData> weaponTable = new List<WeaponData>();
    public List<ArtifactData> artifactTable = new List<ArtifactData>();
    public List<ConsumableData> consumableTable = new List<ConsumableData>();
    public bool isLoadDone = false;
    public event Action onFunctionDone;
    public void Init(Action Done = null)
    {
        LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log("loading" + key + "||" + count + "/" + totalCount);


            if (Managers.UI.loadBar == null)
            {
                GameObject tempGOBJ = MonoBehaviour.Instantiate(Resources.Load<GameObject>("LoadingCanvas"), null);
                Managers.UI.loadBar = tempGOBJ.transform.GetChild(0).Find("Slider").GetComponent<Slider>();
            }
            Managers.UI.loadBar.maxValue = totalCount;
            Managers.UI.loadBar.value = count;

            if (count == totalCount)
            {
                Managers.UI.loadBar.transform.parent.parent.gameObject.SetActive(false);
                //string jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/ArtifactTable.json");
                TextAsset tempTA = Datas["ArtifactTable"] as TextAsset;
                artifactTable = JsonConvert.DeserializeObject<List<ArtifactData>>(tempTA.text);
                //jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/ConsumableItemTable.json");
                tempTA = Datas["ConsumableItemTable"] as TextAsset;
                consumableTable = JsonConvert.DeserializeObject<List<ConsumableData>>(tempTA.text);
                //jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/GunItemTable.json");
                tempTA = Datas["GunItemTable"] as TextAsset;
                weaponTable = JsonConvert.DeserializeObject<List<WeaponData>>(tempTA.text);
                Done?.Invoke();
                isLoadDone = true;
                onFunctionDone?.Invoke();
            }
        });
        LoadAllAsync<Sprite>("Sprites", (key, count, totalCount) =>
        {
            Debug.Log("loading" + key + "||" + count + "/" + totalCount);
        });
    }
    public T Load<T>(string key) where T : Object
    {
        if (Datas.TryGetValue(key, out Object resource))
        {
            return resource as T;
        }
        if (typeof(T) == typeof(Sprite))
        {
            key = key + ".sprite";
            if (Datas.TryGetValue(key, out Object temp))
            {
                return temp as T;
            }
        }
        return null;
    }
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }
        if (pooling)
        {
            return Managers.Pool.Pop(prefab);
        }
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }
    public void Destroy(GameObject go)
    {
        if (go == null) return;
        if (Managers.Pool.Push(go)) return;

        Object.Destroy(go);
    }
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
    {
        //��������Ʈ�� ��� ������ü�� �̸����� �ε��ϸ� ��������Ʈ�� �ε��� ��
        string loadKey = key;
        if (key.Contains(".sprite"))
        {
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
        }
        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += (op) =>
        {
            //ĳ�� Ȯ��
            if (Datas.TryGetValue(key, out Object resource))
            {
                callback?.Invoke(op.Result);
                return;
            }
            Datas.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
    {
        var OpHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        OpHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;
            foreach (var result in op.Result)
            {
                if (result.PrimaryKey.Contains(".sprite"))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }

}
