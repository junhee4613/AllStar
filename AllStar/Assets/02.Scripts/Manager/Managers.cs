using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers instance { get { Init(); return s_instance; } }

    private static bool isInitialized = false;
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
            isInitialized = true; // 초기화 완료
            Debug.Log("게임 메니저 생성");
        }
    }
    public bool IsInitialized()
    {
        return isInitialized;
    }
    [SerializeField]
    GameManager _game = new GameManager();
    [SerializeField]
    DataManager _data = new DataManager();
    PoolingManager _pool = new PoolingManager();
    UIManager _ui = new UIManager();

    SGProjectileManager _projectileManager = new SGProjectileManager();         //Manager 하위에 projectiManager
    SGShotManager _shotManager = new SGShotManager();
    [SerializeField]
    SoundManager _soundManager = new SoundManager();

    public static GameManager GameManager { get { return instance?._game; } }
    public static DataManager DataManager { get { return instance?._data; }}
    public static PoolingManager Pool { get { return instance?._pool; } }
    public static UIManager UI { get { return instance?._ui; } }
    //Manager 하위에 shotManager
                              //Manager 하위에 shotManager

    public static SGShotManager ShotManager { get { return instance?._shotManager; } }      //ShotManager 접근 시 인스턴스 리턴
    public static SGProjectileManager projectileManager { get { return instance?._projectileManager; } }
    public static SoundManager Sound { get { return instance?._soundManager; } }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Managers.Pool.Clear();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            Managers.Sound.bgmList.Add(Managers.DataManager.Datas["Stage_00" + SceneManager.GetActiveScene().buildIndex + 1] as AudioClip);
        }
    }
}
