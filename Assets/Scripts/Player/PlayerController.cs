using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, NewInputActions.IPlayerActions
{
	const float cameraExpandDelay  = 0.5f;

	[SerializeField] bool isMouseInput;

	private NewInputActions inputs;
	private PlayerMove move;
	private PlayerRoll roll;
	private ShurikenShooter attack;

	bool isLooking = false;
	float cameraExpandTimer = -1f;

	bool IsMouseInput => isMouseInput;

	void Awake()
	{
		inputs = new();
		inputs.Player.SetCallbacks(this);
		inputs.Enable();
		
		TryGetComponent(out move);
		TryGetComponent(out roll);
		TryGetComponent(out attack);
	}
	private void OnDisable()
	{
		inputs.Disable();
	}
	void Update()
    {
		SetAttackDirectionWithMouse();
		if (cameraExpandTimer >= 0)
        {
			cameraExpandTimer += Time.deltaTime;
			if (cameraExpandTimer >= cameraExpandDelay)
			{
				// set PlayerNum
				VCamManager.Instance.Expand(0);
			}
		}
	}

	void SetAttackDirectionWithMouse()
    {
		if (IsMouseInput)
		{
			Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
			attack.SetDirection(dir);
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		Debug.Log($"클릭한 디바이스: {context.control.device.name}");
		
		if (GameManager.Instance.isBattleStart)
		{
			move.OnMove(context);
			if (!isLooking)
            {
				attack.SetDirection(context.ReadValue<Vector2>());
            }
		}
    }

	public void OnLook(InputAction.CallbackContext context)
	{
		var direction = context.ReadValue<Vector2>();
		if (!IsMouseInput && direction != Vector2.zero)
		{
			attack.SetDirection(direction);
		}
		if (context.started)
		{
			isLooking = true;
			Debug.Log($"isLooking = true");
		}
		else if (context.canceled)
		{
			isLooking = false;
			Debug.Log($"isLooking = false");
		}
	}

    public void OnLookOnMouse(InputAction.CallbackContext context)
	{
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		if (GameManager.Instance.isBattleStart)
		{
			if (context.started)
			{
				if (attack.StartCharge())
					cameraExpandTimer = 0;
				if (IsMouseInput)
                {
					isLooking = true;
                }
			}
			if (context.canceled)
			{
				SetAttackDirectionWithMouse();
				if (attack.EndCharge())
				{
					cameraExpandTimer = -1;
					VCamManager.Instance.Reduce();
				}
				if (IsMouseInput)
                {
					isLooking = false;
                }
			}
		}
	}

	public void OnRoll(InputAction.CallbackContext context)
	{
		if (GameManager.Instance.isBattleStart)
		{
			roll.OnRoll(context);
			attack.Cancel();
			cameraExpandTimer = -1;
			VCamManager.Instance.Reduce();
		}
	}
}
