using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleLight : MonoBehaviour
{
    public Light buildingLight;
    public float minFlickerDuration = 1.0f; // 불빛이 꺼져 있는 시간
    public float maxFlickerDuration = 5.0f; // 불빛이 켜져 있는 시간
    byte randomValue;
    void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            buildingLight.enabled = true; // 불빛을 항상 켭니다.
            yield return new WaitForSeconds(Random.Range(minFlickerDuration, maxFlickerDuration)); // 꺼져 있는 시간을 기다립니다.
            randomValue = (byte)Random.Range(1, 10);
            for (int i = 0; i < randomValue; i++)
            {
                buildingLight.enabled = false; // 불빛을 꺼놓습니다.
                yield return new WaitForSeconds(minFlickerDuration); // 매우 짧은 시간 동안 불빛을 끈 상태를 유지합니다.
                buildingLight.enabled= true;
            }

        }
    }
}

