using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum ShurikenAttribute { ExplodeOnHit, Boomerang, BounceOnWall, SizeUp, Guidance, KnockbackToWall, DoT, Slow, Vulnerable, Vampire, HealReduction }

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterStatsData", order = 1)]
public class CharacterStatsData : ScriptableObject
{
    [Header("Check for default data")]
    public bool forDefaultState;
    [Header("Character")]
    public float maxHp;
    public float moveSpeed;
    [Header("Shuriken")]
    public float attackPower;
    public float chargeSpeed;
    public int maxCatridgeNum;
    public float maxDistance;
    public float shurikenSpeed;
    public int shurikenNum;
    public List<ShurikenAttribute> shurikenAttributes;
}
