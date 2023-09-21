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
    [Header("����ü Ÿ��")]
    public BulletType bulletType;
    public AttackTypeStatus bulletAttackStatus;
    [Header("�߻� Ÿ��")]
    public ShotType shotType;
    public ShotStatus shotStatus;
    //���� 
    public virtual void SetBasicValue()
    {
        Debug.Log(this.GetType());
        //Ȯ�� ��� Ŭ���� �̸��� ������ �� ������ ���������̺� �Ѿ� ������ ��ųʸ�ȭ �Ͽ� �Ѿ� Ŭ���� �̸��� ��ȹ���� ���� �����͸� �ҷ��½����� ����
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
