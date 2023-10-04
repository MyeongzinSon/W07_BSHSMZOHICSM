using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] CharacterStatsData initialStats;

    //hp
    public float maxHp { get { return damageable.maxHp;} private set { damageable.maxHp = value; } }
    //move
    public float moveSpeed { get { return mover.speed;} private set { mover.speed = value; } }
    public float rollDistance { get; private set; }
    public float rollCooldown { get; private set; }
    public int maxRollNum { get; private set; }
    //shuriken
    public float attackPower { get; private set; }
    public float chargeSpeed { get; private set; }
    public float maxChargeAmount { get; private set; }
    public int maxCartridgeNum { get; private set; }
    public float maxDistance { get; private set; }
    public float shurikenSpeed { get; private set; }
    public int shurikenNum { get; private set; }
    public float shurikenScale { get; private set; }
    public List<ShurikenAttribute> shurikenAttributes { get; private set; }

    private Mover mover;
    private Damageable damageable;
    private ShurikenShooter shooter;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        damageable = GetComponent<Damageable>();
        shooter = GetComponent<ShurikenShooter>();
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
        rollDistance = info.rollDistance;
        rollCooldown = info.rollCooldown;
        maxRollNum = info.maxRollNum;
        attackPower = info.attackPower;
        chargeSpeed = info.chargeSpeed;
        maxChargeAmount = info.maxChargeAmount;
        maxCartridgeNum = info.maxCartridgeNum;
        maxDistance = info.maxDistance;
        shurikenSpeed = info.shurikenSpeed;
        shurikenNum = info.shurikenNum;
        
        CheckMinValues();

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
        moveSpeed +=  info.moveSpeed;
        rollDistance += info.rollDistance;
        rollCooldown += info.rollCooldown;
        maxRollNum += info.maxRollNum;
        attackPower +=  info.attackPower;
        chargeSpeed +=  info.chargeSpeed;
        maxCartridgeNum += info.maxCartridgeNum;
        maxChargeAmount += info.maxChargeAmount;
        maxDistance +=  info.maxDistance;
        shurikenNum += info.shurikenNum;
        shurikenScale += info.shurikenScale;
        
        //AtkSpeed ==shurikenSpeed== 투척 속도: %로 증가합니다.
        shurikenSpeed += info.shurikenSpeed*shurikenSpeed*0.01f;
        
        CheckMinValues();
        
        shooter.AddCurrentCartridge(info.maxCartridgeNum);
        shurikenAttributes.AddRange(info.shurikenAttributes);

        if (shurikenAttributes.Contains(ShurikenAttribute.Curse))
        {
            maxHp = 50;
        }
    }

    void CheckMinValues()
    {
        //최소값 처리, ex: 발사 속도가 0보다 작으면 안됨
        maxHp = Mathf.Max(10f,maxHp);
        moveSpeed =  Mathf.Max(1f, moveSpeed);
        attackPower =  Mathf.Max(1f, attackPower);
        rollDistance = Mathf.Max(1f, rollDistance);
        rollCooldown = Mathf.Max(0f, rollCooldown);
        maxRollNum = Mathf.Max(1, maxRollNum);
        chargeSpeed =  Mathf.Max(0.1f, chargeSpeed);
        maxCartridgeNum = Mathf.Max(1,maxCartridgeNum);
        maxDistance = Mathf.Max(0.01f, maxDistance);
        shurikenNum = Mathf.Max(1,shurikenNum);
        shurikenScale = Mathf.Max(0.01f,shurikenScale);
        shurikenSpeed = Mathf.Max(1f,shurikenSpeed);
    }
}
