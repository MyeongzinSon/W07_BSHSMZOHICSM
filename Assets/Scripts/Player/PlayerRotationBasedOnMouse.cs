using UnityEngine;
using System.Collections;
 
public class PlayerRotationBasedOnMouse : MonoBehaviour {
     
    private Vector3 mouse_pos;
    public Transform target;
    private Vector3 object_pos;
    private float angle;
 
    void Update()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = -20;
        object_pos = Camera.main.WorldToScreenPoint(target.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}