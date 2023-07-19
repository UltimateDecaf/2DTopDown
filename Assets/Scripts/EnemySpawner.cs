using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
       StartCoroutine("SpawnEnemies");
    }

    IEnumerator SpawnEnemies()
    {
        Instantiate(enemy); 
        yield return new WaitForSeconds(5);

    }
}
