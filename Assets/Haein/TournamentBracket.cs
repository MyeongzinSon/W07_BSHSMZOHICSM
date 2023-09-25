using UnityEngine;

public class TournamentBracket : MonoBehaviour
{
    public Transform[] teamSlots; // 팀 이름을 표시할 UI Text 요소를 가지고 있는 GameObject 배열
    public LineRenderer lineRenderer; // 선을 그리기 위한 LineRenderer 컴포넌트

    void Start()
    {
        // 팀 이름과 선을 초기화합니다.
        InitializeTeams();
        InitializeLines();
    }

    void InitializeTeams()
    {
        // 각 슬롯에 팀 이름을 할당합니다. 본인의 데이터 소스로 대체해야 합니다.
        for (int i = 0; i < teamSlots.Length; i++)
        {
            Transform teamSlot = teamSlots[i];
            TextMesh textMesh = teamSlot.GetComponentInChildren<TextMesh>();
            textMesh.text = "Team " + (i + 1);
        }
    }

    void InitializeLines()
    {
        // LineRenderer의 점을 설정하여 선을 그립니다.
        lineRenderer.positionCount = 4;
        
        // 각 라운드의 경기 슬롯에 대한 위치를 계산합니다.
        for (int i = 0; i < teamSlots.Length; i += 2)
        {
            Vector3 teamSlot1Position = teamSlots[i].position;
            Vector3 teamSlot2Position = teamSlots[i + 1].position;

            // 선을 그릴 점의 위치를 설정합니다.
            lineRenderer.SetPosition(i, teamSlot1Position);
            lineRenderer.SetPosition(i + 1, new Vector3(teamSlot1Position.x, (teamSlot1Position.y + teamSlot2Position.y) / 2f, teamSlot1Position.z));
            lineRenderer.SetPosition(i + 2, new Vector3(teamSlot1Position.x, (teamSlot1Position.y + teamSlot2Position.y) / 2f, teamSlot1Position.z));
            lineRenderer.SetPosition(i + 3, teamSlot2Position);
        }
    }
}