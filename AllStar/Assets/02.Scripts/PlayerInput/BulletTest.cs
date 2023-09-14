using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    public float BulletSpeed;
    private void Update()
    {
        transform.Translate(transform.forward*(BulletSpeed*Time.deltaTime),Space.World);
    }
    private void Start()
    {
        Debug.Log(transform.forward);
    }
}
