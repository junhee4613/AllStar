using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public void OnDamageTextOff()
    {
        Debug.Log("¿ÃπÃ¡ˆ" + transform.parent.name);
        Managers.Pool.Push(transform.parent.gameObject);
    }
}
