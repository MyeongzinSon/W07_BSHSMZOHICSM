using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineRunner = Unity.VisualScripting.CoroutineRunner;

public class ShurikenShooter : MonoBehaviour
{
	public TestMover shurikenPrefab;
	public LayerMask damageLayer;

	[Header("플레이어에게서 약간 떨어진 거리에서 발사됩니다.")]
	public float shootRadius = 0.5f;

	#region privateArea

	private Camera camera;
	

	#endregion
	
	private void Start()
	{
		camera = Camera.main;
	}

	private void Update()
	{
		//테스트용, 나중에 Input System으로 변경 필요
		if (Input.GetMouseButtonDown(0))
		{
			Shoot(camera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		}
	}

	void Shoot(Vector2 _dir)
	{
		_dir = _dir.normalized;
		TestMover inst = Instantiate(shurikenPrefab, (Vector2)transform.position + _dir*shootRadius, Quaternion.identity);
		inst.direction = _dir;
		inst.SetRotationByDirection();
		Shuriken instSrk = inst.GetComponent<Shuriken>();
		instSrk.damageLayer = damageLayer;
	}
}
