using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed;
    private float removeTimer;
    private float timer;
    [SerializeField] private ParticleSystem targetHitParticle;
    [SerializeField]private bulletTypeEnum bulletType;
    [SerializeField]private float bulletTotalDMG;
    [SerializeField] private float totalExplosionDMG;
    [SerializeField] private float totalExplosionRange;

    private void OnEnable()
    {
        Bullet tempThisCompo = this;
        Managers.GameManager.SetBullet(this.gameObject.name,ref tempThisCompo);
    }
    private void Update()
    {
        if (Physics.SphereCast(transform.position - Vector3.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 64))
        {
            if (hit.collider.gameObject.TryGetComponent<MonsterController>(out MonsterController MC))
            {
                if (!targetHitParticle.gameObject.activeSelf)
                {
                    MC.getDamage(bulletTotalDMG);
                    targetHitParticle.gameObject.SetActive(true);
                    GetComponent<MeshRenderer>().enabled = false;
                    timer = removeTimer;
                }
                if ((targetHitParticle.time / targetHitParticle.main.duration) < 1)
                {
                }
                else
                {
                    targetHitParticle.gameObject.SetActive(false);
                    GetComponent<MeshRenderer>().enabled = true;
                    Managers.Pool.Push(this.gameObject);
                }
                Debug.Log("파티클 시스템이 끝난 뒤 실행되는 함수 만들어서 넣어줘야함");
            }
        }
        else
        {
            transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);
        }
        if (timer <= 0)
        {
            timer = removeTimer;
            targetHitParticle.gameObject.SetActive(false);
            Managers.Pool.Push(this.gameObject);
            Debug.Log("파티클 시스템이 끝난 뒤 실행되는 함수 만들어서 넣어줘야함");
        }
        timer -= Time.deltaTime;
    }
    public void BulletSetting(in BulletStat tempBullet,in float totalCollDMG,in Vector2 totalExDMG = default)
    {
        bulletType = tempBullet.bulletType;
        bulletSpeed = tempBullet.bulletSpeed;
        removeTimer = tempBullet.removeTimer;
        timer = removeTimer;
        bulletTotalDMG = totalCollDMG;
        if (bulletType == bulletTypeEnum.explosion)
        {
            totalExplosionDMG = totalExDMG.x;
            totalExplosionRange = totalExDMG.y;
        }
    }
}
