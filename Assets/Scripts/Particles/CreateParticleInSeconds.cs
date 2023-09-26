using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticleInSeconds : MonoBehaviour
{
    public float destroyTime;
    public GameObject particle;
    private void Update()
    {
        if (destroyTime > 0 && destroyTime != 999f) destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
        {
            destroyTime = 999f;
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
