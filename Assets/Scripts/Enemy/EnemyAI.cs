using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyAI
{
    protected EnemyController main;
    protected NavMeshMover move;
    protected PlayerRoll roll;
    protected ShurikenShooter attack;

    protected bool useRoll;
    protected float rollSpeed;
    protected float rollCooldown;
    protected float minProperDistance;
    protected float maxProperDistance;
    protected float attackDistanceOffset;

    public void Initialize(EnemyController _main)
    {
        main = _main;
        move = main.Move;
        roll = main.Roll;
        attack = main.Attack;
    }

    public void SetPersonalVariables(EnemyAIData _data)
    {
        useRoll = _data.useRoll;
        rollSpeed = _data.rollSpeed;
        rollCooldown = _data.rollCooldown;
        minProperDistance = _data.minProperDistance;
        maxProperDistance = _data.maxProperDistance;
        attackDistanceOffset = _data.attackDistanceOffset;
    }
    public abstract void OnUpdate();
    public abstract bool UpdateOnFollowTarget();
    public abstract bool UpdateOnAttack();
    public abstract bool UpdateOnIdle();
    public abstract bool EvaluateTarget();
}
