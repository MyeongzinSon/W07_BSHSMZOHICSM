using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public float maxHp = 32;
	public float hp = 32;

	private void Awake()
	{
		hp = maxHp;
	}

	public void Hit(float damage)
	{
		Debug.Log("Damaged: "+name);
		hp -= damage;
		if (hp <= 0)
		{
			hp = 0f;
			Kill();
		}
	}

	public void Kill()
	{
		Debug.Log("Killed: "+name);
		gameObject.SetActive(false);
	}
}
