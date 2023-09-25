using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum UpgradeIdx
    {
        LARGE = 0,
        BACKPACK,
        BOUNCE,
        RELOAD,
        BOOMERANG,
        EXPLOSION,
        THROWNUM,
        WALLSTUN,
        KNOCKBACKTIME,
        ENFORCE,
        POISON,
        SLOW,
        VAMPIRE,
        WEAKNESS,
        FIRE
    }
    
    public List<UpgradeIdx> canUpgradeIdxList = new List<UpgradeIdx>
    {
        UpgradeIdx.LARGE,
        UpgradeIdx.BACKPACK,
        UpgradeIdx.BOUNCE,
        UpgradeIdx.RELOAD,
        UpgradeIdx.BOOMERANG,
        UpgradeIdx.EXPLOSION,
        UpgradeIdx.THROWNUM,
        UpgradeIdx.WALLSTUN,
        UpgradeIdx.KNOCKBACKTIME,
        UpgradeIdx.ENFORCE,
        UpgradeIdx.POISON,
        UpgradeIdx.SLOW,
        UpgradeIdx.VAMPIRE,
        UpgradeIdx.WEAKNESS,
        UpgradeIdx.FIRE
    };
    public int upgradeIdxCount = System.Enum.GetValues(typeof(UpgradeIdx)).Length;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }
    
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}