using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectParameter
{
	public StatusEffectController.StatusEffect status;
	public float duration = 3f;
	public float value = 0f;
	public Vector2 dir = new Vector2();

	public StatusEffectParameter(StatusEffectController.StatusEffect _status)
	{
		status = _status;
	}

}
public class StatusEffectController : MonoBehaviour
{
	public enum StatusEffect
	{
		KNOCKBACK,
		SLOW,
		MORE_DAMAGE_TAKEN,
		INHIBIT_HEAL
	}

	private Mover mover;
	private ShurikenShooter shooter;
	private Damageable damageable;

	private List<StatusEffectParameter> effectList = new List<StatusEffectParameter>();

	private void Start()
	{
		mover = GetComponent<Mover>();
		shooter = GetComponent<ShurikenShooter>();
		damageable = GetComponent<Damageable>();
	}

	void Update()
	{
		for(int i=0;i<effectList.Count;)
		{
			var effect = effectList[i];
			switch (effect.status)
			{
				case StatusEffect.KNOCKBACK:
					mover.CanMove = false;
					transform.position = (Vector2)transform.position +effect.dir*(Time.deltaTime*effect.value);
					break;
			}
			effect.duration -= Time.deltaTime;
			if (effect.duration <= 0f)
			{
				RemoveStatusEffect(i);
			}
			else
			{
				i += 1;
			}
		}
	}

	public void AddStatusEffect(StatusEffectParameter _data)
	{
		Debug.Log("상태 추가! : "+_data.status);
		effectList.Add(_data);
		switch (_data.status)
		{
			case StatusEffect.KNOCKBACK:
				_data.duration = _data.value;
				break;
			case StatusEffect.SLOW:
				_data.value = mover.speed*_data.value;
				mover.speed -= _data.value;
				break;
			case StatusEffect.MORE_DAMAGE_TAKEN:
				damageable.damageCoef += _data.value;
				break;
			case StatusEffect.INHIBIT_HEAL:
				damageable.canHeal = false;
				break;
		}
	}

	public void RemoveStatusEffect(int _index)
	{
		StatusEffectParameter _data = effectList[_index];
		switch (_data.status)
		{
			case StatusEffect.SLOW:
				mover.speed += _data.value;
				break;
			case StatusEffect.MORE_DAMAGE_TAKEN:
				damageable.damageCoef -= _data.value;
				break;
			case StatusEffect.INHIBIT_HEAL:
				damageable.canHeal = true;
				break;
		}
		effectList.RemoveAt(_index);
	}
}
