using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.GameManager.AddStatus(statusType.criticalDamage, 40);
            Debug.Log(Managers.GameManager.PlayerStat.criticalChance);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            /*Util.Load<UnityEngine.Object>("AA");*/
            Debug.Log(Managers.DataManager.Datas.Count);
        }
    }

}
