using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public float maxHp = 32;
	public float hp = 32;

	public float damageCoef = 1f;
	public bool canHeal = true;

	private void Start()
	{ 
		if (TryGetComponent<CharacterStats>(out var stats))
        {
			maxHp = stats.maxHp;
        }
		hp = maxHp;
	}

	private void Update()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			TestEnemyDamage();
		}
		#endif
	}

	public void Hit(float damage)
	{
		Debug.Log("Damaged: "+name);
		hp -= damage * damageCoef;
		GameObject hitParticle = (GameObject)Resources.Load("Prefabs/Particles/BloodParticle");
		Instantiate(hitParticle, transform.position, Quaternion.identity);
		if (hp <= 0)
		{
			hp = 0f;
			Kill();
		}
	}

	public void Heal(float amout)
	{
		if (!canHeal)
			return;
		Debug.Log("Healed: " + name);
		hp += amout;
		if (hp >= maxHp)
		{
			hp = maxHp;
		}
	}

	public void Kill()
	{
		Debug.Log("Killed: "+name);
		gameObject.SetActive(false);
        
        //플레이어가 죽었으면 게임오버
        if (gameObject.tag == "Player")
        {
	        Debug.Log("Game Over");
        }
        else //적이 죽었으면 스테이지 클리어
        {
	        GameManager.Instance.StageClear();
        }
       
	}
	
	public void TestEnemyDamage()
	{
		if (gameObject.tag == "Enemy")
		{
			Hit(50f);
		}
	}
}
