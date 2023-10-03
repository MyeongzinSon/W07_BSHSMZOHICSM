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

	//curse option
	public int curseStack = 0;
	private GameObject curseContainer;
	private void Start()
	{ 
		if (TryGetComponent<CharacterStats>(out var stats))
        {
			maxHp = stats.maxHp;
        }
		hp = maxHp;
		curseContainer = transform.Find("CurseStackContainer").gameObject;
	}

	private void Update()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			TestEnemyDamage(1);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			TestEnemyDamage(2);
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

		if (curseStack > 0)
		{
			GameObject curseStackPrefab = (GameObject)Resources.Load("Prefabs/UI/CurseStack");

			for (int i = 0; i < curseStack; i++)
			{
				GameObject curseStack = Instantiate(curseStackPrefab, curseContainer.transform.position + new Vector3(i * .8f, 0f, 0f), Quaternion.identity);
				curseStack.transform.parent = curseContainer.transform;
			}
			
		}
		
		if (curseStack >= 3)
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
	        GameManager.Instance.StageClear(2);
        }
        else //플레이어2 승리
        {
	        GameManager.Instance.StageClear(1);
        }
	}
	
	public void TestEnemyDamage(int num)
	{
		if (gameObject.tag == "Player" + num.ToString())
		{
			Hit(50f);
		}
	}
}
