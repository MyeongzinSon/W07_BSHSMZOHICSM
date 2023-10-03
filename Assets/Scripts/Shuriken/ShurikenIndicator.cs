using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenIndicator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private void Awake()
    {
        TryGetComponent(out lineRenderer);
    }

    public void SetPosition(int num, Vector3 position)
    {
        lineRenderer.SetPosition(num, position);
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

    }
}
