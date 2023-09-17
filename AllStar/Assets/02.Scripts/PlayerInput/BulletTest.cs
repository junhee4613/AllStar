using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    public float bulletSpeed = 40;
    public float removeTimer = 0;
    public float bulletDamage;
    private void Update()
    {
        transform.Translate(transform.forward*(bulletSpeed*Time.deltaTime),Space.World);

        if (Physics.SphereCast(transform.position-Vector3.forward,0.5f,transform.forward, out RaycastHit hit,0.7f,64))
        {
            Managers.Pool.Push(this.gameObject);
            if (hit.collider.gameObject.TryGetComponent<MonsterController>(out MonsterController MC))
            {
                MC.getDamage(bulletDamage);
            }
        }
        else if(removeTimer > 10)
        {
            removeTimer = 0;
            Managers.Pool.Push(this.gameObject);
        }
        removeTimer += Time.deltaTime;
    }
    private void Start()
    {
        Debug.Log(transform.forward);
    }
}
