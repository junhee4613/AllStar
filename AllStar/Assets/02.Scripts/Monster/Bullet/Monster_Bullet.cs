using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet : MonoBehaviour
{
    [SerializeField]
    float damage = 1;
    [SerializeField]
    float speed;
    [SerializeField]
    Rigidbody rb;
    [Header("총알이 사라지는 현재시간")]
    public float pushTime;
    [Header("총알이 사라지는 기준시간")]
    public float pushTimer;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pushTime += Time.deltaTime;
        rb.velocity = transform.forward * speed;
        if(Physics.SphereCast(transform.position - transform.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 128))
        {
            Managers.GameManager.PlayerStat.GetDamage(damage);
            Managers.Pool.Push(this.gameObject);
        }
        else if(pushTime >= pushTimer)
        {
            pushTime = 0f;
            Managers.Pool.Push(this.gameObject);
        }
    }
}
