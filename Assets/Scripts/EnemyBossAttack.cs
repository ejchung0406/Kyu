using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAttack: EnemyAttack
{
    private float lastBossShootTime;
    public float shootingBossInterval = 1f;
    public int numBullets = 36;

    public override void Update()
    {
        if (shooting){
            // Check if enough time has passed since the last shoot
            if (Time.time - lastBossShootTime >= shootingBossInterval && distanceToPlayer <= detectionRange)
            {
                float angleStep = 360f / numBullets;

                for (int i = 0; i < numBullets; i++)
                {
                    float angle = i * angleStep;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    Vector2 direction = rotation * new Vector2(1, 0);

                    GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
                    bullet.GetComponent<Bullet>().damageAmount = atk / 2;

                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(direction * bulletSpeed / 2, ForceMode2D.Impulse);
                }

                // Update the last shoot time
                lastBossShootTime = Time.time;
            }
        }

        base.Update();
    }
}
