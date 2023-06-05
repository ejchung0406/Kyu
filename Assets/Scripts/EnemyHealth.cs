using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int experience;
    public int currentHealth;

    public HealthBar healthBar;

    public EnemySpawner spawner;

    public GameObject floatingPoints;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            // Apply damage to the enemy
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(bullet.damageAmount, bullet.shooter, bullet.triggerBonusAttack);
        }

        if (collision.gameObject.CompareTag("Player")) {
            // Apply damage to the enemy
            Shooting shooter = collision.gameObject.GetComponent<Shooting>();
            TakeDamage(shooter.punchDamageAmount, shooter, false);
        }
    }

    void TakeDamage(int damage, Shooting shooter, bool triggerBonusAttack) {
        if (Random.value <= shooter.critialPoss){
            // critial damage!
            damage *= 2;
            GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity);
            points.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
            points.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
            
        } else {
            GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity);
            points.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        shooter.enemyHit(damage, triggerBonusAttack);

        if (currentHealth <= 0){
            DefeatEnemy();
            shooter.enemyDied(experience);
        }
    }

    void DefeatEnemy() {
        // Perform actions when the enemy is defeated
        // e.g., play death animation, give player points, etc.
        spawner.DecrementEnemies();
        Destroy(gameObject);
    }
}
