using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 5;
    public int maxSpawn;
    public int startEnemies = 1;
    public float spawnRadius = 10f;
    public float spawnInterval = 1f;

    private int currentEnemies = 0;
    public bool stopSpawning = false;
    private float spawnTimer = 0f;

    void Start()
    {
        maxSpawn = maxEnemies * 2;
        // Spawn the maximum number of enemies at the start of the game
        for (int i = 0; i < startEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    public virtual void Update()
    {
        if (maxSpawn <= 0) stopSpawning = true;
        if (stopSpawning) Destroy(gameObject);

        // Check if maximum number of enemies is reached
        if (currentEnemies >= maxEnemies)
        {
            return; // Stop spawning
        }

        // Increment the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if the spawn interval has passed
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f; // Reset the timer
        }
    }

    void SpawnEnemy()
    {
        // Randomly select a position within the spawn radius
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.z = 0f; // Assuming a 2D environment

        // Instantiate the enemy prefab at the spawn position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().spawner = this;

        // Increment the count of spawned enemies
        currentEnemies++;
        maxSpawn--;
    }

    public void DecrementEnemies(){
        currentEnemies--;
    }
}


