using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatridgeUIManager : MonoBehaviour
{
    private int catridgeNum;
    private float initialWidth;
    private float addWidth;
    private float offset = 40f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        initialWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        catridgeNum = player.GetComponent<CharacterStats>().maxCartridgeNum;
    }

    void DrawCatridgeUIBasedOnMaxCartridgeNum()
    {
        addWidth = (initialWidth - offset) * catridgeNum;
        float xx = initialWidth + addWidth;
        float yy = transform.GetComponent<RectTransform>().sizeDelta.y;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(xx, yy);
        transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(xx + 20, yy + 20);
    }
}
