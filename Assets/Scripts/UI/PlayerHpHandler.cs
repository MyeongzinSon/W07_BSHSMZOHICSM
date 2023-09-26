using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpHandler : MonoBehaviour
{
    public GameObject playerHpBar;
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
        if (playerHpBar != null)
        {
            playerHpBar.GetComponent<Image>().fillAmount = smoothHp / playerStats.maxHp;
        }
        //else
        //{
        //    playerHpBar.GetComponent<Image>().fillAmount = 0;
        //}
    }
}