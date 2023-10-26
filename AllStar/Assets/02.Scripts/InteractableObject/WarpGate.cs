using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    Collider[] colls;
    // Update is called once per frame
    void Update()
    {
        colls = Physics.OverlapBox(transform.position, new Vector3(1, 2, 1.2f), Quaternion.identity, 128);
        if (colls.Length > 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
