using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerMove move;
    PlayerRoll roll;
    ShurikenShooter attack;
    EnemyAI ai;

    [SerializeField] Transform currentTarget;
    [SerializeField] float targetPositionOffset;
    [SerializeField] float attackDistanceOffset;

    public PlayerMove Move => move;
    public PlayerRoll Roll => roll;
    public ShurikenShooter Attack => attack;
    public Transform CurrentTarget => currentTarget;
    public float TargetPositionOffset => targetPositionOffset;
    public float AttackDistanceOffset => attackDistanceOffset;

    public Transform AttackTarget { get; private set; }

    private void Awake()
    {
        TryGetComponent(out move);
        TryGetComponent(out roll);
        TryGetComponent(out attack);
    }
    private void Start()
    {
        AttackTarget = FindObjectOfType<PlayerController>().transform;

        ai = new EnemyAIStandard();
        ai.Initialize(this);
    }

    private void Update()
    {
        ai.OnUpdate();
    }

    public void SetTarget(Transform _target)
    {
        currentTarget = _target;
    }

}