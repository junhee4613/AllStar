using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    float timer = 0;
    float dmg;
    public void OnDamageTextOff()
    {
        Managers.Pool.UIPush(transform.parent.gameObject);
    }
    public void FixedUpdate()
    {
        timer += Time.deltaTime*5;
        if (timer < 1)
        {
            text.text = (dmg / timer).ToString();
        }
        else if (timer > 1&&dmg != float.Parse(text.text))
        {
            text.text = dmg.ToString();
        }
    }
    public void ActiveSetting(float damage)
    {
        dmg = 0;
        timer = 0;
        text.text = 0.ToString();
        dmg = damage;
    }
}
