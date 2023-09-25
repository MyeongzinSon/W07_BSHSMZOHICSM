using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    public Text[] teamNames; // 팀 이름을 표시할 UI Text 요소 배열
    public Text[] scores;    // 점수를 표시할 UI Text 요소 배열
    public GameObject[] brackets; // 대진표를 나타내는 GameObject 배열

    private List<string> teams;
    private int currentRound;

    private void Start()
    {
        InitializeTeams();
        currentRound = 0;
        UpdateBracket();
    }

    private void InitializeTeams()
    {
        // 팀 이름의 리스트를 초기화합니다. ("팀 A", "팀 B", "팀 C", ... 등)
        teams = new List<string>()
        {
            "팀 A", "팀 B", "팀 C", "팀 D",
            "팀 E", "팀 F", "팀 G", "팀 H",
            "팀 I", "팀 J", "팀 K", "팀 L",
            "팀 M", "팀 N", "팀 O", "팀 P",
            "팀 Q", "팀 R", "팀 S", "팀 T",
            "팀 U", "팀 V", "팀 W", "팀 X",
            "팀 Y", "팀 Z", "팀 AA", "팀 BB",
            "팀 CC", "팀 DD"
        };
    }

    private void UpdateBracket()
    {
        int numMatches = brackets.Length;

        for (int i = 0; i < numMatches; i++)
        {
            // 현재 라운드에 따라 팀 이름과 점수를 업데이트합니다.
            if (currentRound < teams.Count)
            {
                teamNames[i * 2].text = teams[currentRound * 2];
                teamNames[i * 2 + 1].text = teams[currentRound * 2 + 1];
                scores[i * 2].text = "0";
                scores[i * 2 + 1].text = "0";
            }
            else
            {
                // 더 이상 표시할 경기가 없습니다.
                teamNames[i * 2].text = "";
                teamNames[i * 2 + 1].text = "";
                scores[i * 2].text = "";
                scores[i * 2 + 1].text = "";
            }

            // 현재 라운드의 경기가 완료되었으면 다음 라운드로 진행합니다.
            if (i == currentRound - 1)
            {
                currentRound++;
                UpdateBracket();
            }
        }
    }

    public void RecordMatchResult(int matchIndex, int scoreTeamA, int scoreTeamB)
    {
        // 결과를 처리하는 논리를 여기에 구현합니다.
        // 간단히 UI에 점수를 표시하는 데 사용합니다.
        scores[matchIndex * 2].text = scoreTeamA.ToString();
        scores[matchIndex * 2 + 1].text = scoreTeamB.ToString();

        // 현재 라운드의 모든 경기가 완료되면 다음 라운드로 진행합니다.
        if (matchIndex == currentRound - 1)
        {
            currentRound++;
            UpdateBracket();
        }
    }
}
