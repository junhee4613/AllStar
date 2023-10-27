using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGShotManager
{
    private List<SGShotCtrl> m_shotList = new List<SGShotCtrl>(2048);       //�Ѿ��� 2048������ �����ϴ� List
    private HashSet<SGShotCtrl> m_shotHashSet = new HashSet<SGShotCtrl>();  //hashset ������ �� ��Ʈ�� List ����
    public int activeShotCount { get { return m_shotList.Count; } }         //activeshot ���� �� ��� ���� ����

    public void UpdateShots(float deltaTime)                                //�Ѿ� �� �� Update ���������� �����ְ� ������ �� ��� ����
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

    public void AddShot(SGShotCtrl shotCtrl)                                //AddShot Hashset���� �������� �͵� �߰��ϴ� �Լ�
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
