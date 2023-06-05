using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int atk;
    private Rigidbody2D rb;

    public Transform player;
    public Transform[] players;
    public float movementSpeed = 10000f;
    public float detectionRange = 10f;
    public float avoidanceForce = 10000f;
    public float avoidanceDistance = 0.1f;

    public float distanceToPlayer;

    public GameObject bulletPrefab;
    public float shootingInterval = 1f;
    public float bulletSpeed = 10f;

    private float lastShootTime;

    public bool shooting = false;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        atk = 10;
        player = findNearestPlayer();
    }

    public virtual void Update()
    {
        player = findNearestPlayer();
        // Calculate the distance between the enemy and the player
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the distance is within the detection range, move towards the player
        if (distanceToPlayer <= detectionRange)
        {
            // Calculate the direction to the player
            Vector2 desiredDirection = (player.position - transform.position).normalized;

            // Check for overlap with other enemies and avoid them
            AvoidOtherEnemies();

            // Apply the movement force
            rb.AddForce(desiredDirection * movementSpeed * Time.deltaTime);
        }

        if (shooting){
            // Check if enough time has passed since the last shoot
            if (Time.time - lastShootTime >= shootingInterval && distanceToPlayer <= detectionRange)
            {
                // Create a new bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().damageAmount = atk;

                // Calculate the direction towards the player
                Vector2 direction = (player.position - transform.position).normalized;

                // Get the rigidbody of the bullet
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                // Apply force to the bullet towards the player
                bulletRb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);

                // Update the last shoot time
                lastShootTime = Time.time;
            }
        }
    }

    void AvoidOtherEnemies()
    {
        // Get all other enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Skip self
            if (enemy == this)
                continue;

            // Calculate the distance to the other enemy
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            // If the distance is less than the avoidance distance, avoid the other enemy
            if (distanceToEnemy <= avoidanceDistance)
            {
                // Calculate the direction away from the other enemy
                Vector2 avoidanceDirection = (transform.position - enemy.transform.position).normalized;

                // Apply the avoidance force to steer away from the other enemy
                rb.AddForce(avoidanceDirection * avoidanceForce * Time.deltaTime);
            }
        }
    }

    public int getAtk(){
        return atk;
    }

    public Transform findNearestPlayer() {
        Transform nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playerObjects.Length];

        for (int i = 0; i < playerObjects.Length; i++) {
            players[i] = playerObjects[i].transform;
        }

        foreach (Transform player in players)
        {
            float distanceToPlayer = Vector3.Distance(this.transform.position, player.position);

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;
    }
}
