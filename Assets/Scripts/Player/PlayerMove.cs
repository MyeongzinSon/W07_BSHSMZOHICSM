using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Mover, NewInputActions.IPlayerActions
{
    [Header("³Ë¹é °ü·Ã")]
    public float knockbackTime = 0.25f;
    //public Vector2 lastInput;
    private NewInputActions inputs;

    private void Awake()
    {
        inputs = new();
        inputs.Player.SetCallbacks(this);
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

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

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnLookOnMouse(InputAction.CallbackContext context)
    {
    }

    public void OnFire(InputAction.CallbackContext context)
    {
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
    }
}
