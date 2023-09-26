using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, NewInputActions.IPlayerActions
{
	const float cameraExpandDelay  = 0.5f;

	private NewInputActions inputs;
	private PlayerMove move;
	private PlayerRoll roll;
	private ShurikenShooter attack;

	float cameraExpandTimer = -1f;

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
		SetAttackDirection();
		if (cameraExpandTimer >= 0)
        {
			cameraExpandTimer += Time.deltaTime;
			if (cameraExpandTimer >= cameraExpandDelay)
			{
				VCamManager.Instance.Expand();
			}
		}
	}

	void SetAttackDirection()
    {
		Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
		attack.SetDirection(dir);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		move.OnMove(context);
	}

	public void OnLook(InputAction.CallbackContext context)
	{
	}

	public void OnLookOnMouse(InputAction.CallbackContext context)
	{
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			attack.StartCharge();
			cameraExpandTimer = 0;
		}
		if (context.canceled)
		{
			SetAttackDirection();
			if (attack.EndCharge())
            {
				cameraExpandTimer = -1;
				VCamManager.Instance.Reduce();
			}
		}
	}

	public void OnRoll(InputAction.CallbackContext context)
	{
		roll.OnRoll(context);
	}
}
