using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float totalDamage;
    public float removeTimer;
    public ParticleSystem targetHitParticle;

    public virtual void Update()
    {
        transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);

        if (Physics.SphereCast(transform.position - Vector3.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 64))
        {
            if (hit.collider.gameObject.TryGetComponent<MonsterController>(out MonsterController MC))
            {
                MC.getDamage(totalDamage);
            }
            Managers.Pool.Push(this.gameObject);
        }
        else if (removeTimer <= 0)
        {
            removeTimer = 0;
            Managers.Pool.Push(this.gameObject);

        }
        removeTimer -= Time.deltaTime;
    }
}
