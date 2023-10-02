using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public List<CharacterStatsData> characterStatsDataList = new List<CharacterStatsData>();
    public List<CharacterStatsData> upgradedList = new List<CharacterStatsData>();
    public List<int> upgradedListInt = new List<int>();
    public GameObject clearPanel;
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
        HPUP = 0,
        DAMAGEUP,
        BACKPACK,
        DASHNUM,
        RANGEUP,
        RELOAD,
        BOUNCE,
        BOOMERANG,
        EXPLOSION,
        THROWNUM,
        GUIDANCE,
        SPIDERWEB,
        CURSE
    }

    public int specialStartIdx = 6;
    
    public List<UpgradeIdx> canUpgradeIdxList = new List<UpgradeIdx>
    {
        UpgradeIdx.HPUP,
        UpgradeIdx.DAMAGEUP,
        UpgradeIdx.BACKPACK,
        UpgradeIdx.DASHNUM,
        UpgradeIdx.RANGEUP,
        UpgradeIdx.RELOAD,
        UpgradeIdx.BOUNCE,
        UpgradeIdx.BOOMERANG,
        UpgradeIdx.EXPLOSION,
        UpgradeIdx.THROWNUM,
        UpgradeIdx.GUIDANCE,
        UpgradeIdx.SPIDERWEB,
        UpgradeIdx.CURSE
    };
    
    public int upgradeIdxCount = System.Enum.GetValues(typeof(UpgradeIdx)).Length;
    public bool isBattleStart = false;
    public float magneticFieldAppearCount = 30f;
    private static GameManager _instance;
    
    //몇번째 스테이지인가
    public int stageCount = 1;
    public int maxStage = 6;
    
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
    
    public void StageClear(int winPlayerNum)
    {
        if (VersusManager.Instance.victoryCountPlayer1 == 4 || VersusManager.Instance.victoryCountPlayer2 == 4)
        {
            GameObject gameEndPanel = GameObject.Find("GameEndPanel").gameObject;
            if (VersusManager.Instance.victoryCountPlayer1 == 4)
            {
                gameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<color=#FF0000>플레이어1</color> 최종 승리!";
            }
            else
            {
                gameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<color=#0000FF>플레이어2</color> 최종 승리!";
            }
                
            string descriptionText = "";
            switch (stageCount)
            {
                case 3:
                    descriptionText = "압도적인 경기력이었습니다! \n실력 차이가 어마어마하네요.";
                    break;
                case 6:
                    descriptionText = "정말 치열한 접전이었습니다! \n두 플레이어의 희비가 교차하는 순간입니다.";
                    break;
                default:
                    descriptionText = "좋은 경기였습니다! \n두 플레이어 모두 대단합니다.";
                    break;
            }
            gameEndPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = descriptionText;
            
            gameEndPanel.SetActive(true);
            
            return;
        }
        GameObject gameClearText = GameObject.Find("IngameCanvas").transform.Find("ClearText").gameObject;

        if (winPlayerNum == 1)
        {
            gameClearText.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#FF0000>플레이어1</color> 승리!";
        }
        else
        {
            gameClearText.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#0000FF>플레이어2</color> 승리!";
        }
        
        gameClearText.SetActive(true);
        Invoke("GoToUpgrade", 3.5f);

        stageCount += 1;
        
    }
    
    
}