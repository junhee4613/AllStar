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
    [SerializeField]private LayerMask hitLayer = 64;
    public bool isCritical = false;
    public bool isExplosionCritical = false;
    private void OnEnable()
    {
        Bullet tempThisCompo = this;
        Managers.GameManager.SetBullet(this.gameObject.name,ref tempThisCompo);
        meshtr.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Physics.SphereCast(transform.position , 0.08f, transform.forward, out RaycastHit hit, 0.5f, hitLayer))
        {
            Vector3 randomValue = Random.Range(-0.5f, 0.6f) * (Vector3.right + Vector3.forward);
            
            RectTransform txtTR;
            if (hit.transform.gameObject.layer == 6)
            {
                if (!targetHitParticle.gameObject.activeSelf)
                {
                    if (isCritical)
                    {
                        txtTR = Managers.Pool.UIPop(Managers.DataManager.Load<GameObject>("CriticalDMGText")).transform as RectTransform;
                    }
                    else
                    {
                        txtTR = Managers.Pool.UIPop(Managers.DataManager.Load<GameObject>("DMGText")).transform as RectTransform;
                    }
                    txtTR.position = randomValue+hit.transform.position+Vector3.up;
                    txtTR.GetChild(0).GetComponent<DamageText>().ActiveSetting(bulletTotalDMG);
                    Managers.GameManager.monstersInScene[hit.collider.gameObject.name].GetDamage(bulletTotalDMG);
                    targetHitParticle.gameObject.SetActive(true);
                    meshtr.gameObject.SetActive(false);
                    timer = removeTimer;

                } 
            }
            else
            {
                targetHitParticle.gameObject.SetActive(true);
                meshtr.gameObject.SetActive(false);
                timer = removeTimer;
            }
            if (bulletType == bulletTypeEnum.explosion && (targetHitParticle.time / targetHitParticle.main.duration) > 0.5f&&isExplode == false)
            {
                Collider[] hitMonsters = Physics.OverlapSphere(transform.position, totalExplosionRange, 64);
                Managers.Sound.SFX_Sound(Managers.DataManager.Datas["Explosion_Sound"] as AudioClip);
                Debug.Log("Æø¹ß");
                isExplode = true;
                for (int i = 0; i < hitMonsters.Length; i++)
                {
                    randomValue = hitMonsters[i].transform.position + (Random.Range(-1f, 2f) * (Vector3.right + Vector3.forward));
                    if (isExplosionCritical)
                    {
                        txtTR = Managers.Pool.UIPop(Managers.DataManager.Load<GameObject>("CriticalDMGText")).transform as RectTransform;
                    }
                    else
                    {
                        txtTR = Managers.Pool.UIPop(Managers.DataManager.Load<GameObject>("DMGText")).transform as RectTransform;
                    }
                    txtTR.position = randomValue;
                    txtTR.GetChild(0).GetComponent<DamageText>().ActiveSetting(totalExplosionDMG);
                    Managers.GameManager.monstersInScene[hitMonsters[i].gameObject.name].GetDamage(totalExplosionDMG);
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
