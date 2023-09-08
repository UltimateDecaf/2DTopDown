using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawn : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
   // private Vector3[] spawnPositions = new Vector3[] { new Vector3(-43, -45, 0), new Vector3(0, -49, 0), new Vector3(-45.8f, -45.4f, 0), new Vector3(42.9f, -44.3f, 0) };

   [SerializeField] private float minX = -40;
    [SerializeField] private float maxX = 40;
    [SerializeField] private float minY = -40;
    [SerializeField] private float maxY = 40;
    private int enemiesSpawned;
    [SerializeField] private float spawnTime = 5.0f;
    [SerializeField] private float subtractionTime = 0.5f;

    public override void OnNetworkSpawn()
    {
        enemiesSpawned = 0;
        StartCoroutine(SpawnEnemy());
       

    }

    private IEnumerator SpawnEnemy()
    {
        while(true) 
        {
            // int spawnIndex = Random.Range(0, spawnPositions.Length);
            float spawnPositionX = Random.Range(minX, maxX);
            float spawnPositionY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);
            // GameObject enemy = Instantiate(enemyPrefab, spawnPositions[spawnIndex], Quaternion.identity);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
            enemiesSpawned++;
            if (enemiesSpawned > 10)
            {
                if (spawnTime > 0.5f)
                {
                    spawnTime -= subtractionTime;
                }
                else
                {
                    spawnTime = 0.5f;
                }
                enemiesSpawned = 0;
            }
            yield return new WaitForSeconds(spawnTime);
        }
      
     
    }
}
