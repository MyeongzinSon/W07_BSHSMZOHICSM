using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpHandler : MonoBehaviour
{
    public GameObject playerHpBar;
    public GameObject playerHpText;
    public Damageable playerStats;

    private float smoothHp; 
    private float smoothVelocity; 
    
    void Start()
    {
        smoothHp = playerStats.hp;
        smoothVelocity = 0.0f;
    }

    void Update()
    {
        smoothHp = Mathf.SmoothDamp(smoothHp, playerStats.hp, ref smoothVelocity, 0.2f);
        if (playerHpBar != null && playerHpText != null)
        {
            playerHpBar.GetComponent<Image>().fillAmount = smoothHp / playerStats.maxHp;
            playerHpText.GetComponent<TextMeshProUGUI>().text = playerStats.hp.ToString("F0") + " / " + playerStats.maxHp.ToString("F0");
        }
        //else
        //{
        //    playerHpBar.GetComponent<Image>().fillAmount = 0;
        //}
    }

    public void SetPlayerTextToZero()
    {
        if (playerHpText != null)
        {
            playerHpText.GetComponent<TextMeshProUGUI>().text = "0 / " + playerStats.maxHp.ToString("F0");
        }
    }
}