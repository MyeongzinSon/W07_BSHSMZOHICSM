using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    NavMeshMover move;
    PlayerRoll roll;
    ShurikenShooter attack;
    EnemyAI ai;

    [SerializeField] Transform currentTarget;
    [Header("AI Data")]
    [SerializeField] EnemyAIData aiData;
    private CharacterStats stats;
    public List<CharacterStatsData> itemToAdd = new();
    [Header("Pickable Target")]
    [SerializeField] float targetPositionOffset;
    [Header("Shuriken")]
    [SerializeField] float attackDistanceOffset;
    [Header("Distance Maintenance")]
    [SerializeField] float minProperDistance;
    [SerializeField] float maxProperDistance;

    public NavMeshMover Move => move;
    public PlayerRoll Roll => roll;
    public ShurikenShooter Attack => attack;
    public Transform CurrentTarget => currentTarget;
    public float TargetPositionOffset => targetPositionOffset;
    //public float AttackDistanceOffset => attackDistanceOffset;
    //public float MinProperDistance => minProperDistance;
    //public float MaxProperDistance => maxProperDistance;

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
        //적 기본 스탯 추가
        ai.SetPersonalVariables(aiData);
        
        //적 아이템 추가
        stats = GetComponent<CharacterStats>();
        foreach (var VARIABLE in itemToAdd)
        {
            stats.AddCharacterStats(VARIABLE);
        }
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
