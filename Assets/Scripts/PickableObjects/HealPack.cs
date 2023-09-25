using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Pickable
{
    [SerializeField] private float healAmount = 10f;

    public override void Pick(Collider2D _collision)
    {
        base.Pick(_collision);
        if (_collision.TryGetComponent<Damageable>(out Damageable damageable))
        {
            gameObject.SetActive(false);
            damageable.Heal(healAmount);
            Debug.Log("Heal");
        }
    }
}
