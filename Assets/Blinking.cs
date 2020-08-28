using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Blinking : MonoBehaviour
{
    public bool blink = true;
    public float delay = 1;
    bool isBlinking;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }
    void Update()
    {
        if (blink)
        {
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
        }
        else
        {
            StopCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        light.enabled = false;
        yield return new WaitForSeconds(delay);
        light.enabled = true;
        yield return new WaitForSeconds(delay);
    }
}
