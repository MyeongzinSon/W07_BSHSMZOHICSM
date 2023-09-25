using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineRunner = Unity.VisualScripting.CoroutineRunner;

public class ShurikenShooter : MonoBehaviour
{
	public TestMover shurikenPrefab;

	[Header("플레이어에게서 약간 떨어진 거리에서 발사됩니다.")]
	public float shootRadius = 0.5f;
	
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Shoot(Vector2.right);
		}
	}

	void Shoot(Vector2 _dir)
	{
		_dir = _dir.normalized;
		TestMover inst = Instantiate(shurikenPrefab, (Vector2)transform.position + _dir*shootRadius, Quaternion.identity);
		inst.direction = _dir;
		inst.SetRotationByDirection();
	}
}
