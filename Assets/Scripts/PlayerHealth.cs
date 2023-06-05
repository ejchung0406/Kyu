using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public GameObject floatingPoints;
    private StatSystem statSystem;

    // Start is called before the first frame update
    public void setupHealth()
    {
        statSystem = GetComponent<Shooting>().getStatSystem();
        healthBar.SetMaxHealth(statSystem.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (statSystem.getCurrentHealth() <= 0){
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Apply damage to the player
            TakeDamage(collision.gameObject.GetComponent<EnemyAttack>().getAtk());
        }
        if (collision.gameObject.CompareTag("EnemyBullet")) {
            // Apply damage to the player
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damageAmount);
        }
    }

    void TakeDamage(float damage){
        statSystem.changeHP(-damage);

        GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity);
        points.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        points.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;

        healthBar.SetHealth(statSystem.getCurrentHealth());
    }

    public void Heal(float heal){
        statSystem.changeHP(heal);

        GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity);
        points.transform.GetChild(0).GetComponent<TextMesh>().text = heal.ToString();
        points.transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
        points.transform.GetChild(0).GetComponent<TextMesh>().fontSize = 50;

        healthBar.SetHealth(statSystem.getCurrentHealth());
    }
}
