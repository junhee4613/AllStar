using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGNwayShot : SGBaseShot
{
    public int wayNum = 5;
    public float centerAngle = 180f;
    public float betweenAngle = 10f;
    public float nextLineDely = 0.1f;
    private int nowIndex;
    private float delayTimer;

    public override void Shot()
    { 
        if(projectileNum <= 0 || projectileSpeed <= 0 || wayNum <= 0)
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
        if(_shooting == false)
        {
            return;
        }
        delayTimer -= SGTimer.Instance.deltaTime;

        while (delayTimer < 0)
        {
            for (int i = 0; i < wayNum; i++)
            {
                SGProjectile projectile = GetProjectile(transform.position);

                if(projectile == null)
                {
                    break;
                }

                float baseAngle = wayNum % 2 == 0 ? centerAngle - (betweenAngle / 2f) : centerAngle;

                float angle = SGUtil.GetShiftedAngle(i, baseAngle, betweenAngle);

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
                FiredShot();
                return;
            }

            delayTimer += nextLineDely;
        }

    }
}
