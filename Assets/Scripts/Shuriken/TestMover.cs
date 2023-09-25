using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
	public bool canMove = true;
	public bool isRotate = true;
	
	public float moveSpeed = 5f;
	public Vector2 direction = Vector2.zero;
	public Vector2 lastDireciton = Vector2.down;

	private Rigidbody2D body;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}

	protected void FixedUpdate()
	{
		if (canMove)
		{
			body.velocity = (moveSpeed*direction);
		}
		if (isRotate)
		{
			transform.rotation = Quaternion.Euler(0f,0f,GetAngle());
		}
		if (direction != Vector2.zero)
		{
			direction = direction.normalized;
			lastDireciton = direction;
		}
	}

	float GetAngle()
	{
		Vector2 a = Vector2.right;
		Vector2 b = direction.normalized;
		float angle = Vector2.Angle(a,b);
		float t = ((a.x*b.y - b.x*a.y)>=0)?1f:-1f;
		return angle*t;
	}
}
