using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies during the boss battle
/// </summary>
public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;                                 //the enemy which will be spawned
    public float spawnDelay;                                //time delay between two enemies

    bool canSpawn;                                          //ensures that enemies are spawned after some delay

    void Start()
    {
        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine("SpawnEnemy");
        }
    }

    IEnumerator SpawnEnemy()
    {
        Instantiate(enemy, transform.position, Quaternion.identity); //spawns the enemy
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
