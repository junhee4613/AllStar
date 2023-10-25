using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public void OnNormalTextOff()
    {
        Managers.Pool.UIPush(gameObject);
    }
    public void OnDamageTextOff()
    {
        Managers.Pool.UIPush(transform.parent.gameObject);
    }
}
