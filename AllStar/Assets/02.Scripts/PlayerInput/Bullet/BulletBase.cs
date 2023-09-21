using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public float bulletSpeed;
    public float removeTimer;
    public float bulletDamage;
    public Mesh bulletModel;
    public Material bulletMat;
    [Header("투사체 타입")]
    public BulletType bulletType;
    public AttackTypeStatus bulletAttackStatus;
    [Header("발사 타입")]
    public ShotType shotType;
    public ShotStatus shotStatus;
    //세팅 
    public virtual void SetBasicValue()
    {
        Debug.Log(this.GetType());
        //확인 결과 클래스 이름을 가져올 수 있으니 데이터테이블에 총알 스텟을 딕셔너리화 하여 총알 클래스 이름을 기획서와 맞춰 데이터를 불러는식으로 진행
    }
    private void Update()
    {
        transform.Translate(transform.forward * (bulletSpeed * Time.deltaTime), Space.World);

        if (Physics.SphereCast(transform.position - Vector3.forward, 0.5f, transform.forward, out RaycastHit hit, 0.7f, 64))
        {
            Managers.Pool.Push(this.gameObject);
            if (hit.collider.gameObject.TryGetComponent<MonsterController>(out MonsterController MC))
            {
                MC.getDamage(bulletDamage);
            }
        }
        else if (removeTimer > 10)
        {
            removeTimer = 0;
            Managers.Pool.Push(this.gameObject);
            
        }
        removeTimer += Time.deltaTime;
    }
}

public enum ShotType
{
    multiShot,singleShot
}
