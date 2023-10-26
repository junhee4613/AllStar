using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleLight : MonoBehaviour
{
    public Light buildingLight;
    public float minFlickerDuration = 1.0f; // �Һ��� ���� �ִ� �ð�
    public float maxFlickerDuration = 5.0f; // �Һ��� ���� �ִ� �ð�
    byte randomValue;
    void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            buildingLight.enabled = true; // �Һ��� �׻� �մϴ�.
            yield return new WaitForSeconds(Random.Range(minFlickerDuration, maxFlickerDuration)); // ���� �ִ� �ð��� ��ٸ��ϴ�.
            randomValue = (byte)Random.Range(1, 10);
            for (int i = 0; i < randomValue; i++)
            {
                buildingLight.enabled = false; // �Һ��� �������ϴ�.
                yield return new WaitForSeconds(minFlickerDuration); // �ſ� ª�� �ð� ���� �Һ��� �� ���¸� �����մϴ�.
                buildingLight.enabled= true;
            }

        }
    }
}

