using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGWavingShot : SGBaseShot
{
    public int wayNum = 4;
    public float waveCenterAngle = 180.0f;
    public float waveRangeSize = 40.0f;
    public float waveSpeed = 5.0f;
    public float betweenAngle = 5.0f;
    public float nextLineDelay = 0.1f;

    private int nowIndex;
    private float delayTimer;

    public override void Shot()                     //Shot 필수 구현 함수 (SGBaseShot)
    {

        if (projectileNum <= 0 || projectileSpeed <= 0f || wayNum <= 0)     //옵션값검사
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
        while (delayTimer <= 0)             //총알 딜레이가 다 될경우
        {
            for (int i = 0; i < wayNum; i++)
            {
                SGProjectile projectile = GetProjectile(transform.position);
                if (projectile == null)
                {
                    break;
                }

                float centerAngle = waveCenterAngle + (waveRangeSize / 2.0f * Mathf.Sin(SGTimer.Instance.totalFrameCount * waveSpeed / 100f));
                float baseAngle = wayNum % 2 == 0 ? centerAngle - (betweenAngle / 2f) : centerAngle;
                float angle = GetShiftedAngle(i, baseAngle, betweenAngle);
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
            delayTimer += nextLineDelay;
        }
    }
    public float GetShiftedAngle(int wayIndex, float baseAngle, float betweenAngle)
    {
        float angle = wayIndex % 2 == 0 ?
            baseAngle - (betweenAngle * (float)wayIndex / 2f) :
            baseAngle + (betweenAngle * Mathf.Ceil((float)wayIndex / 2f));

        return angle;
    }
}
