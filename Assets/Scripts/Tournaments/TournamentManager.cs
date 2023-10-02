using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Random = System.Random;

public class TournamentManager : MonoBehaviour
{
    private static TournamentManager instance;
    public static TournamentManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TournamentManager>();
            }

            return instance;
        }
    }
    public int playerIdx = 0;
    [SerializeField] private int objectCount = 64;
    public float movementSpeed = 2f; 
    private float waitTime = 0.35f;
    private int currentRound = 0;
    private List<Transform> objectsToRemove = new List<Transform>(); // 삭제할 오브젝트 저장
    private List<Transform> remainingTourmantObjects = new List<Transform>();
    GameObject PortraitIconContainerPrefab;
    
    private List<string> NinjaSpecies = new List<string>();

    private void GenerateNinjaSpecies()
    {
        int totalSpecies = 64;
        NinjaSpecies.Add("주인공");
        for (int i = 1; i < totalSpecies; i++)
        {
            if (i == 1) NinjaSpecies.Add("멍청한");
            else if (i == 3) NinjaSpecies.Add("폭발적인");
            else if (i == 6) NinjaSpecies.Add("묵직한");
            else if (i == 12) NinjaSpecies.Add("충동적인");
            else if (i == 24) NinjaSpecies.Add("날렵한");
            else if (i == 48) NinjaSpecies.Add("숙련된");
            else
            {
                int rndIdx = UnityEngine.Random.Range(0, 3);
                if (rndIdx == 0) NinjaSpecies.Add("신중한");
                else if (rndIdx == 1) NinjaSpecies.Add("재빠른");
                else NinjaSpecies.Add("공격적인");
            }
        }
    }
    
    private List<string> NinjaNames = new List<string>
    {
        "Player",
        "Shad",
        "Strike",
        "Blade",
        "Storm",
        "Raven",
        "Fox",
        "Zen",
        "Ash",
        "Echo",
        "Fury",
        "Gale",
        "Thorn",
        "Swift",
        "Slate",
        "Onyx",
        "Dragon",
        "Tiger",
        "Lotus",
        "Pawn",
        "Abdo",
        "Wraith",
        "Yin",
        "Petal",
        "Silent",
        "Steel",
        "Mystic",
        "Nova",
        "Jade",
        "Wolf",
        "Quake",
        "Iron",
        "Viper",
        "Saki",
        "Asin",
        "Zephyr",
        "Ghost",
        "Sable",
        "Talon",
        "Blaze",
        "9z",
        "Shuri",
        "Sword",
        "Rogue",
        "Sapp",
        "Kaze",
        "Dawn",
        "Flame",
        "Jun",
        "Jinx",
        "Cov",
        "Stormy",
        "Abyss",
        "Koi",
        "Dagger",
        "Chaos",
        "Dusk",
        "Grim",
        "Aurora",
        "Poison",
        "Rune",
        "Astral",
        "Razor",
        "Kavo"
    };

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        GenerateNinjaSpecies();
        PortraitIconContainerPrefab = Resources.Load<GameObject>("Prefabs/Tournaments/PortraitIconContainer");
        CreateIconContainers();
        InitDrawLines();
    }
    
    public void Init()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        StartTournament();
    }

    public void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Init();
        }
        #endif
    }
    
    private void StartTournament()
    {
        InitDrawLines();
        StartCoroutine(SimulateTournament());
    }

    private IEnumerator SimulateTournament()
    {
        yield return new WaitForSeconds(1.0f);
        int matchesPerRound = objectCount;
        int repeatNum = (int)objectCount / 8;
        repeatNum = Mathf.Max(repeatNum, 2);
        
        for (int i = 0; i < matchesPerRound; i += repeatNum)
        {
            List<Transform> winners = new List<Transform>();
            List<Transform> losers = new List<Transform>();

            for (int j = i; j < i + repeatNum; j += 2)
            {
                Transform winner = SimulateMatch(remainingTourmantObjects[j], remainingTourmantObjects[j + 1], j);
                Transform loser = remainingTourmantObjects[j + 1];
                if (winner == remainingTourmantObjects[j + 1]) loser = remainingTourmantObjects[j];

                if (j == 0) // 주인공 항상 이기게 함
                {
                    winner = remainingTourmantObjects[j];
                    loser = remainingTourmantObjects[j + 1];
                }
                

                winners.Add(winner);
                losers.Add(loser);
            }
            
            yield return new WaitForSeconds(waitTime);

            for (int j = 0; j < (int)repeatNum / 2; j++)
            {
                if (j == 0 && i == 0)
                {
                    winners[j].GetChild(0).GetComponent<Image>().color = Color.yellow;
                    losers[j].GetChild(0).GetComponent<Image>().color = Color.red;
                }
                else
                {
                    winners[j].GetChild(0).GetComponent<Image>().color = Color.green;
                    losers[j].GetChild(0).GetComponent<Image>().color = Color.red;
                }
                objectsToRemove.Add(losers[j]);
            }

            yield return new WaitForSeconds(waitTime);
        }

        foreach (Transform objToRemove in objectsToRemove)
        {
            remainingTourmantObjects.Remove(objToRemove);
            StartCoroutine(MoveOffScreen(objToRemove, 1.0f));
        }
        objectsToRemove.Clear();
        transform.GetChild(2).gameObject.SetActive(true);

        GoToNextRound();
    }

    private void InitDrawLines()
    {
        if (remainingTourmantObjects.Count >= 2)
        {
            for (int i = 0; i < remainingTourmantObjects.Count; i += 2)
            {
                DrawLinesBetweenMatches(remainingTourmantObjects[i].transform, remainingTourmantObjects[i + 1].transform);
            }
        }
    }
    
    private void DrawLinesBetweenMatches(Transform a, Transform b)
    {
        GameObject line = Resources.Load<GameObject>("Prefabs/Tournaments/LineImage");
        GameObject parent = GameObject.Find("Lines");
        GameObject line1 = Instantiate(line, parent.transform);
        line1.GetComponent<LineImage>().pointA = a.position;
        line1.GetComponent<LineImage>().pointB = b.position;
    }
    
    private void DeleteAllLines()
    {
        GameObject parent = GameObject.Find("Lines");
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    
    private IEnumerator MoveOffScreen(Transform targetTransform, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = targetTransform.position;
        float screenWidth = Screen.width;
        float targetX;
        if (initialPosition.x < screenWidth / 2)
        {
            targetX = -Screen.width * 1.5f;
        }
        else
        {
            targetX = Screen.width * 1.5f;
        }

        while (elapsedTime < duration)
        {
            float newX = Mathf.Lerp(initialPosition.x, targetX, elapsedTime / duration);
            targetTransform.position = new Vector3(newX, initialPosition.y, initialPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(targetTransform.gameObject);

        if (objectCount == 1)
        {
            GameManager.Instance.clearPanel.SetActive(true);
        }
    }
    
    private Transform SimulateMatch(Transform obj1, Transform obj2, int idx)
    {
        if (idx % 6 == 0) return obj1;
        else if (idx % 6 == 1) return obj2;
        else if (idx % 6 == 2) return obj2;
        else if (idx % 6 == 3) return obj1;
        else if (idx % 6 == 4) return obj2;
        else return obj1;
    }
    

    private void GoToNextRound()
    {
        for (int i = 1; i < remainingTourmantObjects.Count; i++)
        {
            Transform obj = remainingTourmantObjects[i];
            Image image = obj.GetChild(0).GetComponent<Image>();
            Color lightGray = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            image.color = lightGray;
        }
        float moveDistance = 200f;
        float screenWidth = Screen.width;
        foreach (Transform obj in remainingTourmantObjects)
        {
            Vector3 targetPosition = obj.position;

            if (obj.position.x < screenWidth / 2)
            {
                targetPosition.x += moveDistance;
            }
            else
            {
                targetPosition.x -= moveDistance;
            }
            
        }

        objectCount /= 2;
        currentRound++;
        
        DeleteAllLines();
        InitDrawLines();
    }
    
    private void CreateIconContainers()
    {
        Debug.Log($"CreateIconContainers");
        int columns = 8; // 열의 수
        int totalContainers = columns * 8; // 총 컨테이너 수
        float xOffset = -840f;
        float yOffset = 470f;

        for (int i = 0; i < totalContainers; i++)
        {
            int column = i % columns;
            int row = i / columns;

            float x = xOffset + row * 220f + (row >= 4 ? 140f : 0f);
            float y = yOffset - column * 140f; // 2쌍씩 붙이므로 260 사용

            if (column % 2 == 1)
            {
                y += 40f; // 다른 쌍과의 간격 조정
            }

            CreateIconContainer(new Vector3(x, y, 0f), i);
        }
    }
    
    private void CreateIconContainer(Vector3 position, int idx)
    {
        GameObject parent = GameObject.Find("TournamentCanvas");
        GameObject iconContainer = Instantiate(PortraitIconContainerPrefab, position, Quaternion.identity);
        Debug.Log(parent.name);
        Debug.Log(iconContainer.name);
        iconContainer.transform.SetParent(parent.transform, false);
        remainingTourmantObjects.Add(iconContainer.transform);
        int rndIdx = UnityEngine.Random.Range(1, 7);
        if (idx == 0)
        {
            iconContainer.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        iconContainer.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/Sprites/TournamentPortraits/Ninja" + rndIdx);
        iconContainer.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = NinjaSpecies[idx];
        iconContainer.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = NinjaNames[idx];
    }
}
