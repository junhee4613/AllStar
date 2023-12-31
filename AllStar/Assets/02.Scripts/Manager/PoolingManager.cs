using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

class PoolTRBase
{
    public GameObject _prefabs;
}
class PoolRectTR : PoolTRBase
{
    public RectTransform _root;
    public RectTransform root
    {
        get
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefabs.name}Pool" };
                RectTransform TempTR = go.AddComponent<RectTransform>();
                _root = TempTR;
            }
            return _root;
        }
    }
}
class PoolTR : PoolTRBase
{
    public Transform _root;
    public Transform root
    {
        get
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefabs.name}Pool" };
                _root = go.transform;
            }
            return _root;
        }
    }
}
class Pool
{
    IObjectPool<GameObject> _pool;
    PoolTRBase poolSet;
    public Pool(GameObject prefab,PoolType poolType = PoolType.OBJ)
    {
        switch (poolType)
        {
            case PoolType.UI:
                poolSet = new PoolRectTR();
                poolSet._prefabs = prefab;
                _pool = new ObjectPool<GameObject>(OnUICreate, OnGet, OnRelease, OnDestroy);
                break;
            case PoolType.Mob:
                poolSet = new PoolTR();
                poolSet._prefabs = prefab;
                _pool = new ObjectPool<GameObject>(OnMobCreate, OnGet, OnRelease, OnDestroy);
                break;
            case PoolType.OBJ:
                poolSet = new PoolTR();
                poolSet._prefabs = prefab;
                _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
                break;
        }

    }    
    public void Push(GameObject go)
    {
        if (go.activeSelf)
        {
            _pool.Release(go);
        }
    }
    public GameObject Pop()
    {
        return _pool.Get();
    }
    GameObject OnCreate()
    {
        PoolTR tempSet = poolSet as PoolTR;
        GameObject go = GameObject.Instantiate(poolSet._prefabs);
        go.transform.SetParent(tempSet.root);
        go.name = poolSet._prefabs.name;
        return go;
    }
    GameObject OnUICreate()
    {
        PoolRectTR tempSet = poolSet as PoolRectTR;
        if (Managers.Pool.UIPoolCanvas == null)
        {
            GameObject tempCanvas = new GameObject() { name = "TempUICanvas" };
            RectTransform tempRectCanvas = tempCanvas.AddComponent<RectTransform>();
            Canvas tempCanvasComp = tempCanvas.AddComponent<Canvas>();

            tempSet.root.SetParent(tempRectCanvas);

            tempCanvasComp.renderMode = RenderMode.WorldSpace;
            Managers.Pool.UIPoolCanvas = tempRectCanvas;
        }
        else
        {
            tempSet.root.SetParent(Managers.Pool.UIPoolCanvas);
        }
        GameObject go = GameObject.Instantiate(poolSet._prefabs);
        go.transform.SetParent(tempSet.root);
        go.name = poolSet._prefabs.name;
        return go;
    }
    GameObject OnMobCreate()
    {
        PoolTR tempSet = poolSet as PoolTR;
        GameObject go = GameObject.Instantiate(poolSet._prefabs);
        go.transform.SetParent(tempSet.root);
        return go;
    }
    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }
    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }
    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}
public class PoolingManager
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    public RectTransform UIPoolCanvas;
    public bool IsObjectInPool(string Name)
    {
        if (_pools.ContainsKey(Name))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void MonsterPop(string mobTypeName,GameObject prefab)
    {
        if (!_pools.ContainsKey(mobTypeName))CreateMobPool(prefab,mobTypeName);
        return;
        //return _pools[mobTypeName].Pop();
    }    
    public GameObject UIPop(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab.name))CreateUIPool(prefab);
        return _pools[prefab.name].Pop();
    }

    public GameObject Pop(GameObject prefab,bool IsUI = false)
    {
        //매개변수 prefab을 받아서 같은 게임오브젝트 pool이 없다면! 풀을 만들어준다
        if (_pools.ContainsKey(prefab.name) == false)CreatePool(prefab);
        //매개변수로 받는 게임오브젝트와 같은 이름을 가진 오브젝트를 _pools에서 뽑아준다
        return _pools[prefab.name].Pop();
    }
    public bool Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 없으면 false를 반환
            return false;
        }
        //게임오브젝트를 풀로 반환해준다
        _pools[go.name].Push(go);
        //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 있으면 true를 반환
        return true;
    }
    public bool UIPush(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 없으면 false를 반환
            return false;
        }
        //게임오브젝트를 풀로 반환해준다
        _pools[go.name].Push(go);
        //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 있으면 true를 반환
        return true;
    }
    public bool MobPush(GameObject go,string MobType)
    {
        if (_pools.ContainsKey(MobType) == false)
        {
            //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 없으면 false를 반환
            return false;
        }
        //게임오브젝트를 풀로 반환해준다
        _pools[MobType].Push(go);
        //_pools에 매개변수로 받은 오브젝트와 같은 이름을 가진풀이 있으면 true를 반환
        return true;
    }
    public void Clear()
    {
        //풀을 비워줌
        _pools.Clear();
    }
    void CreatePool(GameObject original)
    {
        //새로운 풀을 등록
        Pool pool = new Pool(original);
        _pools.Add(original.name, pool);
    }
    void CreateMobPool(GameObject original,string mobTypeName)
    {
        //새로운 풀을 등록
        Pool pool = new Pool(original,PoolType.Mob);
        _pools.Add(mobTypeName, pool);
    }
    void CreateUIPool(GameObject original)
    {
        //새로운 풀을 등록
        Pool pool = new Pool(original,PoolType.UI);
        _pools.Add(original.name, pool);
    }
}
public enum PoolType
{
    UI,Mob,OBJ
}