using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;                //추가 using 사용

public class SGSprialShot : SGBaseShot
{
    public float startAngle = 180f;      //시작 각도
    public float shiftAngle = 10f;      //움직이는 각도
    public float betweenDealy = 0.2f;   
    int nowIndex;
    float delayTimer;

    public override void Shot()                 //shot필수 구현 함수(SGBaseShot)
    {
        if(projectileNum <= 0 || projectileSpeed <= 0)      //옵션값검사
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
            SGProjectile projectile = GetProjectile(transform.position);    //총알의 포지션을 받아온다.
            if(projectile == null)      //총알이 없을 경우 종료
            {
                FinishedShot();
                return;
            }

            float angle = startAngle + (shiftAngle * nowIndex);
                ShotProjectile(projectile, projectileSpeed, angle);
            projectile.UpdateMove(-delayTimer);

            nowIndex++;

            FiredShot();

            if(nowIndex >= projectileNum)
            {
                FinishedShot();
                return;
            }
            delayTimer += betweenDealy;
        }
    }
}
