using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.upgradedListPlayer1.Count; i++)
        {
            transform.GetComponent<CharacterStats>().AddCharacterStats(GameManager.Instance.upgradedListPlayer1[i]);
        }
        
        for (int i = 0; i < GameManager.Instance.upgradedListPlayer2.Count; i++)
        {
            transform.GetComponent<CharacterStats>().AddCharacterStats(GameManager.Instance.upgradedListPlayer2[i]);
        }
    }
}
