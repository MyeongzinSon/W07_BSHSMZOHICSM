using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Mover
{
    [Header("넉백 관련")]
    public float knockbackTime = 0.25f;
    //public Vector2 lastInput;

    public void OnMove(InputAction.CallbackContext _callback)
    {
        Vector2 input = _callback.ReadValue<Vector2>();
        direction = input;
        if (input != Vector2.zero)
        {
            lastInput = direction;
        }
    }


    public void KnockBack(Vector2 direction, float power)
    {
        if (CanMove
            &&direction!=Vector2.zero)
        {
            body.velocity = Vector2.zero;
            body.AddForce(direction.normalized * power);
            StartCoroutine(SetKnockBackTimer());
        }
    }

    private IEnumerator SetKnockBackTimer()
    {
        CanMove = false;
        yield return new WaitForSeconds(knockbackTime);
        CanMove = true;
    }
}
