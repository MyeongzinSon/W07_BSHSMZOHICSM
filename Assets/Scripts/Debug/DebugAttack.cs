using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugAttack : MonoBehaviour, NewInputActions.IPlayerActions
{
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            VCamManager.Instance.Expand();
        }
        else if(context.canceled)
        {
            VCamManager.Instance.Reduce();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnLookOnMouse(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
    }
}
