using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public List<CharacterStatsData> characterStatsDataList = new List<CharacterStatsData>();
    public List<CharacterStatsData> upgradedList = new List<CharacterStatsData>();
    public enum GameState
    {
        Tournament = 100,
        Battle,
        Upgrade,
        GameClear,
        GameOver
    }
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
        UpgradeIdx.SLOW,
        UpgradeIdx.VAMPIRE,
        UpgradeIdx.WEAKNESS,
        UpgradeIdx.FIRE
    };
    public int upgradeIdxCount = System.Enum.GetValues(typeof(UpgradeIdx)).Length;
    public bool isBattleStart = false;
    public float magneticFieldAppearCount = 30f;
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
    
    private void Start()
    {
        EnterState(GameState.Tournament);
    }

    private void Update()
    {
        if (gameState == GameState.Battle && magneticFieldAppearCount > 0f)
        {
            magneticFieldAppearCount -= Time.deltaTime;
            if (magneticFieldAppearCount <= 0f)
            {
                //자기장 생성
            }
        }
    }
    
    public void EnterState(GameState state)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.Tournament:
                break;
            case GameState.Battle:
                isBattleStart = true;
                magneticFieldAppearCount = 30f;
                break;
            case GameState.Upgrade:
                UIManager.Instance.UpgradeCanvas.SetActive(true);
                break;
            case GameState.GameClear:
                break;
            case GameState.GameOver:
                break;
        }
    }
    
    public void ExitState(GameState state)
    {
        switch (gameState)
        {
            case GameState.Tournament:
                TournamentManager.Instance.gameObject.SetActive(false);
                break;
            case GameState.Battle:
                isBattleStart = false;
                break;
            case GameState.Upgrade:
                UIManager.Instance.UpgradeCanvas.SetActive(false);
                break;
            case GameState.GameClear:
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void GoToBattle()
    {
        ExitState(GameState.Tournament);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToUpgrade()
    {
        ExitState(GameState.Battle);
        EnterState(GameState.Upgrade);
    }
    
    public void StageClear()
    {
        GameObject gameClearText = GameObject.Find("IngameCanvas").transform.Find("ClearText").gameObject;
        gameClearText.SetActive(true);
        Invoke("GoToUpgrade", 3.5f);
    }
    
    
}