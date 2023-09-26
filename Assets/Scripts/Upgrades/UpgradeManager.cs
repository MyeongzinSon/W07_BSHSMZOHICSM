using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    public ShurikenDB shurikenDB;
    public Boolean[] isSelected = new Boolean[3];
    public List<int> selectedIdxes; //뜬 3개의 인덱스 리스트
    public GameObject player;
    private Transform[] UpgradeIconContainers = new Transform[3];

    void OnEnable()
    {
        selectedIdxes = GetRandomUpgradeNumbers();
        for (int i = 0; i < 3; i++)
        {
            UpgradeIconContainers[i] = transform.GetChild(i + 2).transform;
            
            UpgradeIconContainers[i].GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Prefabs/Sprites/Icons/Icon" + selectedIdxes[i]);
            
            UpgradeIconContainers[i].GetChild(4).GetComponent<TextMeshProUGUI>().text = shurikenDB.Shurikens[selectedIdxes[i]].name;
            UpgradeIconContainers[i].GetChild(5).GetComponent<TextMeshProUGUI>().text = shurikenDB.Shurikens[selectedIdxes[i]].description;

            UpgradeIconContainers[i].GetChild(6).GetComponent<TextMeshProUGUI>().text = ""; //Merit
            UpgradeIconContainers[i].GetChild(7).GetComponent<TextMeshProUGUI>().text = ""; //MeritAmount
            UpgradeIconContainers[i].GetChild(8).GetComponent<TextMeshProUGUI>().text = ""; //Demerit
            UpgradeIconContainers[i].GetChild(9).GetComponent<TextMeshProUGUI>().text = ""; //DemeritAmount
            
            if (shurikenDB.Shurikens[selectedIdxes[i]].meritSpecies != "none")
            {
                string meritText = "";
                string meritAmount =  "+ " + shurikenDB.Shurikens[selectedIdxes[i]].meritAmount.ToString();
                switch (shurikenDB.Shurikens[selectedIdxes[i]].meritSpecies)
                {
                    case "AtkDmg":
                        meritText = "공격력";
                        break;
                    case "AtkSpeed":
                        meritText = "공격속도";
                        meritAmount += "%";
                        break;
                    case "Cartridge":
                        meritText = "총 수리검";
                        meritAmount = shurikenDB.Shurikens[selectedIdxes[i]].meritAmount.ToString() + " (고정)";
                        break;
                    case "MoveSpeed":
                        meritText = "이동속도";
                        break;
                    case "Hp":
                        meritText = "체력";
                        break;
                }
                UpgradeIconContainers[i].GetChild(6).GetComponent<TextMeshProUGUI>().text = meritText;
                UpgradeIconContainers[i].GetChild(7).GetComponent<TextMeshProUGUI>().text = meritAmount;
            }
            
            if (shurikenDB.Shurikens[selectedIdxes[i]].demeritSpecies != "none")
            {
                string meritText = "";
                string meritAmount =  "- " + shurikenDB.Shurikens[selectedIdxes[i]].demeritAmount.ToString();
                switch (shurikenDB.Shurikens[selectedIdxes[i]].demeritSpecies)
                {
                    case "AtkDmg":
                        meritText = "공격력";
                        break;
                    case "AtkSpeed":
                        meritText = "공격속도";
                        meritAmount += "%";
                        break;
                    case "Cartridge":
                        meritText = "총 수리검";
                        meritAmount = shurikenDB.Shurikens[selectedIdxes[i]].meritAmount.ToString() + " (고정)";
                        break;
                    case "MoveSpeed":
                        meritText = "이동속도";
                        break;
                    case "Hp":
                        meritText = "체력";
                        break;
                }
                UpgradeIconContainers[i].GetChild(8).GetComponent<TextMeshProUGUI>().text = meritText;
                UpgradeIconContainers[i].GetChild(9).GetComponent<TextMeshProUGUI>().text = meritAmount;
            }
        }
    }
    private List<int> GetRandomUpgradeNumbers() //3개의 랜덤한 숫자를 뽑음
    {
        List<int> selectedNumbers = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomNum = getCanUpgradeRandomNumber();
            while(randomNum == -1 || CheckRedundancy(selectedNumbers, randomNum))
            {
                randomNum = getCanUpgradeRandomNumber();
            }
            selectedNumbers.Add(randomNum);
        }
        
        return selectedNumbers;
    }

    private int getCanUpgradeRandomNumber() //ShowOnlyOnce를 고려해서 Int 변수 하나 뽑음
    {
        int randomInt = Random.Range(0, GameManager.Instance.upgradeIdxCount + 1);
        if (GameManager.Instance.canUpgradeIdxList.Contains((GameManager.UpgradeIdx) randomInt))
        {
            return randomInt;
        }
        else
        {
            return -1;
        }
    }
    
    public bool CheckRedundancy(List<int> selectedNumbers, int idx) //중복체크
    {
        if (selectedNumbers.Contains(idx))
        {
            return true;
        }
        return false;
    }

    public void ResetSelect()
    {
        for (int i = 0; i < 3; i++)
        {
            UpgradeIconContainers[i].GetChild(0).gameObject.SetActive(false);
            isSelected[i] = false;
        }
    }

    public void SelectConfirmHandler()
    {
        int selectedIdx = -1;
        for (int i = 0; i < selectedIdxes.Count; i++)
        {
            if (isSelected[i])
            {
                selectedIdx = selectedIdxes[i];
                if (shurikenDB.Shurikens[selectedIdxes[i]].showOnlyOnce > 0) // 한번만 나와야 하는 경우
                {
                    GameManager.Instance.canUpgradeIdxList.Remove((GameManager.UpgradeIdx) selectedIdxes[i]);
                }
            }
        }
        ResetSelect();
        
        //업그레이드 실제 연동
        if (selectedIdx != -1)
        {
            CharacterStatsData data = GameManager.Instance.characterStatsDataList[selectedIdx];
            GameManager.Instance.upgradedList.Add(data);
            GameManager.Instance.upgradedListInt.Add(selectedIdx);
        }
        
        GameManager.Instance.ExitState(GameManager.GameState.Upgrade);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        GameManager.Instance.EnterState(GameManager.GameState.Tournament);
        TournamentManager.Instance.gameObject.SetActive(true);
        TournamentManager.Instance.Init();
        gameObject.SetActive(false);
        
    }
}
