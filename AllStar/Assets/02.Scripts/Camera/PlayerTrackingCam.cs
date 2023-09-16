using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTrackingCam : MonoBehaviour
{
    [Range(0,20)]
    public float cameraDistance;
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + new Vector3(0, cameraDistance, -cameraDistance);
    }
}
