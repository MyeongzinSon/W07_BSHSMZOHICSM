using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfInTime : MonoBehaviour
{
    public float destroyTime;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
