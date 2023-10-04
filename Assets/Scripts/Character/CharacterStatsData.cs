using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum ShurikenAttribute { ExplodeOnHit, Boomerang, BounceOnWall, SizeUp, Guidance, KnockbackToWall, DoT, Slow, Vulnerable, Vampire, HealReduction, Curse, ComeBack }

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterStatsData", order = 1)]
public class CharacterStatsData : ScriptableObject
{
    [Header("Check for default data")]
    public bool forDefaultState;
    [Header("Character")]
    public float maxHp;
    public float moveSpeed;
    public float rollDistance;
    public float rollCooldown;
    public int maxRollNum;
    [Header("Shuriken")]
    public float attackPower;       //데미지
    public float chargeSpeed;       //충전 속도
    public float maxChargeAmount;   //최대 충전 시간
    public int maxCartridgeNum;     //탄창 수
    public float maxDistance;       //최대 사거리
    public float shurikenSpeed;     //수리검이 날아가는 속도
    public int shurikenNum;         //한번에 던지는 수리검의 수
    public float shurikenScale;     //수리검의 크기
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
            case ShurikenAttribute.Curse:
            case ShurikenAttribute.ComeBack:
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

