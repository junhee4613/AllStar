using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using physicsPlus;

public class OverlapTest : MonoBehaviour
{
    public Collider[] colliders;
    public EnhancedPhysics EP = new EnhancedPhysics();
    // Update is called once per frame
    void Update()
    {
        if (EP.IsChangedInArray(colliders, transform.position, 10,7))
        {
            colliders = Physics.OverlapSphere(transform.position, 8);
            if (EP.OverlapSearchTheOBJ(ref colliders, out GameObject aa, "heoyoon"))
            {

            }
        }
    }
}
