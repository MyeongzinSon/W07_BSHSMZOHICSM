using System.Collections;
using UnityEngine;
using Cinemachine;

public class EdgeScroll : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private GameObject followObj;
    private float scrollSpeed = 20.0f;
    private float edgeSizePercent = 20.0f;
    private float maxMovePosition = 5f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        TryGetComponent(out virtualCamera);
        followObj = GameObject.Find("@VCamTarget");
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0f);

        bool isCursorAtLeftEdge = mousePosition.x < screenSize.x * (edgeSizePercent / 100);
        bool isCursorAtRightEdge = mousePosition.x > screenSize.x - (screenSize.x * (edgeSizePercent / 100));
        bool isCursorAtTopEdge = mousePosition.y < screenSize.y * (edgeSizePercent / 100);
        bool isCursorAtBottomEdge = mousePosition.y > screenSize.y - screenSize.y * (edgeSizePercent / 100);

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
            cameraMovement += Vector3.down * (screenSize.x / screenSize.y);
        }
        else if (isCursorAtBottomEdge)
        {
            cameraMovement += Vector3.up * (screenSize.x / screenSize.y);
        }

        // Normalize camera movement vector to maintain consistent speed
        if (cameraMovement != Vector3.zero)
        {
            cameraMovement.Normalize();

            if (virtualCamera.Follow != null)
            {
                Vector3 newPosition = followObj.transform.position + cameraMovement * scrollSpeed * Time.deltaTime;
                newPosition = Vector3.ClampMagnitude(newPosition - followObj.transform.parent.position, maxMovePosition) + followObj.transform.parent.position;
                followObj.transform.position = newPosition;
            }
        }
        else if(Vector3.Distance(followObj.transform.position, followObj.transform.parent.position) > 0)
        {
           followObj.transform.position -= (followObj.transform.position - followObj.transform.parent.position).normalized * scrollSpeed * Time.deltaTime;
        }
    }
}

