using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIStandard : EnemyAI
{
    protected bool IsCharging => main.Attack.IsCharging;
    protected bool CanAttack => main.Attack.CanShoot;

    Vector2 attackDiff;
    bool isReloading = false;

    public override void OnUpdate()
    {
        attackDiff = main.AttackTarget.position - main.transform.position;

        // follow current Target
        if (UpdateOnFollowTarget()) return;

        // attack Player;
        bool isAttacking = UpdateOnAttack();
        if (isAttacking) return;


        if (EvaluateTarget())
        {

        }

        CheckReloading();
    }

    public override bool UpdateOnFollowTarget()
    {
        if (main.CurrentTarget != null)
        {
            var diff = main.CurrentTarget.position - main.transform.position;
            move.direction = diff;
            if (diff.magnitude < main.TargetPositionOffset)
            {
                main.SetTarget(null);
                move.direction = Vector2.zero;
            }
            return true;
        }
        if (main.CurrentTarget == null)
        {
            move.direction = Vector2.zero;
        }
        return false;
    }

    public override bool UpdateOnAttack()
    {
        if (isReloading) return false;

        if (IsCharging)
        {
            if (attack.CurrentDistance > attackDiff.magnitude + main.AttackDistanceOffset || attack.CurrentChargeAmount == 1)
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

    public override bool UpdateOnIdle()
    {
        return false;
    }

    public override bool EvaluateTarget()
    {
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

        var needHeal = true;
        if (needHeal)
        {
            var healpacks = GameObject.FindObjectsOfType<HealPack>().ToList();
            if (TrySetNearestTarget(healpacks))
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
            list.ForEach(item => Debug.Log($"{item.name}, {item.GetInstanceID()}"));
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
}

