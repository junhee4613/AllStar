using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers instance { get { Init(); return s_instance; } }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            Debug.Log("게임 메니저 생성");
        }
    }
    [SerializeField]
    GameManager _game = new GameManager();
    [SerializeField]
    DataManager _data = new DataManager();
    PoolingManager _pool = new PoolingManager();

    public static GameManager GameManager { get { return instance?._game; } }
    public static DataManager DataManager { get { return instance?._data; }}
    public static PoolingManager Pool { get { return instance?._pool; } }
}
