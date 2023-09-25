using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineRunner = Unity.VisualScripting.CoroutineRunner;

public class ShurikenShooter : MonoBehaviour
{
	public Mover shurikenPrefab;
	public LayerMask damageLayer;

	[Header("플레이어에게서 약간 떨어진 거리에서 발사됩니다.")]
	public float shootRadius = 0.5f;

	#region privateArea

	private Camera mainCamera;
	private CharacterStats stats;
	private int maxCartridge;
	private int currnetCartridge;
	private int shurikenCount;

	#endregion
	
	private void Start()
	{
		mainCamera = Camera.main;
		if (!TryGetComponent(out stats))
        {
			Debug.LogError($"ShurikenShooter : 해당 캐릭터에서 CharacterStats 컴포넌트를 찾을 수 없음! (Instance ID : {this.GetInstanceID()})");
        }
		maxCartridge = stats.maxCartridgeNum;
		currnetCartridge = maxCartridge;
		shurikenCount = 0;
	}

	private void Update()
	{
		//테스트용, 나중에 Input System으로 변경 필요
		if (Input.GetMouseButtonDown(0))
		{
			TryShoot();
		}
	}
	bool TryShoot()
    {
		if (currnetCartridge > 0)
		{
			Shoot(mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
			return true;
		}
		return false;
	}
	void Shoot(Vector2 _dir)
	{
		//정규화
		_dir = _dir.normalized;

		currnetCartridge--;
		shurikenCount++;
		//총알 실제 생성, 초기화
		Mover inst = Instantiate(shurikenPrefab, (Vector2)transform.position + _dir*shootRadius, Quaternion.identity);
		inst.direction = _dir;
		inst.SetRotationByDirection();
		
		//슈리켄 값 받아와서 해당 값에 대한 설정
		Shuriken instSrk = inst.GetComponent<Shuriken>();
		instSrk.damageLayer = damageLayer;
		instSrk.damage = stats.attackPower;
		instSrk.moveDistance = stats.maxDistance;
		inst.speed = stats.shurikenSpeed;
		foreach (var a in stats.shurikenAttributes)
        {
			if (shurikenCount % a.GetActivateNumber() == 0)
            {
				instSrk.attributes.Add(a);
            }
        }
	}
}
