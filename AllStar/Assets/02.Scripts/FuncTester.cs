using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class FuncTester : MonoBehaviour
{
    public Bullets[] BulletTest = new Bullets[2];
    public void Start()
    {
        BulletTest[0] = new Santan();
        BulletTest[1] = new MachineGun();
    }
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            BulletTest[0].BasicValue();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Bullets a = null;
            a = BulletTest[1].init();
            Debug.Log(Managers.DataManager.Datas["AA"] + "오브젝트는");
            Debug.Log(Managers.DataManager.Datas["BulletTable"]);
        }
    }

}
