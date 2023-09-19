using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Created by Lari Basangov
//Spawns enemies in the map within spawnTime rate, which is subtracted each time 10 eneimes have been spawned. The subtraction stops, when the spawn rate has reached its minimal value
public class EnemySpawn : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

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
            float spawnPositionX = Random.Range(minX, maxX);
            float spawnPositionY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);
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
