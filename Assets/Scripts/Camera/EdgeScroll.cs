using System.Collections;
using UnityEngine;
using Cinemachine;

public class EdgeScroll : MonoBehaviour
{
    private bool isPlayer1 => controller.playerIndex == 0;

    private PlayerController controller;

    private GameObject followObj;
    private float scrollSpeed = 20.0f;
    private float edgeSizePercent = 20.0f;
    private float maxMovePosition = 5f;
    private Vector2 mousePosition;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        followObj = transform.Find("@VCamTarget").gameObject;
    }

    private void OnEnable()
    {
        TryGetComponent(out controller);
    }

    public void GetMouseInput(Vector2 input)
    {
        mousePosition = input;
    }

    private void Update()
    {
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0f);

        bool isCursorAtLeftEdge = mousePosition.x < screenSize.x / 2 * (edgeSizePercent / 100f) + (isPlayer1 ? 0f : screenSize.x / 2);
        bool isCursorAtRightEdge = mousePosition.x > (screenSize.x / 2) * (isPlayer1 ?  (1 - (edgeSizePercent / 100f)) : (edgeSizePercent / 100f));
        bool isCursorAtTopEdge = mousePosition.y < screenSize.y * (edgeSizePercent / 100f);
        bool isCursorAtBottomEdge = mousePosition.y > screenSize.y - screenSize.y * (edgeSizePercent / 100f);

        Vector3 cameraMovement = Vector3.zero;

        if (isCursorAtLeftEdge)
        {
            cameraMovement += Vector3.left;
        }
        else if (isCursorAtRightEdge)
        {
            cameraMovement += Vector3.right;
        }

        if (isCursorAtTopEdge)
        {
            cameraMovement += Vector3.down;
        }
        else if (isCursorAtBottomEdge)
        {
            cameraMovement += Vector3.up;
        }

        // Normalize camera movement vector to maintain consistent speed
        if (cameraMovement != Vector3.zero)
        {
            cameraMovement.Normalize();

            Vector3 newPosition = followObj.transform.position + cameraMovement * scrollSpeed * Time.deltaTime;
            newPosition = Vector3.ClampMagnitude(newPosition - followObj.transform.parent.position, maxMovePosition) + followObj.transform.parent.position;
            followObj.transform.position = newPosition;
        }
        else if(Vector3.Distance(followObj.transform.position, followObj.transform.parent.position) > 0)
        {
           followObj.transform.position -= (followObj.transform.position - followObj.transform.parent.position).normalized * scrollSpeed * Time.deltaTime;
        }
    }
}

