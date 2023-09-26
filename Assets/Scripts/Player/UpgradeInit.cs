using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.upgradedList.Count; i++)
        {
            transform.GetComponent<CharacterStats>().AddCharacterStats(GameManager.Instance.upgradedList[i]);
        }
    }
}
