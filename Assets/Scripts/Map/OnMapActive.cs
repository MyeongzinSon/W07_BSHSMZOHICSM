using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapActive : MonoBehaviour
{
    private GameObject blueZone;
    private GameObject player;
    private GameObject enemy;
    private GameObject map;

    private Transform playerSpawnPosition;

    public void Init(GameObject _enemy = null)
    {
        //enemy =  _enemy != null ? _enemy : transform.GetChild(0).gameObject;
        map = transform.GetChild(1).gameObject;
        blueZone = transform.GetChild(2).gameObject;

        //enemy.SetActive(true);
        map.SetActive(true);
        blueZone.SetActive(true);

        playerSpawnPosition = map.transform.Find("@Player1SpawnPoint");

        player = FindObjectOfType<PlayerController>().gameObject;

        player.transform.position = playerSpawnPosition.position;


    }
}
