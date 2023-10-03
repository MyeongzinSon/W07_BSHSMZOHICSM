using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class ControllerDivider : MonoBehaviour
{
	private void Start()
	{
		foreach (var i in InputSystem.devices)
		{
			if (i is XInputController)
			{
				Debug.Log($"{i.name}");
			}
		}
	}
}
