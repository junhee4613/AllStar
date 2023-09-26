using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletStat bulletStat;
    private float timer;
    private ParticleSystem targetHitParticle;
    [SerializeField]private float bulletTotalDMG; 
    private void Update()
    {
        transform.Translate(transform.forward * (bulletStat.bulletSpeed * Time.deltaTime), Space.World);

        if (Physics.SphereCast(transform.position - Vector3.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 64))
        {
            if (hit.collider.gameObject.TryGetComponent<MonsterController>(out MonsterController MC))
            {
                MC.getDamage(bulletTotalDMG);
                Debug.Log("��ƼŬ �ý����� ���� �� ����Ǵ� �Լ� ���� �־������");
            }
            Managers.Pool.Push(this.gameObject);
        }
        else if (timer <= 0)
        {
            timer = bulletStat.removeTimer;
            Managers.Pool.Push(this.gameObject);
            Debug.Log("��ƼŬ �ý����� ���� �� ����Ǵ� �Լ� ���� �־������");
        }
        timer -= Time.deltaTime;
    }
    public void BulletSetting(BulletStat tempBullet,float totalDMG)
    {
        bulletStat = tempBullet;
        timer = bulletStat.removeTimer;
        bulletTotalDMG = totalDMG;
    }
}
