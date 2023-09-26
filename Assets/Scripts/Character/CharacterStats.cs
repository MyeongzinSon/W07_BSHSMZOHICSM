using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] CharacterStatsData initialStats;

    //hp
    public float maxHp { get; private set; }
    //move
    public float moveSpeed { get; private set; }
    //shuriken
    public float attackPower { get; private set; }
    public float chargeSpeed { get; private set; }
    public int maxCartridgeNum { get; private set; }
    public float maxDistance { get; private set; }
    public float shurikenSpeed { get; private set; }
    public float shurikenNum { get; private set; }
    public float shurikenScale { get; private set; }
    public List<ShurikenAttribute> shurikenAttributes { get; private set; }
    

    private void Awake()
    {
        SetCharacterStats(initialStats);
    }

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
        maxCartridgeNum = info.maxCartridgeNum;
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
        maxCartridgeNum += info.maxCartridgeNum;
        maxDistance += info.maxDistance;
        shurikenSpeed += info.shurikenSpeed;
        shurikenNum += info.shurikenNum;
        shurikenScale += info.shurikenScale;

        shurikenAttributes.AddRange(info.shurikenAttributes);
    }
}
