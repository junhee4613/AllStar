using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public void OnDamageTextOff()
    {
        Debug.Log("�̹���" + transform.parent.name);
        Managers.Pool.UIPush(transform.parent.gameObject);

    }
}
