using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    Rigidbody rb;
    [Header("총알이 사라지는 시간")]
    public float pushTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.forward * speed;
        if(Physics.SphereCast(transform.position - Vector3.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 128))
        {
            Managers.GameManager.PlayerStat.GetDamage(damage);
            Managers.Pool.Push(this.gameObject);
        }
        else if(pushTime <= 0)
        {
            pushTime = 3f;
            Managers.Pool.Push(this.gameObject);
        }
    }
}
