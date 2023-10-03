using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapActive : MonoBehaviour
{
    private GameObject blueZone;
    private PlayerController[] players;
    private GameObject enemy;
    private GameObject map;

    private Transform player1SpawnPosition;
    private Transform player2SpawnPosition;

    public void Init(GameObject _enemy = null)
    {
        //enemy =  _enemy != null ? _enemy : transform.GetChild(0).gameObject;
        map = transform.GetChild(1).gameObject;
        blueZone = transform.GetChild(2).gameObject;

        //enemy.SetActive(true);
        map.SetActive(true);
        blueZone.SetActive(true);

        player1SpawnPosition = map.transform.Find("@Player1SpawnPoint");
        player2SpawnPosition = map.transform.Find("@Player2SpawnPoint");

        players = FindObjectsOfType<PlayerController>();

        foreach (PlayerController playerController in players)
        {
            switch (playerController.playerIndex)
            {
                case 0:
                    playerController.gameObject.transform.position = player1SpawnPosition.position;
                    break;
                case 1:
                    playerController.gameObject.transform.position = player2SpawnPosition.position;
                    break;
            }
        }
    }
}
