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
            Debug.Log("»ý¼º");
        }
        Debug.Log("¤±?¤©");
    }
    GameManager _game = new GameManager();
    DataManager _data = new DataManager();

    public static GameManager GameManager { get { return instance?._game; } }
    public static DataManager DataManager { get { return instance?._data; }}
}
