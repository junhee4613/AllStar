using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_attack : MonoBehaviour
{
    float damage;
    public ParticleSystem particle;
    // Update is called once per frame
    void Update()
    {
        
        if (!particle.isPlaying)
        {
            Managers.Pool.Push(this.gameObject);
        }
        else if (particle.time >= 1 && particle.time <= 2)
        {
            if (Physics.BoxCast(gameObject.transform.position, new Vector3(1, 2, 0), transform.forward, Quaternion.identity, 16, 1 << 7))
            {
                Managers.GameManager.PlayerStat.GetDamage(damage);
            }
        }
    }
}
