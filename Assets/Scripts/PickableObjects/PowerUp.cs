using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickable
{
    [SerializeField] private float attackPowerUpAmount = 10f;

    public override void Pick(Collider2D _collision)
    {
        base.Pick(_collision);
        /*
        if (_collision.TryGetComponent<Damageable>(out Damageable damageable))
        {
        ���� ������Ʈ ������ ���� ��
        */
            gameObject.SetActive(false);
            Debug.Log("PowerUp");
       // }
    }
}
