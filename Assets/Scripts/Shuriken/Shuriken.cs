using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Shuriken : MonoBehaviour
{
    public enum ShurikenState
    {
        ATTACK, PICKUP,
    }

    public ShurikenState state;
    public GameObject owner;
    
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

    [Header("유도탄 움직임 관련")]
    public bool useGuidedMove = false;
    public float maxGuidedAngularSpeed = 1f;
    public Transform guidedTarget;

    [Header("폭발 수리검")]
    public ExplosionDamager explosionPrefab;
    public bool useExplosion = false;
    public float explosionScale;
    public float explosionTime = 1f;

    [Header("자동 수거")]
    public bool useBoomerang;
    public float boomerangDelay = 5f;
    private bool isBoomerangMoving = false;
    [Header("수리검 속성")]
    public List<ShurikenAttribute> attributes = new();

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

    public void Start()
    {
        state = ShurikenState.ATTACK;
    }

    private void Update()
    {
        //이동한 거리 계산
        if (state == ShurikenState.ATTACK)
        {
            movedDistance += mover.speed*Time.deltaTime;
            if (movedDistance >= moveDistance)
            {
                SetPickUpState();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isBoomerangMoving)
        {
            //부메랑 이동 중일 경우, 충돌 및 유도탄 모두 무시
            return;
        }
        
        //충돌 처리
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
                    SetPickUpState();
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
                    SetPickUpState();
                    StartCoroutine(BounceCoroutine(wallBounceTime));
                }
            }
            
        }
        
        //유도 처리
        if (useGuidedMove&&canDamage)
        {
            AdaptGuidedMove();
        }
    }

    void AdaptGuidedMove()
    {
        if (guidedTarget==null)
        {
            GameObject g = GameObject.FindWithTag("Enemy");
            if (g!=null)
            {
                guidedTarget = g.transform;
            }
        }
        if (guidedTarget!=null)
        {
            Vector3 target = (guidedTarget.position - transform.position).normalized;
            mover.direction = Vector3.Slerp(mover.direction, target,Time.fixedDeltaTime*maxGuidedAngularSpeed);
        }
    }


    void Explosion()
    {
        ExplosionDamager ex = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ex.transform.localScale = new Vector3(explosionScale, explosionScale, 1f);
        ex.destroyTimer = explosionTime;
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
        mover.CanMove = true;
        canDamage = false;
        float orgSpeed = mover.speed*0.5f;
        float timer = _moveTime;
        while (timer > 0f)
        {
            //부메랑 이동이 발견되면, BounceCoroutine을 끊는다. 정말 먼 나중에 착오가 생길 수 있는 하드코딩이므로 시간이 난다면 수정 필요.
            if(isBoomerangMoving)       
                yield break;
            
            timer -= Time.deltaTime;
            mover.speed = orgSpeed*timer/_moveTime;
            yield return null;
        }
        mover.CanMove = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //PICKUP모드일 때 소환자와 만나면 사라짐
        if (state == ShurikenState.PICKUP)
        {
            if (owner == other.gameObject)
            {
                //탄창 +1
                OnPickUp();
            }
        }
    }

    IEnumerator BoomerangCoroutine()
    {
        yield return new WaitForSeconds(boomerangDelay);
        float boomerangAccel = 5f;
        mover.CanMove = true;
        isBoomerangMoving = true;
        mover.speed = 0f;
        while (true)
        {
            mover.direction = owner.transform.position - transform.position;
            mover.speed += boomerangAccel*Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    
    
    void SetPickUpState()
    {
        if (useExplosion)
        {
            Explosion();
        }
        if (useBoomerang)
        {
            StartCoroutine(BoomerangCoroutine());
        }
        attributes.ForEach(a => {
            Debug.Log($"Attribute : {a}");
        });
        state = ShurikenState.PICKUP;
        mover.CanMove = false;
    }

    void OnPickUp()
    {
        Debug.Log("회수 완료.");
        Destroy(gameObject);
    }

}
