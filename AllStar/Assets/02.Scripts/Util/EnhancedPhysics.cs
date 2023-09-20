using UnityEngine;
using System.Linq;

namespace physicsPlus
{
    public class EnhancedPhysics
    {
        public bool OverlapSearchTheOBJ(ref Collider[] coll, out GameObject targetOBJ, string name = null)
        {
            //������ ����� ����ҋ� �Ű����� name���� ���� �̸��� ������Ʈ�� out���� ��������
            targetOBJ = null;
            foreach (var item in coll)
            {
                if (item.gameObject.name == name)
                {
                    targetOBJ = item.gameObject;
                    return true;
                }
            }

            return false;
        }
        public bool IsChangedInArray(Collider[] array, Vector3 originPos, float range,int targetLayer)
        {
            //���̾�� �ν����ͻ��� ���̾� ���ڷ� �־��ָ��
            targetLayer = 1 << targetLayer;
            //overlap �迭 ���� �ִ� �ݶ��̴��� �ֽŰ��� �������� ���Ͽ� �޶����� ������ True�� ��ȯ,�װ� �ƴϸ� false�� ��ȯ
            if (Enumerable.SequenceEqual(array.OrderBy(a => a.name), Physics.OverlapSphere(originPos, range, targetLayer).OrderBy(a => a.name)))
            {
                Debug.Log("������ �޽� ��ȯ");
                return false;
            }
            else
            {
                Debug.Log("������ Ʈ�� ��ȯ");
                return true;
            }
        }
    }
}
