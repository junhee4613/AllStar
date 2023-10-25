using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

class Pool
{
    GameObject _prefabs;
    IObjectPool<GameObject> _pool;

    Transform _root;
    Transform Root
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
    public Pool(GameObject prefab,PoolType poolType = PoolType.OBJ)
    {
        _prefabs = prefab;
        switch (poolType)
        {
            case PoolType.UI:
                _pool = new ObjectPool<GameObject>(OnUICreate, OnGet, OnRelease, OnDestroy);
                break;
            case PoolType.Mob:
                _pool = new ObjectPool<GameObject>(OnMobCreate, OnGet, OnRelease, OnDestroy);
                break;
            case PoolType.OBJ:
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
        GameObject go = GameObject.Instantiate(_prefabs);
        go.transform.SetParent(Root);
        go.name = _prefabs.name;
        return go;
    }
    GameObject OnUICreate()
    {
        if (Root.transform.childCount == 0)
        {
            GameObject tempCanvas = new GameObject {name = "Canvas" };
            tempCanvas.transform.SetParent(Root);
            Canvas tempCanvasComp = tempCanvas.AddComponent<Canvas>();
            tempCanvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
            _root = tempCanvas.transform;
        }
        GameObject go = GameObject.Instantiate(_prefabs);
        go.transform.SetParent(Root);
        go.name = _prefabs.name;
        return go;
    }
    GameObject OnMobCreate()
    {
        GameObject go = GameObject.Instantiate(_prefabs);
        go.transform.SetParent(Root);
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
    public GameObject MonsterPop(string mobTypeName,GameObject prefab)
    {
        if (!_pools.ContainsKey(mobTypeName))CreateMobPool(prefab,mobTypeName);
        return _pools[mobTypeName].Pop();
    }    
    public GameObject UIPop(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab.name))CreateUIPool(prefab);
        return _pools[prefab.name].Pop();
    }

    public GameObject Pop(GameObject prefab,bool IsUI = false)
    {
        //�Ű����� prefab�� �޾Ƽ� ���� ���ӿ�����Ʈ pool�� ���ٸ�! Ǯ�� ������ش�
        if (_pools.ContainsKey(prefab.name) == false)CreatePool(prefab);
        //�Ű������� �޴� ���ӿ�����Ʈ�� ���� �̸��� ���� ������Ʈ�� _pools���� �̾��ش�
        return _pools[prefab.name].Pop();
    }
    public bool Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ false�� ��ȯ
            CreatePool(go);
            return false;
        }
        //���ӿ�����Ʈ�� Ǯ�� ��ȯ���ش�
        _pools[go.name].Push(go);
        //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ true�� ��ȯ
        return true;
    }
    public bool UIPush(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ false�� ��ȯ
            CreateUIPool(go);
            return false;
        }
        //���ӿ�����Ʈ�� Ǯ�� ��ȯ���ش�
        _pools[go.name].Push(go);
        //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ true�� ��ȯ
        return true;
    }
    public bool MobPush(GameObject go,string MobType)
    {
        if (_pools.ContainsKey(MobType) == false)
        {
            CreateMobPool(go,MobType);
            //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ false�� ��ȯ
            return false;
        }
        //���ӿ�����Ʈ�� Ǯ�� ��ȯ���ش�
        _pools[MobType].Push(go);
        //_pools�� �Ű������� ���� ������Ʈ�� ���� �̸��� ����Ǯ�� ������ true�� ��ȯ
        return true;
    }
    public void Clear()
    {
        //Ǯ�� �����
        _pools.Clear();
    }
    void CreatePool(GameObject original)
    {
        //���ο� Ǯ�� ���
        Pool pool = new Pool(original);
        _pools.Add(original.name, pool);
    }
    void CreateMobPool(GameObject original,string mobTypeName)
    {
        //���ο� Ǯ�� ���
        Pool pool = new Pool(original,PoolType.Mob);
        _pools.Add(mobTypeName, pool);
    }
    void CreateUIPool(GameObject original)
    {
        //���ο� Ǯ�� ���
        Pool pool = new Pool(original,PoolType.UI);
        _pools.Add(original.name, pool);
    }
}
public enum PoolType
{
    UI,Mob,OBJ
}