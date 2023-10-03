using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatridgeUIManager : MonoBehaviour
{
    private int catridgeNum;
    private float initialWidth;
    private float totalWidth;
    private float offset = 40f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        initialWidth = transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x;
        catridgeNum = player.GetComponent<CharacterStats>().maxCartridgeNum;
        DrawCatridgeUIBasedOnMaxCartridgeNum();
    }

    void DrawCatridgeUIBasedOnMaxCartridgeNum()
    {
        totalWidth = (initialWidth) * catridgeNum - (catridgeNum - 1) * offset;
        float xx = totalWidth;
        float yy = transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(xx + 20, yy + 20);
        transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(xx, yy);
        int playerNum = player.CompareTag("Player1") ? 1 : 2;
        ChangeCurrentKunai(playerNum);
    }

    public void ChangeCurrentKunai(int playerNum)
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        if (playerNum == 1)
        {
            GameObject kunaiPrefab = (GameObject)Resources.Load("Prefabs/UI/KunaiImagePlayer1");
            int remainNum = player.GetComponent<ShurikenShooter>().GetcurrentCartridge();
            for (int i = 0; i < remainNum; i++)
            {
                GameObject kunai = Instantiate(kunaiPrefab, transform);
                kunai.transform.SetParent(transform);
                kunai.transform.GetComponent<RectTransform>().position = new Vector3(transform.position.x + (offset * 2) * i, transform.position.y, transform.position.z);
            }
        }
        else
        {
            GameObject kunaiPrefab = (GameObject)Resources.Load("Prefabs/UI/KunaiImagePlayer2");
            int remainNum = player.GetComponent<ShurikenShooter>().GetcurrentCartridge();
            for (int i = remainNum; i > 0; i--)
            {
                GameObject kunai = Instantiate(kunaiPrefab, transform);
                kunai.transform.SetParent(transform);
                kunai.transform.GetComponent<RectTransform>().position = new Vector3(transform.position.x - (offset * 2) * i, transform.position.y, transform.position.z);
            }
        }
        
    }
}
