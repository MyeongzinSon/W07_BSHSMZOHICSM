using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapActive : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private GameObject map;

    private Transform playerSpawnPosition;

    public void Init()
    {
        enemy = transform.GetChild(0).gameObject;
        map = transform.GetChild(1).gameObject;

        enemy.SetActive(true);
        map.SetActive(true);

        playerSpawnPosition = map.transform.Find("@PlayerSpawnPoint");

        player = FindObjectOfType<PlayerController>().gameObject;

        player.transform.position = playerSpawnPosition.position;
    }
}