using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util<UnityEngine.GameObject>;

public class FuncTester : MonoBehaviour
{
    public GameObject B;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.GameManager.AddStatus(statusType.criticalDamage, 40);
            Debug.Log(Managers.GameManager.PlayerStat.criticalChance);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            B = LoadToAsync("AA", () => 
            {
                Debug.Log(B.name);
            });

        }
    }
}
