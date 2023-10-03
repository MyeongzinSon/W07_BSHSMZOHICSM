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
    public Boolean[] canSelect = new Boolean[3];
    public int remainingSelectCount = 2;
    public List<int> selectedIdxes; //뜬 3개의 인덱스 리스트
    public GameObject player;
    private Transform[] UpgradeIconContainers = new Transform[3];

    void OnEnable()
    {
        for (int i = 0; i < 3; i++)
        {
            canSelect[i] = true;
        }
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
                    case "ChargeSpeed":
                        meritText = "장전속도";
                        break;
                    case "Range":
                        meritText = "최대사거리";
                        break;
                    case "DashNum":
                        meritText = "대시 횟수";
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
                    case "ChargeSpeed":
                        meritText = "장전속도";
                        break;
                    case "Range":
                        meritText = "최대사거리";
                        break;
                    case "DashNum":
                        meritText = "대시 횟수";
                        break;
                }
                UpgradeIconContainers[i].GetChild(8).GetComponent<TextMeshProUGUI>().text = meritText;
                UpgradeIconContainers[i].GetChild(9).GetComponent<TextMeshProUGUI>().text = meritAmount;
            }
        }
    }
    private List<int> GetRandomUpgradeNumbers() //3개의 랜덤한 숫자를 뽑음
    {
        bool isSpecial = false;
        if (GameManager.Instance.stageCount == 1 || GameManager.Instance.stageCount == 4) //특일일특일일
        {
            isSpecial = true;
        }
        List<int> selectedNumbers = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomNum = getCanUpgradeRandomNumber(isSpecial);
            while(randomNum == -1 || CheckRedundancy(selectedNumbers, randomNum))
            {
                randomNum = getCanUpgradeRandomNumber(isSpecial);
            }
            selectedNumbers.Add(randomNum);
        }
        
        return selectedNumbers;
    }

    private int getCanUpgradeRandomNumber(bool isSpecial = false) //ShowOnlyOnce를 고려해서 Int 변수 하나 뽑음
    {
        int randomInt;
        if (isSpecial) //특수
        {
            randomInt = Random.Range(GameManager.Instance.specialStartIdx, GameManager.Instance.upgradeIdxCount + 1);
        }
        else //일반
        {
            randomInt = Random.Range(0, GameManager.Instance.specialStartIdx);
        }

        if (remainingSelectCount == 2)
        {
            if (GameManager.Instance.canUpgradeIdxListPlayer1.Contains((GameManager.UpgradeIdx) randomInt))
            {
                return randomInt;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (GameManager.Instance.canUpgradeIdxListPlayer2.Contains((GameManager.UpgradeIdx) randomInt))
            {
                return randomInt;
            }
            else
            {
                return -1;
            }
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
        transform.GetChild(5).gameObject.SetActive(false);
    }
    
    public void ResetSelectAll()
    {
        for (int i = 0; i < 3; i++)
        {
            canSelect[i] = true;
            remainingSelectCount = 2;
            Color originalColor = new Color(0.718f, 0.576f, 0.360f);
            UpgradeIconContainers[i].GetChild(1).GetComponent<Image>().color = originalColor;
        }

        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<color=#FF0000>플레이어1</color>업그레이드";
    }

    public void SelectConfirmHandler()
    {
        int selectedIdx = -1;
        int curIdx = -1;
        for (int i = 0; i < selectedIdxes.Count; i++)
        {
            if (isSelected[i])
            {
                selectedIdx = selectedIdxes[i];
                curIdx = i;
            }
        }

        if (selectedIdx == -1)
        {
            return;
        }
        else
        {
            if (remainingSelectCount == 2)
            {
                CharacterStatsData data = GameManager.Instance.characterStatsDataList[selectedIdx];
                GameManager.Instance.upgradedListPlayer1.Add(data);
                GameManager.Instance.upgradedListIntPlayer1.Add(selectedIdx);
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<color=#0000FF>플레이어2</color>업그레이드";
            }
            else
            {
                CharacterStatsData data = GameManager.Instance.characterStatsDataList[selectedIdx];
                GameManager.Instance.upgradedListPlayer2.Add(data);
                GameManager.Instance.upgradedListIntPlayer2.Add(selectedIdx);
            }

            if (shurikenDB.Shurikens[selectedIdxes[curIdx]].showOnlyOnce > 0) // 한번만 나와야 하는 경우
            {
                GameManager.Instance.canUpgradeIdxListPlayer1.Remove((GameManager.UpgradeIdx)selectedIdxes[curIdx]);
            }

            canSelect[curIdx] = false;
            UpgradeIconContainers[curIdx].GetChild(1).GetComponent<Image>().color = new Color(0.341f, 0.278f, 0.169f);
            
            
        }

        remainingSelectCount--;
        ResetSelect();

        if (remainingSelectCount <= 0)
        {
            ResetSelectAll();
            GameManager.Instance.ExitState(GameManager.GameState.Upgrade);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            GameManager.Instance.EnterState(GameManager.GameState.Tournament);
            //TournamentManager.Instance.gameObject.SetActive(true);
            //TournamentManager.Instance.Init();
            gameObject.SetActive(false);

        }
        
    }
}
