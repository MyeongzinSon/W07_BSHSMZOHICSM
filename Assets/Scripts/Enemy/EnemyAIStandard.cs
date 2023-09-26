using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIStandard : EnemyAI
{
    protected bool IsCharging => main.Attack.IsCharging;
    protected bool CanAttack => main.Attack.CanShoot;
    protected bool shouldRoll = false;

    Vector2 attackDiff;
    Transform escapeTarget;
    Vector2 dodgeDirection;
    bool isReloading = false;

    public override void OnUpdate()
    {
        attackDiff = main.AttackTarget.position - main.transform.position;

        if (EvaluateTarget()) { }
        CheckReloading();
        CheckEscape();

        if (UpdateOnDodge())
        {
            return;
        }
        else
        {
            dodgeDirection = Vector2.zero;
        }

        // follow current Target
        if (UpdateOnFollowTarget()) return;

        if (UpdateOnMaintainDistance()) return;

        // attack Player;
        bool isAttacking = UpdateOnAttack();
        if (isAttacking) return;
    }

    public override bool UpdateOnFollowTarget()
    {
        if (main.CurrentTarget != null)
        {
            var diff = main.CurrentTarget.position - main.transform.position;
            move.SetDestination(main.CurrentTarget.position);
            TryRoll();
            if (diff.magnitude < main.TargetPositionOffset)
            {
                if (main.CurrentTarget.Equals(escapeTarget))
                {
                    GameObject.Destroy(escapeTarget.gameObject);
                    escapeTarget = null;
                }
                main.SetTarget(null);
                move.SetDirection(Vector2.zero);
            }
            return true;
        }
        if (main.CurrentTarget == null)
        {
            move.SetDirection(Vector2.zero);
        }
        return false;
    }
    public override bool UpdateOnAttack()
    {
        if (isReloading) return false;

        move.SetRotationTo(attackDiff.normalized);
        if (IsCharging)
        {
            if (attack.CurrentDistance > attackDiff.magnitude + attackDistanceOffset || attack.CurrentChargeAmount == 1)
            {
                attack.SetDirection(attackDiff.normalized);
                if (attack.EndCharge()) return true;
                else
                {
                    Debug.LogWarning($"AI에서 attack.EndCharge를 성공하지 못함!");
                    return false;
                }
            }
        }
        if (CanAttack && !IsCharging)
        {
            if (attack.StartCharge()) return true;
            else
            {
                Debug.LogWarning($"AI에서 attack.StartCharge를 성공하지 못함!");
                return false;
            }
        }
        return false;
    }
    public virtual bool UpdateOnMaintainDistance()
    {
        if (isReloading || IsCharging) return false;

        var distance = attackDiff.magnitude;
        if (distance < minProperDistance)
        {
            move.SetDirection(-attackDiff);
            TryRoll();
            return true;
        }
        if (distance > maxProperDistance)
        {
            move.SetDirection(attackDiff);
            TryRoll();
            return true;
        }
        return false;
    }

    public override bool UpdateOnIdle()
    {
        return false;
    }

    public override bool EvaluateTarget()
    {
        var needHeal = true;
        if (needHeal)
        {
            var healpacks = GameObject.FindObjectsOfType<HealPack>().ToList();
            if (TrySetNearestTarget(healpacks))
            {
                return true;
            }
        }

        if (isReloading)
        {
            var shurikens = GameObject.FindObjectsOfType<Shuriken>()
                .Where(s => s.owner.Equals(main.gameObject)
                    && s.state == Shuriken.ShurikenState.PICKUP
                    ).ToList();

            if (TrySetNearestTarget(shurikens))
            {
                return true;
            }
        }

        return false;
    }

    bool TrySetNearestTarget<T>(List<T> list) where T : MonoBehaviour
    {
        if (list.Count > 0)
        {
            //list.ForEach(item => Debug.Log($"{item.name}, {item.GetInstanceID()}"));
            var desiredTarget = list.OrderBy(s => Vector3.Distance(main.transform.position, s.transform.position))
                .First().transform;
            main.SetTarget(desiredTarget);
            return true;
        }
        return false;
    }

    void CheckReloading()
    {
        if (!isReloading)
        {
            if (!CanAttack)
            {
                isReloading = true;
            }
        }
        else
        {
            if (attack.IsCartridgeFull)
            {
                isReloading = false;
            }
        }
    }
    void CheckEscape()
    {
        if (main.CurrentTarget != null) return;

        var epsilon = 1.5f;
        if (attackDiff.magnitude < minProperDistance)
        {
            if (Physics2D.Raycast(main.transform.position, move.direction, epsilon, 1 << LayerMask.NameToLayer("Wall")))
            {
                var newTarget = new GameObject();
                newTarget.name = "EscapeTarget";
                escapeTarget = newTarget.transform;
                escapeTarget.position = main.AttackTarget.position + (Vector3)attackDiff.normalized * epsilon;
                main.SetTarget(escapeTarget);
            }
        }
    }
    bool UpdateOnDodge()
    {
        if (!canDodgeRoll) return false;

        var shurikens = GameObject.FindObjectsOfType<Shuriken>()
                .Where(s => s.owner.Equals(main.AttackTarget.gameObject)
                    && s.state == Shuriken.ShurikenState.ATTACK
                    ).ToList();

        if (shurikens.Count == 0) return false;

        var dangerShurikens = shurikens.
            Where(s =>
            {
                var shuriken = s.GetComponent<Shuriken>();
                var direction = shuriken.direction;
                var distance = shuriken.moveDistance;
                var hit = Physics2D.Raycast(s.transform.position, direction, distance, 1 << LayerMask.NameToLayer("Enemy"));
                return hit;
            }).OrderBy(s => Vector3.Distance(s.transform.position, main.transform.position))
            .ToList();

        if (dangerShurikens.Count == 0) { return false; }

        var targetShuriken = dangerShurikens.First();
        var front = targetShuriken.direction;
        var right = new Vector2(front.y, -front.x);
        var left = -right;

        if (IsCharging)
        {
            attack.SetDirection(attackDiff.normalized);
            if (attack.EndCharge()) { }
            else
            {
                Debug.LogWarning($"AI에서 attack.EndCharge를 성공하지 못함!");
            }
        }

        if (dodgeDirection.Equals(Vector2.zero))
        {
            dodgeDirection = Random.value > 0.5 ? right : left;
        }
        
        move.SetDirection(dodgeDirection);
        TryRoll(true);

        return true;
    }
    bool TryRoll(bool forDodge = false)
    {
        var condition = forDodge ? true : useRoll;
        if (condition && roll.CanRoll)
        {
            roll.TryRoll();
            return true;
        }
        return false;
    }
}

