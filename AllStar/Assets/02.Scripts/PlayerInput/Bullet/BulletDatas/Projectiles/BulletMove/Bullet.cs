using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed;
    private float removeTimer;
    private float timer;
    [SerializeField] private Transform meshtr;
    [SerializeField] private ParticleSystem targetHitParticle;
    [SerializeField]private bulletTypeEnum bulletType;
    [SerializeField]private float bulletTotalDMG;
    [SerializeField]private bool isExplode = false;
    [SerializeField] private float totalExplosionDMG;
    [SerializeField] private float totalExplosionRange;
    public bool isCritical = false;
    private void OnEnable()
    {
        Bullet tempThisCompo = this;
        Managers.GameManager.SetBullet(this.gameObject.name,ref tempThisCompo);
        meshtr.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Physics.SphereCast(transform.position , 0.1f, transform.forward, out RaycastHit hit, 0.5f, 64))
        {
            if (!targetHitParticle.gameObject.activeSelf)
            {
                Managers.GameManager.monstersInScene[hit.collider.gameObject.name].GetDamage(hit.transform.position, bulletTotalDMG);
                targetHitParticle.gameObject.SetActive(true);
                meshtr.gameObject.SetActive(false);
                timer = removeTimer;
            }
            if (bulletType == bulletTypeEnum.explosion && (targetHitParticle.time / targetHitParticle.main.duration) > 0.5f&&isExplode == false)
            {
                Collider[] hitMonsters = Physics.OverlapSphere(transform.position, totalExplosionRange, 64);
                Debug.Log("Æø¹ß");
                isExplode = true;
                for (int i = 0; i < hitMonsters.Length; i++)
                {
                    
                    Managers.GameManager.monstersInScene[hitMonsters[i].gameObject.name].GetDamage(hitMonsters[i].transform.position, totalExplosionDMG);
                }
            }
            if ((targetHitParticle.time / targetHitParticle.main.duration) > 1)
            {
                targetHitParticle.gameObject.SetActive(false);
                if (bulletType == bulletTypeEnum.explosion)
                {
                    isExplode = false;
                }
                Managers.Pool.Push(this.gameObject);
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
            isExplode = false;
            Managers.Pool.Push(this.gameObject);
        }
        timer -= Time.deltaTime;
    }
    public void BulletSetting(in GunStat tempBullet,in float totalCollDMG,in Vector2 totalExDMG = default)
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
