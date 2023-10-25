using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SGShotCtrl : MonoBehaviour
{
    public bool _shooting;

    public enum UpdateStep
    {
        StartDelay,
        StartShot,
        WaitDelay,
        UpdateIndex,
        FinishShot,
    }

    [Serializable]
    public class ShotInfo
    {       
        public SGBaseShot shotObj;
        public float afterDelay = 0.1f;
    }
    
    public SGUtil.AXIS axisMove = SGUtil.AXIS.X_AND_Y;   
    public bool inheritAngle = false;    
    public bool startOnAwake = true;  
    public float startOnAwakeDelay = 1f;   
    public bool startOnEnable = false;   
    public float startOnEnableDelay = 1f;   
    public bool loop = true;  

    public List<ShotInfo> shotList = new List<ShotInfo>();
           
    public UpdateStep updateStep;
    private int nowIndex;
    private float delayTimer;
  
    private bool isInitialized = false;

    private void Start()
    {
        if (startOnAwake)
        {
            StartShotRoutine(startOnAwakeDelay);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(WaitForSingleton());     
    }

    private IEnumerator WaitForSingleton()
    {
        while (!isInitialized)
        {
            if (Managers.instance != null && Managers.instance.IsInitialized())
            {
                isInitialized = true;
            }
            yield return null;
        }

        Managers.ShotManager.AddShot(this);

        if (startOnEnable)
        {
            StartShotRoutine(startOnEnableDelay);
        }
    }
    private void OnDestroy()
    {
        _shooting = false;


        if (Managers.ShotManager != null)
        {
            Managers.ShotManager.RemoveShot(this);
        }
    }

    public void UpdateShot(float deltaTime)
    {
         if (_shooting == false)
        {
            return;
        }

        if (updateStep == UpdateStep.StartDelay)
        {
            if (delayTimer > 0f)
            {
                delayTimer -= deltaTime;
                return;
            }
            else
            {
                delayTimer = 0f;
                updateStep = UpdateStep.StartShot;
            }
        }

        ShotInfo nowShotInfo = shotList[nowIndex];

        if (updateStep == UpdateStep.StartShot)
        {
            if (nowShotInfo.shotObj != null)
            {
                nowShotInfo.shotObj.SetShotCtrl(this);
                nowShotInfo.shotObj.Shot();
            }

            delayTimer = 0f;
            updateStep = UpdateStep.WaitDelay;
        }

        if (updateStep == UpdateStep.WaitDelay)
        {
            if (nowShotInfo.afterDelay > 0 && nowShotInfo.afterDelay > delayTimer)
            {
                delayTimer += deltaTime;
            }
            else
            {
                nowShotInfo.afterDelay = 0.1f;
                delayTimer = 0f;
                updateStep = UpdateStep.UpdateIndex;
            }
        }

        if (updateStep == UpdateStep.UpdateIndex)
        {            
            if (loop || nowIndex < shotList.Count - 1)
            {
                nowIndex = (int)Mathf.Repeat(nowIndex + 1f, shotList.Count);
                updateStep = UpdateStep.StartShot;
            }
            else
            {
                updateStep = UpdateStep.FinishShot;
            }            
        }

        if (updateStep == UpdateStep.StartShot)
        {
            UpdateShot(deltaTime);
        }
        else if (updateStep == UpdateStep.FinishShot)
        {
            _shooting = false;
        }
    }
    public void StartShotRoutine(float startDelay = 0f)
    {
        if (shotList == null || shotList.Count <= 0)
        {
            //샷 리스트가 비어있음
            return;
        }

        bool enableShot = false;
        for (int i = 0; i < shotList.Count; i++)
        {
            if (shotList[i].shotObj != null)
            {
                enableShot = true;
                break;
            }
        }
        if (enableShot == false)
        {
            //오브젝트 셋팅 안됨
            return;
        }

        if (loop)
        {
            bool enableDelay = false;
            for (int i = 0; i < shotList.Count; i++)
            {
                if (0f < shotList[i].afterDelay)
                {
                    enableDelay = true;
                    break;
                }
            }
            if (enableDelay == false)
            {
                //샷 리스트 제로
                return;
            }
        }

        if (_shooting)
        {
            //이미 쏨
            return;
        }

        _shooting = true;
        delayTimer = startDelay;
        updateStep = delayTimer > 0f ? UpdateStep.StartDelay : UpdateStep.StartShot;
       
        nowIndex = 0;
    }

}
