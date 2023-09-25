using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    public int playerIdx = 0;
    private int objectCount = 16;
    public float movementSpeed = 2f; 
    private float waitTime = 0.3f;
    private int currentRound = 0;
    private List<Transform> objectsToRemove = new List<Transform>(); // 삭제할 오브젝트를 저장하는 리스트
    private List<Transform> remainingTourmantObjects = new List<Transform>();
    GameObject PortraitIconContainerPrefab;
    
    private List<string> NinjaSpecies = new List<string>
    {
        "",
        "공격적",
        "수비적",
        "공격적",
        "수비적",
        "공격적",
        "수비적",
        "공격적",
        "수비적",
        "공격적",
        "수비적",
        "수비적",
        "공격적",
        "수비적",
        "공격적",
        "공격적"
    };
    
    private List<string> NinjaNames = new List<string>
    {
        "Player",
        "Shadow",
        "Strike",
        "Blade",
        "Storm",
        "Raven",
        "Fox",
        "Zen",
        "Ash",
        "Echo",
        "Fury",
        "Phoenix",
        "Thorn",
        "Swift",
        "Slate",
        "Onyx"
    };
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
        StartCoroutine(SimulateTournament());
    }

    private IEnumerator SimulateTournament()
    {
        for (int i = 0; i < objectCount; i += 2)
        {
            Transform winner = SimulateMatch(remainingTourmantObjects[i], remainingTourmantObjects[i + 1]);
            Transform loser = remainingTourmantObjects[i + 1];
            if (winner == remainingTourmantObjects[i + 1]) loser = remainingTourmantObjects[i];
            
            if (i == 0) //항상 플레이어가 이기게 함 (플레이어 idx = 0)
            {
                winner = remainingTourmantObjects[i];
                loser = remainingTourmantObjects[i + 1];
            }
            
            yield return new WaitForSeconds(waitTime);
            winner.GetChild(0).GetComponent<Image>().color = Color.green;
            loser.GetChild(0).GetComponent<Image>().color = Color.red;
            objectsToRemove.Add(loser); // 삭제할 오브젝트를 리스트에 추가
            yield return new WaitForSeconds(waitTime);
        }

        // 모든 매치가 끝나면 한 번에 삭제
        foreach (Transform objToRemove in objectsToRemove)
        {
            remainingTourmantObjects.Remove(objToRemove);
            Destroy(objToRemove.gameObject);
        }
        objectsToRemove.Clear(); // 리스트 비우기

        GoToNextRound();
    }

    private Transform SimulateMatch(Transform obj1, Transform obj2)
    {
        // 이긴 오브젝트를 무작위로 선택
        return Random.Range(0, 2) == 0 ? obj1 : obj2;
    }
    

    private void GoToNextRound()
    {
        foreach (Transform obj in remainingTourmantObjects)
        {
            Image image = obj.GetChild(0).GetComponent<Image>();
            Color lightGray = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            image.color = lightGray;
        }

        // 화면 왼쪽에 있는 오브젝트들을 오른쪽으로, 그렇지 않은 오브젝트들은 왼쪽으로 이동
        float moveDistance = 200f; // 이동 거리 조절
        float screenWidth = Screen.width;
        foreach (Transform obj in remainingTourmantObjects)
        {
            Vector3 targetPosition = obj.position;

            if (obj.position.x < screenWidth / 2)
            {
                // 화면 왼쪽에 있는 오브젝트를 오른쪽으로 이동
                targetPosition.x += moveDistance;
            }
            else
            {
                // 화면 오른쪽에 있는 오브젝트를 왼쪽으로 이동
                targetPosition.x -= moveDistance;
            }

            StartCoroutine(MoveTransform(obj, targetPosition, 0.5f));
        }

        objectCount /= 2;
        currentRound++;
    }

    private IEnumerator MoveTransform(Transform targetTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = targetTransform.position;

        while (elapsedTime < duration)
        {
            targetTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.position = targetPosition;
    }
    
    private void CreateIconContainers()
    {
        // 첫 번째 8개의 아이콘 컨테이너 생성
        for (int i = 0; i < 8; i++)
        {
            CreateIconContainer(new Vector3(105f, 990f - i * 130f, 0f), i);
        }

        // 나머지 8개의 아이콘 컨테이너 생성
        for (int i = 0; i < 8; i++)
        {
            CreateIconContainer(new Vector3(1635f, 990f - i * 130f, 0f), i + 8);
        }
    }
    
    private void CreateIconContainer(Vector3 position, int idx)
    {
        GameObject iconContainer = Instantiate(PortraitIconContainerPrefab, position, Quaternion.identity);
        iconContainer.transform.SetParent(transform);
        remainingTourmantObjects.Add(iconContainer.transform); 
        iconContainer.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = NinjaSpecies[idx];
        iconContainer.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = NinjaNames[idx];
    }
}
