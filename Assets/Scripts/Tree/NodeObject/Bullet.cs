using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    private float speed = 0;
    private float damage = 0;
    private float maxRange = 0;

    private float startTime = Mathf.Infinity;
    
    public void Initialization(Vector2 dir,float speed, float damage, float maxRange)
    {
        this.dir = dir;
        this.speed = speed;
        this.damage = damage;
        this.maxRange = maxRange;
        
        startTime = Time.time;
    }
    
    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (startTime + maxRange < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().OnDamage(damage);
            Destroy(gameObject);
        }
    }
}
