using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    private float speed = 0;
    private float damage = 0;
    
    public void Initialization(Vector2 dir,float speed, float damage)
    {
        this.dir = dir;
        this.speed = speed;
        this.damage = damage;
    }
    
    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().OnDamage(damage);
        }
    }
}
