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
    private int objectCount = 64;
    public float movementSpeed = 2f; 
    private float waitTime = 0.5f;
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
            if (UnityEngine.Random.Range(0, 2) == 0) NinjaSpecies.Add("공격적");
            else NinjaSpecies.Add("수비적");
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
        "Cobra",
        "Quake",
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
        "Koi",
        "Jinx",
        "Cov",
        "Stormy",
        "Abyss",
        "PZ",
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
        }

        DontDestroyOnLoad(gameObject);
        GenerateNinjaSpecies();
    }
    
    private void Start()
    {
        PortraitIconContainerPrefab = Resources.Load<GameObject>("Prefabs/Tournaments/PortraitIconContainer");
        CreateIconContainers();
        StartTournament();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartTournament();
        }
    }

    private void StartTournament()
    {
        InitDrawLines();
        StartCoroutine(SimulateTournament());
    }

    private IEnumerator SimulateTournament()
    {
        int matchesPerRound = objectCount;
        int repeatNum = (int)objectCount / 8;
        repeatNum = Mathf.Max(repeatNum, 2);
        
        for (int i = 0; i < matchesPerRound; i += repeatNum)
        {
            List<Transform> winners = new List<Transform>();
            List<Transform> losers = new List<Transform>();

            for (int j = i; j < i + repeatNum; j += 2)
            {
                    Transform winner = SimulateMatch(remainingTourmantObjects[j], remainingTourmantObjects[j + 1]);
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
                winners[j].GetChild(0).GetComponent<Image>().color = Color.green;
                losers[j].GetChild(0).GetComponent<Image>().color = Color.red;
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
    }
    
    private Transform SimulateMatch(Transform obj1, Transform obj2)
    {
        return UnityEngine.Random.Range(0, 2) == 0 ? obj1 : obj2;
    }
    

    private void GoToNextRound()
    {
        foreach (Transform obj in remainingTourmantObjects)
        {
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
        iconContainer.transform.SetParent(parent.transform, false);
        remainingTourmantObjects.Add(iconContainer.transform); 
        iconContainer.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = NinjaSpecies[idx];
        iconContainer.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = NinjaNames[idx];
    }
}
