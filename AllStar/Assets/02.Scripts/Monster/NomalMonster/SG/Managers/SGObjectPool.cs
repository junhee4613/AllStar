using System;
using System.Collections.Generic;
using UnityEngine;

public class SGObjectPool : MonoBehaviour
{
    static SGObjectPool s_instance;
    public static SGObjectPool Instance { get { return s_instance; } }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@ObjectPool");
            if (go == null)
            {
                go = new GameObject { name = "@ObjectPool" };
                go.AddComponent<SGObjectPool>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<SGObjectPool>();
        }
    }

    [Serializable]
    private class InitializePool
    {
        public GameObject projectilePrefab = null;
        public int initialPoolNum = 0;
    }

    [SerializeField]
    private List<InitializePool> _initializePoolList = null;

    private class PoolingParam
    {
        public List<SGProjectile> projectileList = new List<SGProjectile>(1024);
        public int searchStartIndex = 0;
    }

    private Dictionary<int, PoolingParam> pooledprojectileDic = new Dictionary<int, PoolingParam>(256);

    public void Awake()
    {
        Init();
        if (_initializePoolList != null && _initializePoolList.Count > 0)
        {
            for (int i = 0; i < _initializePoolList.Count; i++)
            {
                CreatePool(_initializePoolList[i].projectilePrefab, _initializePoolList[i].initialPoolNum);
            }
        }       
    }

    public void CreatePool(GameObject goPrefab, int createNum)
    {
        for (int i = 0; i < createNum; i++)
        {
            SGProjectile projectile = Getprojectile(goPrefab, SGUtil.VECTOR3_ZERO, true);
            if (projectile == null)
            {
                break;
            }
            ReleaseProjectile(projectile);
        }
    }

    public SGProjectile Getprojectile(GameObject goPrefab, Vector3 position, bool forceInstantiate = false)
    {
        if (goPrefab == null)
        {
            return null;
        }

        SGProjectile projectile = null;

        int key = goPrefab.GetInstanceID();

        if (pooledprojectileDic.ContainsKey(key) == false)
        {
            pooledprojectileDic.Add(key, new PoolingParam());
        }

        PoolingParam poolParam = pooledprojectileDic[key];

        if (forceInstantiate == false && poolParam.projectileList.Count > 0)
        {
            if (poolParam.searchStartIndex < 0 || poolParam.searchStartIndex >= poolParam.projectileList.Count)
            {
                poolParam.searchStartIndex = poolParam.projectileList.Count - 1;
            }

            for (int i = poolParam.searchStartIndex; i >= 0; i--)
            {
                if (poolParam.projectileList[i] == null || poolParam.projectileList[i].gameObject == null)
                {
                    poolParam.projectileList.RemoveAt(i);
                    continue;
                }
                if (poolParam.projectileList[i].isActive == false)
                {
                    poolParam.searchStartIndex = i - 1;
                    projectile = poolParam.projectileList[i];
                    break;
                }
            }
            if (projectile == null)
            {
                for (int i = poolParam.projectileList.Count - 1; i > poolParam.searchStartIndex; i--)
                {
                    if (poolParam.projectileList[i] == null || poolParam.projectileList[i].gameObject == null)
                    {
                        poolParam.projectileList.RemoveAt(i);
                        continue;
                    }
                    if (i < poolParam.projectileList.Count && poolParam.projectileList[i].isActive == false)
                    {
                        poolParam.searchStartIndex = i - 1;
                        projectile = poolParam.projectileList[i];
                        break;
                    }
                }
            }
        }

        if (projectile == null)
        {
            GameObject go = Instantiate(goPrefab, transform);
            projectile = go.GetComponent<SGProjectile>();
            if (projectile == null)
            {
                projectile = go.AddComponent<SGProjectile>();
            }
            poolParam.projectileList.Add(projectile);
            poolParam.searchStartIndex = poolParam.projectileList.Count - 1;
        }

        projectile.transform.SetPositionAndRotation(position, SGUtil.QUATERNION_IDENTITY);

        projectile.SetActive(true);

        if (projectile == null)
        {
            return null;
        }

        Managers.projectileManager.Addprojectile(projectile);

        return projectile;
    }

    public void ReleaseProjectile(SGProjectile projectile, bool destroy = false)
    {
        if (projectile == null || projectile.gameObject == null)
        {
            return;
        }

        projectile.OnFinishedShot();
        
        Managers.projectileManager.Removeprojectile(projectile, destroy);

        if (destroy)
        {
            Destroy(projectile.gameObject);
            Destroy(projectile);
            projectile = null;
            return;
        }

        if(projectile != null)
        projectile.SetActive(false);
    }

}
