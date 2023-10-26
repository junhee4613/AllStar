using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public void OnDamageTextOff()
    {
        Managers.Pool.UIPush(transform.parent.gameObject);
    }
}
