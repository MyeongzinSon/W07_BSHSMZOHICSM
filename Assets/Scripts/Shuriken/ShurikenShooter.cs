using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenShooter : MonoBehaviour
{
	public Mover shurikenPrefab;
	public LayerMask damageLayer;

	[Header("플레이어에게서 약간 떨어진 거리에서 발사됩니다.")]
	public float shootRadius = 0.5f;
	[Header("여러발 발사할 때의 최대 발사각")]
	public float launchAngle = 15f;

	[Header("조준 중 n% 느려집니다.")]
	public float slowOnCharge = 0.3f;

	[Header("차지 데미지")] 
	public float chargeDamageMultiplier = .25f;
	public int chargeLevel = 1;
	[Range(0, 100)] public float ghostShurikenDamageScale = 25;
	private float shurikenDamageScale = 100f;

	#region privateArea

	private Mover mover;
	private Vector2 direction;
	private CharacterStats stats;
	private float maxCharge = 3;
	private int maxCartridge;
	private int currentCartridge;
	private int shurikenCount;
	private float currentCharge = 0;
	private float rawSlowNum = 0;

	private ShurikenIndicator shurikenIndicator;
	
	[SerializeField] private GameObject chargeParticle;
	[SerializeField] private CatridgeUIManager catridgeUIManager;

	#endregion

	public bool CanShoot => currentCartridge > 0;
	public bool IsCharging => currentCharge > 0;
	public bool IsCartridgeFull => currentCartridge == maxCartridge;
	public float CurrentChargeAmount => currentCharge / maxCharge;
	public float CurrentDistance => stats.maxDistance * CurrentChargeAmount;

	private void Start()
	{
		if (!TryGetComponent(out stats))
        {
			Debug.LogError($"ShurikenShooter : 해당 캐릭터에서 CharacterStats 컴포넌트를 찾을 수 없음! (Instance ID : {this.GetInstanceID()})");
        }
		mover = GetComponent<Mover>();
		maxCharge = stats.maxChargeAmount; //maxChargeAmount로 수정이 안됨. 왜지?
		maxCartridge = stats.maxCartridgeNum;
		currentCartridge = maxCartridge;
		shurikenCount = 0;

		shurikenIndicator = transform.GetComponentInChildren<ShurikenIndicator>();
	}

	private void Update()
	{
		shurikenIndicator?.SetPosition(0, transform.position);

		if (IsCharging)
		{
			//Debug.Log(dir);
			shurikenIndicator?.SetPosition(1, transform.position + (Vector3)direction * CurrentDistance);
			
			//Debug.Log(dir);

			currentCharge += Time.deltaTime * stats.chargeSpeed;
			currentCharge = Mathf.Min(currentCharge, maxCharge);
			if (shurikenIndicator != null)
			{
				Color newColor;
				if (currentCharge >= 7)
				{
					newColor = new Color(1.0f, 0.0f, 0.0f, .3f); 
					chargeDamageMultiplier = 2f;
				}
				else if (currentCharge >= 6)
				{
					newColor = new Color(0.9f, 0.2f, 0.8f, .3f); // 파란빛
					chargeDamageMultiplier = 1.75f;
				}
				else if (currentCharge >= 5)
				{
                    newColor = new Color(0.7f, 0.4f, 0.8f, .3f); // 파란빛
					chargeDamageMultiplier = 1.5f;
				}
				else if (currentCharge >= 4)
				{
					Debug.Log("asd");
					newColor = new Color(0.3f, 0.2f, 0.8f, .3f); // 파란빛
					chargeDamageMultiplier = 1.25f;
				}
				else if (currentCharge >= 3)
				{
					newColor = new Color(0.0f, 0.0f, 0.8f, .3f); // 파란빛
					chargeDamageMultiplier = 1f;
				}
				else if (currentCharge >= 2)
				{
					newColor = new Color(0.0f, 0.6314f, 0.6902f, .3f); // 진한 청록빛
					
					chargeDamageMultiplier = 0.75f;
				}
				else if (currentCharge >= 1)
				{
					newColor = new Color(0.0f, 0.7490f, 0.6275f, .3f); // 진한 초록빛
					chargeDamageMultiplier = 0.5f;
				}
				else
				{
					newColor = new Color(0.4078f, 0.9922f, 0.8902f, .3f);
					chargeDamageMultiplier = 0.25f;
				}
				shurikenIndicator.SetColor(newColor);
			}
			
			SwitchChargeParticle();
		}
		else
		{
			shurikenIndicator?.SetPosition(1, transform.position);
			if (chargeParticle != null)
			{
				chargeParticle.SetActive(false);
			}
		}
	}
	public bool StartCharge()
	{
		if (CanShoot && !IsCharging)
        {
			currentCharge += Time.deltaTime;
			rawSlowNum = mover.speed*slowOnCharge;
			mover.speed -= rawSlowNum;
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
				OnFinishCharging();
				return true;
            }
		}
		return false;
    }
	public void SetDirection(Vector2 _direction)
    {
		if (_direction != Vector2.zero)
        {
			direction = _direction.normalized;
			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle);
		}
    }
	bool TryShoot()
    {
		if (currentCartridge > 0 && GameManager.Instance.isBattleStart)
		{
			float angle = -launchAngle;
			if(stats.shurikenNum==1)
				Shoot(direction, false);
			else
			{
				for (int i = 0; i < stats.shurikenNum; i++)
				{
					float radAngle = angle*Mathf.Deg2Rad;
					Vector2 td
						= new Vector3(
							Mathf.Cos(radAngle)*direction.x - Mathf.Sin(radAngle)*direction.y,
							Mathf.Sin(radAngle)*direction.x + Mathf.Cos(radAngle)*direction.y);
					//Vector3 td = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
					if((int)stats.shurikenNum/2==i)
						Shoot(td,false);
					else
						Shoot(td,true);
					Debug.Log($"i: {i}, dir: {direction}, cur: {td}, angle: {angle}");
					angle += launchAngle*2f/(stats.shurikenNum-1);

					if (i != stats.shurikenNum / 2)
					{
						shurikenDamageScale = ghostShurikenDamageScale;
					}
					else
					{
						shurikenDamageScale = 100f;
					}
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

		//Debug.Log($"Shoot : transform.tag = {transform.tag}");
		//총알 이미지 변경
		if (transform.CompareTag("Player1"))
		{
			Sprite kunaiRed = Resources.Load<Sprite>("Prefabs/Sprites/KunaiRed");
			inst.GetComponent<SpriteRenderer>().sprite = kunaiRed;
		}
		else
		{
			Sprite kunaiBlue = Resources.Load<Sprite>("Prefabs/Sprites/KunaiBlue");
			inst.GetComponent<SpriteRenderer>().sprite = kunaiBlue;
		}

		//Debug.Log(inst.speed);
		//슈리켄 값 받아와서 해당 값에 대한 설정
		Shuriken instSrk = inst.GetComponent<Shuriken>();
		instSrk.damageLayer = damageLayer;
		instSrk.owner = gameObject;
		instSrk.damage = stats.attackPower * chargeDamageMultiplier * (shurikenDamageScale / 100);
		instSrk.moveDistance = CurrentDistance;
		instSrk.isShadow = isShadow;
		instSrk.chargeAmount = CurrentChargeAmount;
		
		//특대형 수리검
		instSrk.transform.localScale *= 1f + stats.shurikenScale;
		
		//플레이어 수리검 UI 연동
		if (TryGetComponent<PlayerController>(out var _))
        {
	        int playerNum = transform.CompareTag("Player1") ? 1 : 2;
			catridgeUIManager.ChangeCurrentKunai(playerNum);
        }

		inst.speed = stats.shurikenSpeed * CurrentChargeAmount;
		foreach (var a in stats.shurikenAttributes)
        {
			if (shurikenCount % a.GetActivateNumber() == 0)
            {
				instSrk.AddAttribute(a);
            }
        }
		
		// 파티클 정보 전달
		instSrk.GetComponent<ShurikenParticleCreator>().shurikenShooter = this;
	}

	public void Cancel()
	{
		OnFinishCharging();
	}
	void OnFinishCharging()
    {
		if (IsCharging)
		{
			mover.speed += rawSlowNum;
			currentCharge = 0f;
		}
	}

	public void AddCurrentCartridge(int _amount)
	{
		currentCartridge += _amount;
		//수리검 UI 연동
		if (TryGetComponent<PlayerController>(out var _))
		{
			int playerNum = transform.CompareTag("Player1") ? 1 : 2;
			catridgeUIManager.ChangeCurrentKunai(playerNum);
		}
	}

	public int GetcurrentCartridge()
	{
		return currentCartridge;
	}

	public void SwitchChargeParticle()
	{
		if (chargeParticle != null)
		{
			if (transform.tag == "Player1") //플레이어 1
			{
				Material chargeMat = (Material)Resources.Load("Prefabs/Particles/ChargeParticleRed");
				Material sketchMat = (Material)Resources.Load("Prefabs/Particles/SketchParticleRed");
				chargeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().GetComponent<Renderer>().material = chargeMat;
				chargeParticle.transform.GetChild(1).GetComponent<ParticleSystem>().GetComponent<Renderer>().material = sketchMat;
			}
			else //플레이어 2
			{
				//Debug.Log("플레이어2");
				Material chargeMat = (Material)Resources.Load("Prefabs/Particles/ChargeParticleBlue");
				Material sketchMat = (Material)Resources.Load("Prefabs/Particles/SketchParticleBlue");
				chargeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().GetComponent<Renderer>().material = chargeMat;
				chargeParticle.transform.GetChild(1).GetComponent<ParticleSystem>().GetComponent<Renderer>().material = sketchMat;
			}
			chargeParticle.SetActive(true);
		}
	}
}
