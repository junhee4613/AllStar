using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_hp_bar : MonoBehaviour
{
    float time = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= Managers.UI.monster_hp_bar_time)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Managers.UI.monster_hp_bar_time += Time.deltaTime;
        }
    }
}
