using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawn : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Vector3[] spawnPositions = new Vector3[] { new Vector3(-43, -45, 0), new Vector3(0, -49, 0), new Vector3(-45.8f, -45.4f, 0), new Vector3(42.9f, -44.3f, 0) };
    [SerializeField] private float spawnTime = 10.0f;
    [SerializeField] private float subtractionTime = 0.01f;

    public override void OnNetworkSpawn()
    {
        InvokeRepeating("SpawnEnemy", 0.5f, spawnTime);
    }
  
    void SpawnEnemy()
    {
        if (!IsServer) return;
        int spawnIndex = Random.Range(0, spawnPositions.Length);
        GameObject enemy = Instantiate(enemyPrefab, spawnPositions[spawnIndex], Quaternion.identity);
        enemy.GetComponent<NetworkObject>().Spawn();
        if(spawnTime > 2f)
        {
            spawnTime -= subtractionTime;
        }
        else
        {
            spawnTime = 3f;
        }
    
    }
}
