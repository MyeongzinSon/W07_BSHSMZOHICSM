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
    public int maxCartridgeNum;
    public float maxDistance;
    public float shurikenSpeed;
    public int shurikenNum;
    public List<ShurikenAttribute> shurikenAttributes;
}

public static class ShurikenAttributeExtensions
{
    public static int GetActivateNumber(this ShurikenAttribute attribute)
    {
        switch (attribute)
        {
            case ShurikenAttribute.ExplodeOnHit:
            case ShurikenAttribute.Boomerang:
            case ShurikenAttribute.BounceOnWall:
            case ShurikenAttribute.SizeUp:
            case ShurikenAttribute.Guidance:
            case ShurikenAttribute.KnockbackToWall:
                return 1;
            case ShurikenAttribute.DoT:
            case ShurikenAttribute.Slow:
            case ShurikenAttribute.Vulnerable:
            case ShurikenAttribute.Vampire:
            case ShurikenAttribute.HealReduction:
                return 3;
            default:
                Debug.LogError($"주어진 ShurikenAttribute가 가능한 값이 아님! : {attribute}");
                return -1;
        }

    }
}

