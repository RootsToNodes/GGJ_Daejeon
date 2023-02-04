using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : NodeObject
{
    [SerializeField] private Bullet bulletPrefab;
    
    [SerializeField] private float radialFormAngle;
    [SerializeField] private float straightFormRange;

    [SerializeField] private Transform bulletStart;
    [SerializeField] private GameObject gun;
    
    private Enemy targetEnemy;
    private float lastAttackTime;

    private bool isOn;
    
    public override void OnDamage(int amount)
    {
    }

    public override void OnHealing(int amount)
    {
    }

    public void SetEnable(bool enable)
    {
        isOn = enable;
        gun.SetActive(enable);
    }
    

    private void Update()
    {
        if (!isOn)
        {
            return;
        }
        
        if (!targetEnemy)
        {
            targetEnemy = node.GetTarget();
            return;
        }

        var dir = (transform.position - targetEnemy.transform.position);
        
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        
        if (lastAttackTime + node.currentStatus.attackSpeed > Time.time)
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        var bulletform = node.currentStatus.bulletForm;
        var bulletSpeed = node.currentStatus.bulletSpeed;
        var bulletPower = node.currentStatus.bulletSpeed;
        
        for (int i = 0; i < node.currentStatus.bulletCount; i++)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.Initialization(((Vector2) bulletStart.localPosition).normalized, bulletSpeed, bulletPower);
        }
    }
}
