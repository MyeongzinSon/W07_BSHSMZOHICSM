using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamager : MonoBehaviour
{
	public LayerMask damageLayer;
	public float damage = 3;
	public float destroyTimer = 1f;
	private float timer = 0f;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		int targetLayer = 1<<other.gameObject.layer;
		if ((targetLayer & damageLayer) > 0)
		{
			if (other.gameObject.TryGetComponent<Damageable>(out var d))
			{
				d.Hit(damage);
			}
		}
	}
	
	
	private void Update()
	{
		if (destroyTimer > 0)
		{
			timer += Time.deltaTime;
			if (timer >= destroyTimer)
			{
				Destroy(gameObject);
			}
		}
	}
}
