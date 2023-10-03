using System.Collections;
using UnityEngine;
using Cinemachine;

public class EdgeScrollPad : MonoBehaviour
{
    private PlayerController controller;

    private GameObject followObj;
    private float scrollSpeed = 20.0f;
    private float edgeSizePercent = 20.0f;
    private float maxMovePosition = 5f;
    private Vector3 stickPosition;

    private void Awake()
    {
        followObj = transform.Find("@VCamTarget").gameObject;
    }

    private void OnEnable()
    {
        TryGetComponent(out controller);
    }

    public void GetStickInput(Vector2 input)
    {
        stickPosition = input;
    }

    private void Update()
    {

        // Normalize camera movement vector to maintain consistent speed
        if (stickPosition != Vector3.zero)
        {
            stickPosition.Normalize();

            Vector3 newPosition = followObj.transform.position + stickPosition * scrollSpeed * Time.deltaTime;
            newPosition = Vector3.ClampMagnitude(newPosition - followObj.transform.parent.position, maxMovePosition) + followObj.transform.parent.position;
            followObj.transform.position = newPosition;
        }
        else if(Vector3.Distance(followObj.transform.position, followObj.transform.parent.position) > 0)
        {
           followObj.transform.position -= (followObj.transform.position - followObj.transform.parent.position).normalized * scrollSpeed * Time.deltaTime;
        }
    }
}

