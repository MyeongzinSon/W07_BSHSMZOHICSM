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
        ATTACK,
        AIRBORN,
        PICKUP,
    }

    public ShurikenState state;
    public GameObject owner;

    [Header("수리검 회수 가능 여부")]
    public bool isShadow = false;

    [Header("수리검의 데미지")]
    public float damage = 3f;
    public bool canDamage = true;

    [Header("충돌 레이어 설정")]
    [Header("데미지를 줄 레이어")]
    public LayerMask damageLayer;
    [Header("충돌 시 튕길 레이어")]
    public LayerMask bounceLayer;
    [FormerlySerializedAs("canReflect")] public bool useBounce = false;
    [FormerlySerializedAs("bounceWallTime")]
    public float wallBounceTime = 0.3f; //벽 충돌 시 튕겨나갈 시간
    [FormerlySerializedAs("bounceDamageTime")]
    public float enemyBounceTime = 1.0f; // 적 충돌 시 튕겨나갈 시간

    [Header("움직일 거리")]
    public float moveDistance = 3f;

    [Header("유도탄 움직임 관련")]
    public bool useGuidedMove = false;
    public float maxGuidedAngularSpeed = 1f;
    public Transform guidedTarget;

    [Header("폭발 수리검")]
    public ExplosionDamager explosionPrefab;
    public bool useExplosion = false;
    public float explosionScale = 10f;
    public float explosionTime = 1f;
    public float explosionDamageRatio = 0.3f; //수리검 데미지의 n%의 데미지
    private bool isExploded = false;

    [Header("부메랑")]
    public bool useBoomerang;
    public float boomerangDelay = 5f;
    public float boomerangAccel = 3f;
    private bool isBoomerangReturning = false;
    [Header("수리검 속성")]
    public List<ShurikenAttribute> attributes = new();

    [Header("데미지 줄 때 값 처리")]
    public bool useLifeSteal = false;
    [FormerlySerializedAs("healAmount")] public float lifeStealAmount = 2f;
    public bool useKnockBack = false;
    public float knockbackPower = 5f;
    public bool useSlow = false;
    public bool useDamageBuff = false;
    public bool useInhibitHeal = false;
    private SpriteOutline spriteOutline;

    #region privateValues

    private Mover mover;
    private float movedDistance = 0f;
    private BoxCollider2D collider;

    #endregion

    private void Awake()
    {
        mover = GetComponent<Mover>();
        collider = GetComponent<BoxCollider2D>();
        spriteOutline = GetComponent<SpriteOutline>();
    }

    public void Start()
    {
        state = ShurikenState.ATTACK;
    }

    public void AddAttribute(ShurikenAttribute attr)
    {
        attributes.Add(attr);

        //bool 변수로 컨트롤 되는 경우 수정
        switch (attr)
        {
            case ShurikenAttribute.Boomerang:
                useBoomerang = true;
                break;
            case ShurikenAttribute.Guidance:
                useGuidedMove = true;
                break;
            case ShurikenAttribute.BounceOnWall:
                useBounce = true;
                break;
            case ShurikenAttribute.ExplodeOnHit:
                useExplosion = true;
                break;
            case ShurikenAttribute.Vampire:
                useLifeSteal = true;
                break;
            case ShurikenAttribute.KnockbackToWall:
                useKnockBack = true;
                break;
            case ShurikenAttribute.Slow:
                useSlow = true;
                break;
            case ShurikenAttribute.Vulnerable:
                useDamageBuff = true;
                break;
            case ShurikenAttribute.HealReduction:
                useInhibitHeal = true;
                break;
        }
    }

    void CalculateMoveDistance()
    {
        //이동한 거리 계산
        if (state == ShurikenState.ATTACK)
        {
            movedDistance += mover.speed*Time.deltaTime;

            if (movedDistance >= moveDistance)
            {
                if (useBoomerang)
                {
                    if (!isBoomerangReturning)
                    {
                        //부메랑 모드일 경우, 공격을 유지하고 돌아오는 중으로 체크
                        mover.speed = 0f;
                        isBoomerangReturning = true;
                    }
                }
                else
                {
                    SetPickUpState();
                    Debug.Log($"{owner.name} 거리 이동완료, ");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isBoomerangReturning)
        {
            //부메랑 리턴은 PICKUP이어도 작동한다.
            AdaptBoomerangReturnMove();
        }

        if (state == ShurikenState.PICKUP && owner.tag == "Player")
        {
            spriteOutline.UpdateOutline(true);
            return;
        }
        else
        {
            spriteOutline.UpdateOutline(false);
        }
            
        CalculateMoveDistance();

        //유도 처리
        if (useGuidedMove && canDamage)
        {
            AdaptGuidedMove();
        }
        //충돌 처리
        float angle = mover.SetRotationByDirection();
        RaycastHit2D hit
            = Physics2D.BoxCast(
                (Vector2) transform.position + collider.offset,
                collider.size,
                angle,
                mover.direction,
                Time.fixedDeltaTime*mover.speed,
                bounceLayer | damageLayer);
        //충돌 체크
        if (hit.collider != null)
        {
            int targetLayer = (1 << hit.collider.gameObject.layer);

            //적과 충돌하였는가?
            if (canDamage && (targetLayer & damageLayer) > 0)
            {
                //적 
                if (hit.collider.gameObject.TryGetComponent<Damageable>(out var target))
                {
                    canDamage = false;
                    if (isBoomerangReturning)
                    {
                        isBoomerangReturning = false;
                    }
                    //대상에게 공격 판정
                    target.Hit(damage);
                    OnHitEnemy(target);

                    //PickUp으로 변경 및 Drop
                    float dropDistance = 5f;
                    Vector3 dropPos = transform.position + (Vector3) mover.direction*dropDistance;

                    //레이캐스트를 쏴서, 벽이 없는지 확인한다.
                    RaycastHit2D hitToDrop = Physics2D.Raycast(transform.position, dropPos - transform.position, dropDistance, bounceLayer);

                    if (hitToDrop.collider != null)
                    {
                        //벽이 있다면, 가장 가까운 거리에 Drop한다.
                        dropPos = transform.position + (Vector3) mover.direction*(hitToDrop.distance - collider.size.y*0.5f);
                    }
                    state = ShurikenState.AIRBORN;
                    StartCoroutine(DropCoroutine(transform.position, dropPos));
                    //SetPickUpState();
                    //StartCoroutine(BounceCoroutine(enemyBounceTime));
                }
            }
            //벽에 부딪힘
            if ((targetLayer & bounceLayer) > 0)
            {
                if(!isBoomerangReturning)
                    mover.direction = GetReflectVector(mover.direction, hit.normal);
                //리플렉트가 불가능하다면, 벽 반사 움직임 코루틴 시작, 가능하다면 그냥 방향만 바뀌고 쭊 날아감
                if (!useBounce)
                {
                    canDamage = false;
                    StartCoroutine(BounceCoroutine(wallBounceTime));
                }
                //부메랑 모드
                //벽과 충돌 시, 공격은 유지하면서, 방향만 플레이어쪽으로 향한다.
                if (useBoomerang)
                {
                    mover.speed = 0f;
                    isBoomerangReturning = true;
                }
            }

        }


    }

    private IEnumerator DropCoroutine(Vector2 currentPos, Vector2 dropPos)
    {
        float elapsedTime = 0f;
        float maxTime = 1f;
        float rotationPerTick = 20;

        Vector2 p1 = currentPos;
        Vector2 p3 = dropPos;

        Vector2 midpoint = (p1 + p3) / 2;

        // 원하는 방향 벡터
        Vector2 direction = transform.up;

        // 원하는 거리
        float distance = 5f; // 원하는 거리

        // 중점에서 원하는 방향과 거리만큼 떨어진 좌표 계산
        Vector2 p2 = midpoint + direction.normalized * distance;

        Vector2 b1;
        Vector2 b2;

        int tick = 0;
        while (elapsedTime < maxTime)
        {
            tick++;

            transform.Rotate(Vector3.forward, rotationPerTick * tick);
            b1 = Vector2.Lerp(p1, p2, elapsedTime / maxTime);
            b2 = Vector2.Lerp(p2, p3, elapsedTime / maxTime);
            transform.position = Vector2.Lerp(b1, b2, elapsedTime / maxTime);
            
            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
        //transform.position = dropPos;
        SetPickUpState();
    }

    void AdaptGuidedMove()
    {
        if (guidedTarget == null)
        {
            GameObject g = GameObject.FindWithTag("Enemy");
            if (g != null)
            {
                guidedTarget = g.transform;
            }
        }
        if (guidedTarget != null)
        {
            Vector3 target = (guidedTarget.position - transform.position).normalized;
            mover.direction = Vector3.Slerp(mover.direction, target, Time.fixedDeltaTime*maxGuidedAngularSpeed);
        }
    }


    void AdaptBoomerangReturnMove()
    {
        Vector2 dist = owner.transform.position - transform.position;
        mover.CanMove = true;
        mover.direction = dist.normalized;
        mover.accel = boomerangAccel;
        if (dist.magnitude < 0.5f)
        {
            SetPickUpState();
        }
        else
        {
            canDamage = true;
        }
    }


    void Explosion()
    {
        if (isExploded)
        {
            return;
        }
        isExploded = true;
        ExplosionDamager ex = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ex.transform.localScale = new Vector3(explosionScale, explosionScale, 1f);
        ex.destroyTimer = explosionTime;
        ex.damageLayer = damageLayer;
        ex.damage = damage*explosionDamageRatio;
    }


    Vector2 GetReflectVector(Vector2 _dir, Vector2 _normal)
    {
        // 충돌한 객체의 법선 벡터를 가져옴
        Vector2 n = _normal; //법선벡터
        Vector2 p = _dir; //입사벡터
        //P + 2n(-P dot N)
        Vector2 reflect = p + 2*(-p.x*n.x - p.y*n.y)*n;

        //Vector2 reflect = hit.normal;
        return reflect.normalized;
    }

    IEnumerator BounceCoroutine(float _moveTime)
    {
        //벽에 충돌 후 더이상 데미지를 주지 않는다.
        if (state == ShurikenState.PICKUP)
            yield break;
        mover.CanMove = true;
        canDamage = false;
        float orgSpeed = mover.speed*0.5f;
        float timer = _moveTime;
        while (timer > 0f)
        {
            //부메랑 이동이 발견되면, BounceCoroutine을 끊는다. 정말 먼 나중에 착오가 생길 수 있는 하드코딩이므로 시간이 난다면 수정 필요.
            if (isBoomerangReturning)
                yield break;

            timer -= Time.deltaTime;
            mover.speed = orgSpeed*timer/_moveTime;
            yield return null;
        }
        SetPickUpState();
        mover.CanMove = false;
    }

    private void OnTriggerStay2D(Collider2D other)
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


    void SetPickUpState()
    {
        if (useExplosion)
        {
            Explosion();
        }
        // if (useBoomerang)
        // {
        //     StartCoroutine(BoomerangCoroutine());
        // }


        attributes.ForEach(a => { Debug.Log($"Attribute : {a}"); });
        state = ShurikenState.PICKUP;
        mover.CanMove = false;
        
        //PICKUP모드로 들어가면, 더이상 데미지를 줄 수 없다.
        canDamage = false;


        //쉐도우라면, PICKUP으로 들어가면서 사라진다.
        if (isShadow)
        {
            Destroy(gameObject);
        }
    }

    void OnPickUp()
    {
        //Debug.Log("회수 완료.");
        owner.GetComponent<ShurikenShooter>().AddCurrentCartridge(1);
        Destroy(gameObject);
    }

    void OnHitEnemy(Damageable _target)
    {
        if (useLifeSteal)
        {
            if (owner.TryGetComponent<Damageable>(out var ownerHp))
            {
                ownerHp.Heal(lifeStealAmount);
            }
            
        }
        
        //맞았을때 적에게 디버프 효과
        if (_target.TryGetComponent<StatusEffectController>(out var sec))
        {
            if (useKnockBack)
            {
                StatusEffectParameter sep = new StatusEffectParameter(StatusEffectController.StatusEffect.KNOCKBACK);
                sep.dir = mover.direction;
                sep.duration = 1.0f;
                sep.value = 5f;
                sec.AddStatusEffect(sep);
            }
            if (useSlow)
            {
                StatusEffectParameter sep = new StatusEffectParameter(StatusEffectController.StatusEffect.SLOW);
                
                sep.duration = 3.0f;
                sep.value = 0.3f;
                sec.AddStatusEffect(sep);
            }
            if (useDamageBuff)
            {
                StatusEffectParameter sep = new StatusEffectParameter(StatusEffectController.StatusEffect.MORE_DAMAGE_TAKEN);
                
                sep.duration = 5.0f;
                sep.value = 0.5f;
                sec.AddStatusEffect(sep);
                
            }
            if (useInhibitHeal)
            {
                StatusEffectParameter sep = new StatusEffectParameter(StatusEffectController.StatusEffect.INHIBIT_HEAL);
                
                sep.duration = 5.0f;
                sec.AddStatusEffect(sep);
                
            }
        }
    }

}
