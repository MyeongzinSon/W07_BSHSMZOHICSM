using UnityEngine;

public class DropItemCreateHandler : MonoBehaviour
{
    private float timer = 0.0f;
    private float spawnInterval = 20.0f;
    private int createIdx = -1;

    private void Update()
    {
        // 각각의 시간 간격에 따라 createIdx 설정
        if (timer >= 20.0f && createIdx == -1)
        {
            CreateDropItemSpawner(0);
            createIdx = 0;
        }
        else if (timer >= 40.0f && createIdx == 0)
        {
            CreateDropItemSpawner(1);
            createIdx = 1;
        }

        timer += Time.deltaTime;
    }

    private void CreateDropItemSpawner(int createIdx)
    {
        // DropItemSpawner를 생성하고 설정
        GameObject dropItemSpawnerPrefab = Resources.Load("Prefabs/DropItems/DropItemSpawner") as GameObject;

        if (dropItemSpawnerPrefab != null)
        {
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform.position;
            Vector3 spawnPosition = (playerPosition + enemyPosition) / 2.0f;

            GameObject dropItemSpawner = Instantiate(dropItemSpawnerPrefab, spawnPosition, Quaternion.identity);
            dropItemSpawner.GetComponent<DropItemHandler>().createIdx = createIdx;
        }
        else
        {
            Debug.LogError("DropItemSpawner prefab not found!");
        }
    }
}