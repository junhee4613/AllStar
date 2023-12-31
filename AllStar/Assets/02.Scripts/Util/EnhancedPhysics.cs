using UnityEngine;
using System.Linq;

namespace physicsPlus
{
    public class EnhancedPhysics<T>
    {
        public bool SearchTheComponent(in Collider[] coll,out T targetComponent,string objName)
        {
            targetComponent = default;
            for (int i = 0; i < coll.Length; i++)
            {
                if (coll[i].gameObject.name.Contains(objName))
                {
                    targetComponent = coll[i].gameObject.GetComponent<T>();
                    return true;
                }
            }
            return false;
        }
        public bool OverlapSearchTheOBJ(in Collider[] coll, out GameObject targetOBJ, string name = null)
        {
            //오버랩 기능을 사용할떄 매개변수 name값과 같은 이름의 오브젝트를 out으로 내보내줌
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
            //레이어는 인스펙터상의 레이어 숫자로 넣어주면됨
            targetLayer = 1 << targetLayer;
            //overlap 배열 내에 있는 콜라이더를 최신값과 이전값을 비교하여 달라진게 있으면 True를 반환,그게 아니면 false를 반환
            if (Enumerable.SequenceEqual(array.OrderBy(a => a.name), Physics.OverlapSphere(originPos, range, targetLayer).OrderBy(a => a.name)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
