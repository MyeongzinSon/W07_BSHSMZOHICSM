using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAIData", order = 2)]
public class EnemyAIData : ScriptableObject
{
    [Header("AI")]
    public bool useRoll;
    public float rollSpeed;
    public float rollCooldown;
    public float minProperDistance;
    public float maxProperDistance;
    public float attackDistanceOffset;
}