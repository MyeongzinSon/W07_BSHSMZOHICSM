using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
	public float maxHp = 32;
	public float hp = 32;

	public float damageCoef = 1f;
	public bool canHeal = true;

	private bool isKilled = false;

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
		Debug.Log("Damaged: "+name + " " + Time.time);
		hp -= damage * damageCoef;
		
		GameObject damageTextPrefab = (GameObject)Resources.Load("Prefabs/UI/DamageText");
		GameObject damageText = Instantiate(damageTextPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
		damageText.GetComponent<MoveAndDestroy>()._text = "-" + (damage * damageCoef).ToString("F0");
		
		GameObject hitParticle = (GameObject)Resources.Load("Prefabs/Particles/BloodParticle");
		Instantiate(hitParticle, transform.position, Quaternion.identity);
		if (gameObject.activeSelf)
		{
			transform.GetChild(0).GetComponent<FlashingObject>().MakeObjectFlashSprite(transform.GetChild(0).GetComponent<SpriteRenderer>(), .03f, .1f);
		}
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
		if (isKilled)
			return;
		isKilled = true;
		Debug.Log("Killed: "+name);
        transform.GetComponent<PlayerHpHandler>().playerHpBar.GetComponent<Image>().fillAmount = 0;
        transform.GetComponent<PlayerHpHandler>().SetPlayerTextToZero();
		gameObject.SetActive(false);
        if (gameObject.tag == "Player1") //플레이어1 승리
        {
	        /*
	        Debug.Log("Game Over");
	        GameObject gameOverText = GameObject.Find("IngameCanvas").transform.Find("GameOverPanel").gameObject;
	        gameOverText.SetActive(true);
	        */
	        GameManager.Instance.StageClear(1);
        }
        else //플레이어2 승리
        {
	        GameManager.Instance.StageClear(2);
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
