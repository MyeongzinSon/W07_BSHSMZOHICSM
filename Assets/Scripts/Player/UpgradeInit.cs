using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var playerIndex = GetComponent<PlayerController>().playerIndex;

        if (playerIndex == 0)
        {
            for (int i = 0; i < GameManager.Instance.upgradedListPlayer1.Count; i++)
            {
                transform.GetComponent<CharacterStats>().AddCharacterStats(GameManager.Instance.upgradedListPlayer1[i]);
            }
        }
        else if (playerIndex == 1)
        {
            for (int i = 0; i < GameManager.Instance.upgradedListPlayer2.Count; i++)
            {
                transform.GetComponent<CharacterStats>().AddCharacterStats(GameManager.Instance.upgradedListPlayer2[i]);
            }
        }
        else
        {
            Debug.LogError("UpgradeInit.Start() : PlayerIndex�� 0 �Ǵ� 1�� �ƴ�!");
        }
    }
}
