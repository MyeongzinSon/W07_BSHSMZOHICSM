using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shuriken : MonoBehaviour
{
    [Header("수리검의 데미지")]
    public float damage = 3f;
    public bool canDamage = true;

    [Header("충돌 레이어 설정")]
    [Header("데미지를 줄 레이어")]
    public LayerMask damageLayer;
    [Header("충돌 시 튕길 레이어")]
    public LayerMask bounceLayer;
    public bool canReflect = false;
    [FormerlySerializedAs("bounceWallTime")] public float wallBounceTime = 0.3f;     //벽 충돌 시 튕겨나갈 시간
    [FormerlySerializedAs("bounceDamageTime")] public float enemyBounceTime = 1.0f;   // 적 충돌 시 튕겨나갈 시간

    [Header("움직일 거리")]
    public float moveDistance = 3f;

    #region privateValues

    private Mover mover;
    private float movedDistance = 0f;
    private BoxCollider2D collider;

    #endregion

    private void Awake()
    {
        mover = GetComponent<Mover>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //이동한 거리 계산
        movedDistance += mover.speed*Time.deltaTime;
        if (movedDistance >= moveDistance)
        {
            Debug.Log("이동 가능 거리 달성");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        float angle = mover.SetRotationByDirection();
        RaycastHit2D hit 
            = Physics2D.BoxCast(
                (Vector2)transform.position + collider.offset, 
                collider.size, 
                angle, 
                mover.direction, 
                Time.fixedDeltaTime*mover.speed*3,
                bounceLayer|damageLayer);
        //벽 등 Bounce 대상과 충돌 시
        if (hit.collider != null)
        {
            
            int targetLayer = (1 << hit.collider.gameObject.layer);
            
            //적 타격 시
            if (canDamage&&(targetLayer & damageLayer) > 0)
            {
                    
                if (hit.collider.gameObject.TryGetComponent<Damageable>(out var target))
                {
                    mover.direction = GetReflectVector(mover.direction, hit.normal);
                    target.Hit(damage);
                    StartCoroutine(BounceCoroutine(enemyBounceTime));
                }
            }
            //벽에 부딪힘
            if ((targetLayer & bounceLayer) > 0)
            {
                mover.direction = GetReflectVector(mover.direction, hit.normal);
                //리플렉트가 불가능하다면, 벽 반사 움직임 코루틴 시작, 가능하다면 그냥 방향만 바뀌고 쭊 날아감
                if (!canReflect)
                {
                    StartCoroutine(BounceCoroutine(wallBounceTime));
                }
            }
            
        }
    }
    Vector2 GetReflectVector(Vector2 _dir, Vector2 _normal)
    {
        // 충돌한 객체의 법선 벡터를 가져옴
        Vector2 n = _normal;         //법선벡터
        Vector2 p = _dir;    //입사벡터
        //P + 2n(-P dot N)
        Vector2 reflect = p + 2*(-p.x*n.x - p.y*n.y)*n;

        //Vector2 reflect = hit.normal;
        return reflect.normalized;
    }

    IEnumerator BounceCoroutine(float _moveTime)
    {
        //벽에 충돌 후 더이상 데미지를 주지 않는다.
        
        canDamage = false;
        float orgSpeed = mover.speed*0.5f;
        float timer = _moveTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            mover.speed = orgSpeed*timer/_moveTime;
            yield return null;
        }
        Debug.Log("벽에 부딪히고 dur초 경과!");
        Destroy(gameObject);
    }
}
