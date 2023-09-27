using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junhee_Test : MonoBehaviour
{
    public HashSet<int> test = new HashSet<int>();
    public HashSet<string> test1 = new HashSet<string>();
    public GameObject test_GameObject = null;
    // Start is called before the first frame update
    void Start()
    {
        test_GameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(test_GameObject.transform.position);
    }
}
