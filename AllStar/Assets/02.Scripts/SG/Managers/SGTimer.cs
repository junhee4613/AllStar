using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGTimer : MonoBehaviour
{
    static SGTimer s_instance;
    public static SGTimer Instance { get { Init(); return s_instance; } }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@SGTimer");
            if (go == null)
            {
                go = new GameObject { name = "@SGTimer" };
                go.AddComponent<SGTimer>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<SGTimer>();
        }
    }
    private const float FIXED_DELTA_TIME_BASE = (1f / 60f);

    [SerializeField]
    private SGUtil.TIME m_deltaTimeType = SGUtil.TIME.DELTA_TIME;

    private float _deltaTime;
    private float _deltaTimeUnscaled;
    private float _deltaTimeFixed;

    private float _deltaFrameCount;
    private float _deltaFrameCountUnscaled;
    private float _deltaFrameCountFixed;

    private float _totalFrameCount;
    private float _totalFrameCountUnscaled;
    private float _totalFrameCountFixed;
    private bool _pausing;
    public SGUtil.TIME deltaTimeType { get { return m_deltaTimeType; } set { m_deltaTimeType = value; } }

    public bool pausing { get { return _pausing; } }

    public float deltaTime
    {
        get
        {
            if (_pausing)
            {
                return 0f;
            }

            switch (m_deltaTimeType)
            {
                case SGUtil.TIME.UNSCALED_DELTA_TIME:
                    return _deltaTimeUnscaled;

                case SGUtil.TIME.FIXED_DELTA_TIME:
                    return _deltaTimeFixed;

                case SGUtil.TIME.DELTA_TIME:
                default:
                    return _deltaTime;
            }
        }
    }

    public float deltaFrameCount
    {
        get
        {
            if (_pausing)
            {
                return 0f;
            }

            switch (m_deltaTimeType)
            {
                case SGUtil.TIME.UNSCALED_DELTA_TIME:
                    return _deltaFrameCountUnscaled;

                case SGUtil.TIME.FIXED_DELTA_TIME:
                    return _deltaFrameCountFixed;

                case SGUtil.TIME.DELTA_TIME:
                default:
                    return _deltaFrameCount;
            }
        }
    }
    public float totalFrameCount
    {
        get
        {
            switch (m_deltaTimeType)
            {
                case SGUtil.TIME.UNSCALED_DELTA_TIME:
                    return _totalFrameCountUnscaled;

                case SGUtil.TIME.FIXED_DELTA_TIME:
                    return _totalFrameCountFixed;

                case SGUtil.TIME.DELTA_TIME:
                default:
                    return _totalFrameCount;
            }
        }
    }

    public void Awake()
    {
        UpdateTimes();
    }

    private void Update()
    {
        UpdateTimes();
        Managers.projectileManager.Updateprojectiles(deltaTime);
        Managers.ShotManager.UpdateShots(deltaTime);
    }

    private void UpdateTimes()
    {
        _deltaTime = Time.deltaTime;
        _deltaTimeUnscaled = Time.unscaledDeltaTime;

        float nowFps = 0;
        int vSyncCount = QualitySettings.vSyncCount;
        if (vSyncCount == 1)
        {

            nowFps = Screen.currentResolution.refreshRate;
        }
        else if (vSyncCount == 2)
        {
            nowFps = Screen.currentResolution.refreshRate / 2f;
        }
        else
        {
            nowFps = Application.targetFrameRate;
        }

        if (nowFps > 0)
        {
            _deltaTimeFixed = FIXED_DELTA_TIME_BASE * (60 / nowFps);
        }
        else
        {
            _deltaTimeFixed = 0;
        }

        _deltaFrameCount = _deltaTime / FIXED_DELTA_TIME_BASE;
        _deltaFrameCountUnscaled = _deltaTimeUnscaled / FIXED_DELTA_TIME_BASE;
        _deltaFrameCountFixed = _deltaTimeFixed / FIXED_DELTA_TIME_BASE;

        if (_pausing == false)
        {
            _totalFrameCount += _deltaFrameCount;
            _totalFrameCountUnscaled += _deltaFrameCountUnscaled;
            _totalFrameCountFixed += _deltaFrameCountFixed;
        }
    }

    public void Pause()
    {
        _pausing = true;
    }
    
    public void Resume()
    {
        _pausing = false;
    }
}
