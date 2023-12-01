using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossTrackingCam : MonoBehaviour
{
    [Range(0, 20)]
    public float cameraDistance;
    float real_camera_dis;
    public Transform target;
    public Transform boss;
    // Update is called once per frame
    void Update()
    {
        real_camera_dis = BossPlayerDis(boss, target);
        transform.position = target.position + new Vector3(5, real_camera_dis, -real_camera_dis);
        
    }
    public float BossPlayerDis(Transform boss, Transform player)
    {
        float trs = Mathf.Abs(boss.position.z - player.position.z) + cameraDistance; 
        return trs;
    }
}
