using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("수리검의 데미지")]
    public float damage = 3f;

    [Header("충돌 레이어 설정")]
    [Header("데미지를 줄 레이어")]
    public LayerMask damageLayer;
    [Header("충돌 시 튕길 레이어")]
    public LayerMask bounceLayer;

    #region privateValues

    private TestMover mover;

    #endregion

    private void Awake()
    {
        mover = GetComponent<TestMover>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int targetLayer = (1 << other.gameObject.layer);
        //데미지 대상과 충돌 시
        if ((targetLayer & damageLayer) > 0)
        {
            if (other.TryGetComponent<TestDamageable>(out var target))
            {
                target.Hit(damage);
            }
        }
        //벽 등 바운스 대상과 충돌 시
        if ((targetLayer & bounceLayer) > 0)
        {
        }
    }
}
