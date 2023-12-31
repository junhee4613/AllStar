using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using PlayerSkills.SkillProbs;

[System.Serializable]
public class DataManager
{
    public TextAsset JsonFile;
    public Dictionary<string, Object> Datas = new Dictionary<string, Object>();
    public List<WeaponData> weaponTable = new List<WeaponData>();
    public List<ArtifactData> artifactTable = new List<ArtifactData>();
    public Dictionary<string, ArtifactLevelTable> artifactLevelTable = new Dictionary<string, ArtifactLevelTable>();
    public List<ConsumableData> consumableTable = new List<ConsumableData>();
    public Dictionary<string, SkillLevelTable> skillLevelTable = new Dictionary<string, SkillLevelTable>();
    [SerializeField]
    public List<SkillDataJson> skillTable = new List<SkillDataJson>();
    public bool isLoadDone = false;
    public event Action onFunctionDone;
    public void Init(Action Done = null)
    {


        if (Managers.UI.loadBar == null)
        {
            GameObject tempGOBJ = MonoBehaviour.Instantiate(Resources.Load<GameObject>("LoadingCanvas"), null);
            GameObject.DontDestroyOnLoad(tempGOBJ);
            Managers.UI.loadBar = tempGOBJ.transform.GetChild(0).Find("Slider").GetComponent<Slider>();

        }
        LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log("loading" + key + "||" + count + "/" + totalCount);
            Managers.UI.loadBar.maxValue = totalCount;
            Managers.UI.loadBar.value = count;
            if (count == totalCount)
            {
                //string jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/ArtifactTable.json");
                TextAsset tempTA = Datas["ArtifactTable"] as TextAsset;
                artifactTable = JsonConvert.DeserializeObject<List<ArtifactData>>(tempTA.text);
                //jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/ConsumableItemTable.json");
                tempTA = Datas["ConsumableItemTable"] as TextAsset;
                consumableTable = JsonConvert.DeserializeObject<List<ConsumableData>>(tempTA.text);
                //jsonConvert = File.ReadAllText("Assets/02.Scripts/Items/Jsons/JsonFile/GunItemTable.json");
                tempTA = Datas["GunItemTable"] as TextAsset;
                weaponTable = JsonConvert.DeserializeObject<List<WeaponData>>(tempTA.text);
                tempTA = Datas["SkillTable"] as TextAsset;
                skillTable = JsonConvert.DeserializeObject<List<SkillDataJson>>(tempTA.text);
                tempTA = Datas["ArtifactLevelTable"] as TextAsset;
                foreach (var item in JsonConvert.DeserializeObject<List<ArtifactLevelTable>>(tempTA.text))
                {
                    artifactLevelTable.Add(item.SkillName, item);
                }
                tempTA = Datas["SkillLevels"] as TextAsset;
                foreach (var item in JsonConvert.DeserializeObject<List<SkillLevelTable>>(tempTA.text))
                {
                    skillLevelTable.Add(item.level, item);
                }
                Managers.GameManager.BasicPlayerStats(() =>
                {
                    Done?.Invoke();
                    isLoadDone = true;
                    Managers.Sound.BGM_Sound("Stage_001");
                    onFunctionDone?.Invoke();
                    Managers.UI.loadBar.transform.parent.parent.gameObject.SetActive(false);
                });

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
