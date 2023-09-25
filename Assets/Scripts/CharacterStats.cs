using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Character")]
    public float maxHp;
    public float currentHp;
    public float moveSpeed;
    [Header("Shuriken")]
    public float attackPower;
    public float chargeSpeed;
    public int maxCatridgeNum;
    public int currentCatridgeNum;
    public float maxDistance;
    public float shurikenSpeed;
    public float shurikenNum;
    public List<ShurikenAttribute> shurikenAttributes;

    public void SetCharacterStats(CharacterStatsData info)
    { 
        if (!info.forDefaultState)
        {
            Debug.LogError($"CharacterStats : 설정하려는 Data 파일이 기본용 스탯이 아님! ({info.GetInstanceID()})");
            return;
        }

        maxHp = info.maxHp;
        moveSpeed = info.moveSpeed;
        attackPower = info.attackPower;
        chargeSpeed = info.chargeSpeed;
        maxCatridgeNum = info.maxCatridgeNum;
        maxDistance = info.maxDistance;
        shurikenSpeed = info.shurikenSpeed;
        shurikenNum = info.shurikenNum;

        shurikenAttributes = new();
        shurikenAttributes.AddRange(info.shurikenAttributes);
    }

    public void AddCharacterStats(CharacterStatsData info)
    {
        if (info.forDefaultState)
        {
            Debug.LogError($"CharacterStats : 설정하려는 Data 파일이 추가용 스탯이 아님! ({info.GetInstanceID()})");
            return;
        }

        maxHp += info.maxHp;
        moveSpeed += info.moveSpeed;
        attackPower += info.attackPower;
        chargeSpeed += info.chargeSpeed;
        maxCatridgeNum += info.maxCatridgeNum;
        maxDistance += info.maxDistance;
        shurikenSpeed += info.shurikenSpeed;
        shurikenNum += info.shurikenNum;

        shurikenAttributes.AddRange(info.shurikenAttributes);
    }
}
