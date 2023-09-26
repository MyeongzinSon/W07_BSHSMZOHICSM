using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIStandard : EnemyAI
{
    protected bool IsCharging => main.Attack.IsCharging;
    protected bool CanAttack => main.Attack.CanShoot;
    public override bool UpdateOnAttack()
    {
        return false;
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
        return false;
    }

    public override bool UpdateOnIdle()
    {
        return false;
    }

    public override bool EvaluateTarget()
    {
        throw new System.NotImplementedException();
    }
}

