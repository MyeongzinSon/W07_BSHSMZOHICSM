using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, NewInputActions.IPlayerActions
{
	private NewInputActions inputs;
	private PlayerMove move;
	private PlayerRoll roll;
	private ShurikenShooter attack;

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
		}
		if (context.canceled)
		{
			SetAttackDirection();
			attack.EndCharge();
		}
	}

	public void OnRoll(InputAction.CallbackContext context)
	{
		roll.OnRoll(context);
	}
}
