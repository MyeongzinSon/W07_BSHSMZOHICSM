using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    public int playerIdx = 0;
    public Transform[] tournamentObjects; // 16개의 오브젝트 배열
    public List<Transform> remainingTourmantObjects; // 남아있는 오브젝트 배열
    public float movementSpeed = 2f; // 오브젝트 이동 속도
    private float waitTime = 0.3f; // 이긴 후 대기 시간
    public int objectCount;
    private int currentRound = 0;
    private List<Transform> objectsToRemove = new List<Transform>(); // 삭제할 오브젝트를 저장하는 리스트

    private void Start()
    {
        objectCount = tournamentObjects.Length;
        remainingTourmantObjects = new List<Transform>(tournamentObjects);
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
}
