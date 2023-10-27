using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGSprialMultiShot : SGBaseShot
{
    public int spiralWayNum = 4;
    public float startAngle = 180f;
    public float shiftAngle = 5f;
    public float betweenDelay = 0.2f;
    private int nowIndex;
    private float delayTimer;

    public override void Shot()
    {

        if (projectileNum <= 0 || projectileSpeed <= 0f || spiralWayNum <= 0)
        {
            return;
        }
        if (_shooting)
        {
            return;
        }
        _shooting = true;
        nowIndex = 0;
        delayTimer = 0;

    }
    protected virtual void Update()
    {
        if (_shooting == false)
        {
            return;
        }
        delayTimer -= SGTimer.Instance.deltaTime;

        while (delayTimer <= 0)
        {
            float spiralWayShiftAngle = 360f / spiralWayNum;
            for (int i = 0; i < spiralWayNum; i++)
            {
                SGProjectile projectile = GetProjectile(transform.position);
                if(projectile == null)
                {
                    break;
                }
                float angle = startAngle + (spiralWayShiftAngle * i) + (shiftAngle * Mathf.Floor(nowIndex / spiralWayNum));
                ShotProjectile(projectile, projectileSpeed, angle);
                projectile.UpdateMove(-delayTimer);
                nowIndex++;
                if(nowIndex >= projectileNum)
                {
                    break;
                }
            }
            FiredShot();
            if(nowIndex >= projectileNum)
            {
                FinishedShot();
                return;
            }
            delayTimer += betweenDelay;
        }

    }
}
