using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyAI
{
    protected EnemyController main;
    protected PlayerMove move;
    protected PlayerRoll roll;
    protected ShurikenShooter attack;

    public void Initialize(EnemyController _main)
    {
        main = _main;
        move = main.Move;
        roll = main.Roll;
        attack = main.Attack;
    }

    public abstract void OnUpdate();
    public abstract bool UpdateOnFollowTarget();
    public abstract bool UpdateOnAttack();
    public abstract bool UpdateOnIdle();
    public abstract bool EvaluateTarget();
}
