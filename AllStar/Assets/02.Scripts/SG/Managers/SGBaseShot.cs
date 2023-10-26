using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine;
using GeneralFSM;

public abstract class SGBaseShot : MonoBehaviour                //�Ѿ��� �⺻���� ����
{
    public GameObject projectilePrefab;                         //�Ѿ� ������ ����
    
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

    public bool _shooting;
    public bool continuous_attack = false;
    public bool attack = false;

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
        if (continuous_attack && attack)
        {
            _shooting = false;
        }
        //����� ���� �ֱ� �����ϸ� ��
    }
    public virtual void FinishedShot()              //���� ������ �� �̺�Ʈ�� �ְų� �Լ� ȣ��
    {
    }      
    protected SGProjectile GetProjectile(Vector3 position, bool forceInstantiate = false)
    {
        if (projectilePrefab == null)
        {
            Debug.Log("�����");
            //��Ǯ�� ���� ����
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
