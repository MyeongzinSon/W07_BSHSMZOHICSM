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

    [Header("움직일 거리")]
    public float moveDistance = 3f;

    #region privateValues

    private TestMover mover;
    private float movedDistance = 0f;

    #endregion

    private void Awake()
    {
        mover = GetComponent<TestMover>();
    }

    private void Update()
    {
        //이동한 거리 계산
        movedDistance += mover.moveSpeed*Time.deltaTime;
        if (movedDistance >= moveDistance)
        {
            Debug.Log("이동 가능 거리 달성");
            Destroy(gameObject);
        }
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
