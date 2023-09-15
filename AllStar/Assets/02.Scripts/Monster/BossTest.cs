using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Test()
    {
        while (Time.time < 10)
        {
            gameObject.transform.Rotate(360 * Time.deltaTime, 360 * Time.deltaTime, 360 * Time.deltaTime);
            yield return null;
        }
    }
}
