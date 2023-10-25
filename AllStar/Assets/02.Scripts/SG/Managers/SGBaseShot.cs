using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine;

public abstract class SGBaseShot : MonoBehaviour                //총알의 기본적인 구성
{
    public GameObject projectilePrefab;                         //총알 프리팹 설정
    
    public int projectileNum = 10;    
    public float projectileSpeed = 2f;   
    public float accelerationSpeed = 0f;   
    public bool useMaxSpeed = false;   
    public float maxSpeed = 0f;    
    public bool useMinSpeed = false;  
    public float minSpeed = 0f;   
    public float accelerationTurn = 0f;   
    public bool usePauseAndResume = false;    
    public float pauseTime = 0f;   
    public float resumeTime = 0f;   
    public bool useAutoRelease = false;    
    public float autoReleaseTime = 10f;

    protected bool _shooting;

    private SGShotCtrl _shotCtrl;

    public SGShotCtrl shotCtrl
    {
        get
        {
            if (_shotCtrl == null)
            {
                _shotCtrl = GetComponentInParent<SGShotCtrl>();
            }
            return _shotCtrl;
        }
    }   
    public bool shooting { get { return _shooting; } }
    public virtual bool lockOnShot { get { return false; } }
    protected virtual void OnDestroy()
    {
        _shooting = false;
    }
    public abstract void Shot();
    public void SetShotCtrl(SGShotCtrl shotCtrl)
    {
        _shotCtrl = shotCtrl;
    }  
    protected virtual void FiredShot()
    {
       
    }
    public virtual void FinishedShot()              //샷이 끝났을 때 이벤트를 넣거나 함수 호출
    {
        _shooting = false;
       
    }      
    protected SGProjectile GetProjectile(Vector3 position, bool forceInstantiate = false)
    {
        if (projectilePrefab == null)
        {
            //블릿풀에 문제 있음
            return null;
        }

        return SGObjectPool.Instance.Getprojectile(projectilePrefab, position, forceInstantiate);
    }

    protected void ShotProjectile(SGProjectile projectile, float speed, float angle, bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f,
        bool sinWave = false, float sinWaveSpeed = 0f, float sinWaveRangeSize = 0f, bool sinWaveInverse = false)
    {
        if (projectile == null)
        {
            return;
        }

        projectile.Shot(this, speed, angle, accelerationSpeed, accelerationTurn, homing, homingTarget, homingAngleSpeed, 
            sinWave, sinWaveSpeed, sinWaveRangeSize, sinWaveInverse, usePauseAndResume, pauseTime, resumeTime, useAutoRelease, autoReleaseTime,
            _shotCtrl.axisMove, _shotCtrl.inheritAngle, useMaxSpeed, maxSpeed, useMinSpeed, minSpeed);
       
    }
}
