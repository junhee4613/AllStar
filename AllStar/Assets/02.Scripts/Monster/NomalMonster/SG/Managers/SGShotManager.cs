using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGShotManager
{
    private List<SGShotCtrl> m_shotList = new List<SGShotCtrl>(2048);       //총알을 2048개까지 관리하는 List
    private HashSet<SGShotCtrl> m_shotHashSet = new HashSet<SGShotCtrl>();  //hashset 값으로 샷 컨트롤 List 관리
    public int activeShotCount { get { return m_shotList.Count; } }         //activeshot 접근 할 경우 갯수 리턴

    public void UpdateShots(float deltaTime)                                //총알 쓴 것 Update 지속적으로 시켜주고 지워야 할 경우 지움
    {
        for (int i = m_shotList.Count - 1; i >= 0; i--)
        {
            SGShotCtrl shotCtrl = m_shotList[i];
            if (shotCtrl == null)
            {
                m_shotList.Remove(shotCtrl);
                continue;
            }
            shotCtrl.UpdateShot(deltaTime);
        }
    }

    public void AddShot(SGShotCtrl shotCtrl)                                //AddShot Hashset에서 더해지는 것들 추가하는 함수
    {
        if (m_shotHashSet.Contains(shotCtrl))
        {
            return;
        }
        m_shotList.Add(shotCtrl);
        m_shotHashSet.Add(shotCtrl);
    }
   
    public void RemoveShot(SGShotCtrl shotCtrl)                             
    {
        if (m_shotHashSet.Contains(shotCtrl) == false)
        {
            return;
        }
        m_shotList.Remove(shotCtrl);
        m_shotHashSet.Remove(shotCtrl);
    }
}
