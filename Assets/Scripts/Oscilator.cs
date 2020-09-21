using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 maxOffset;
    [SerializeField] [Range(-1,1)] float sinWave;
    [SerializeField] float period = 2f;

    const float tau = Mathf.PI * 2;    // its full sin wave ~ 6.28

    Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon)
            return;

        float cycles = Time.time / period;
        sinWave = Mathf.Sin(cycles + tau);  // goes from -1 to 1

        Vector3 offset = sinWave * maxOffset;
        transform.position = startingPos + offset;
    }
}
