using System.Collections;
using UnityEngine;

public class DestroySeconds : MonoBehaviour
{
    public float destroyTime = 1.0f;

    void Start()
    {
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}