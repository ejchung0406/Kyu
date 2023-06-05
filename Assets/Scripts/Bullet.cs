using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public int damageAmount;
    public Shooting shooter;
    public bool triggerBonusAttack;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Rigidbody2D rb;
    public bool rotation;


    // Start is called before the first frame update
    public virtual void Start()
    {   
        rb = this.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    public virtual void Update()
    {   
        if (rotation){ 
            Vector2 lookDir = new Vector2(endPosition.x, endPosition.y) - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.1f);
        Destroy(gameObject);
    }
}
