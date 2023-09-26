using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenShooter : MonoBehaviour
{
	const float MaxCharge = 1;

	public Mover shurikenPrefab;
	public LayerMask damageLayer;

	[Header("플레이어에게서 약간 떨어진 거리에서 발사됩니다.")]
	public float shootRadius = 0.5f;
	[Header("여러발 발사할 때의 최대 발사각")]
	public float launchAngle = 45f;

	#region privateArea

	private Vector2 direction;
	private CharacterStats stats;
	private int maxCartridge;
	private int currentCartridge;
	private int shurikenCount;
	private float currentCharge = 0;

	private bool CanShoot => currentCartridge > 0;
	bool IsCharging => currentCharge > 0;

	private LineRenderer lineRenderer;

	#endregion

	public bool CanShoot => currentCartridge > 0;
	public bool IsCharging => currentCharge > 0;
	public bool IsCartridgeFull => currentCartridge == maxCartridge;
	public float CurrentChargeAmount => currentCharge / MaxCharge;
	public float CurrentDistance => stats.maxDistance * CurrentChargeAmount;

	private void Start()
	{
		if (!TryGetComponent(out stats))
        {
			Debug.LogError($"ShurikenShooter : 해당 캐릭터에서 CharacterStats 컴포넌트를 찾을 수 없음! (Instance ID : {this.GetInstanceID()})");
        }
		maxCartridge = stats.maxCartridgeNum;
		currentCartridge = maxCartridge;
		shurikenCount = 0;

		TryGetComponent(out lineRenderer);
	}

	private void Update()
	{
		lineRenderer?.SetPosition(0, transform.position);

		if (IsCharging)
		{
			Vector2 dir = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
			Debug.Log(dir);
			lineRenderer?.SetPosition(1, transform.position + (Vector3)(dir * (stats.maxDistance * (currentCharge / MaxCharge))));

			currentCharge += Time.deltaTime * stats.chargeSpeed;
			currentCharge = Mathf.Min(currentCharge, MaxCharge);
		}
		else
		{
			lineRenderer?.SetPosition(1, transform.position);
		}
	}
	public bool StartCharge()
	{
		if (CanShoot && !IsCharging)
        {
			currentCharge += Time.deltaTime;
			return true;
        }
		return false;
    }
	public bool EndCharge()
    {
		if (IsCharging)
		{
			if (TryShoot())
            {
				currentCharge = 0;
				return true;
            }
		}
		return false;
    }
	public void SetDirection(Vector2 _direction)
    {
		direction = _direction;
    }
	bool TryShoot()
    {
		if (currentCartridge > 0)
		{
			float angle = -launchAngle;
			if(stats.shurikenNum==1)
				Shoot(direction, false);
			else
			{
				for (int i = 0; i < stats.shurikenNum; i++)
				{
					Vector3 td = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
					if((int)stats.shurikenNum/2==i)
						Shoot(td,false);
					else
						Shoot(td,true);
					angle += launchAngle/(stats.shurikenNum/2);
				}
			}
			return true;
		}
		return false;
	}
	void Shoot(Vector2 _dir, bool isShadow)
	{
		//정규화
		_dir = _dir.normalized;

		if(!isShadow)
			currentCartridge--;
		shurikenCount++;
		//총알 실제 생성, 초기화
		Mover inst = Instantiate(shurikenPrefab, (Vector2)transform.position + _dir*shootRadius, Quaternion.identity);
		inst.direction = _dir;
		inst.SetRotationByDirection();

		Debug.Log(inst.speed);
		//슈리켄 값 받아와서 해당 값에 대한 설정
		Shuriken instSrk = inst.GetComponent<Shuriken>();
		instSrk.damageLayer = damageLayer;
		instSrk.owner = gameObject;
		instSrk.damage = stats.attackPower;
		instSrk.moveDistance = CurrentDistance;
		instSrk.isShadow = isShadow;
		
		//특대형 수리검
		instSrk.transform.localScale *= 1f + stats.shurikenScale;

		inst.speed = stats.shurikenSpeed * CurrentChargeAmount;
		foreach (var a in stats.shurikenAttributes)
        {
			if (shurikenCount % a.GetActivateNumber() == 0)
            {
				instSrk.AddAttribute(a);
            }
        }
	}

	public void AddCurrentCartridge(int _amount)
	{
		currentCartridge += _amount;
	}
}
