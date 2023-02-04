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
            targetEnemy = GameManager.instance.GetEnemyInRange(transform.position, node.currentStatus.shotRange);
            lastAttackTime = Time.time;
            return;
        }

        var dir = (transform.position - targetEnemy.transform.position);
        
        var curAngle = transform.rotation.eulerAngles.z;
        var angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg -90;
        var rotationDelta = node.currentStatus.rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(curAngle,angle,rotationDelta));
        
        if (lastAttackTime + node.currentStatus.attackSpeed < Time.time)
        {
            lastAttackTime = Time.time;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        var bulletform = node.currentStatus.bulletForm;
        var bulletSpeed = node.currentStatus.bulletSpeed;
        var bulletPower = node.currentStatus.bulletSpeed;
        var bulletLifeTime = node.currentStatus.bulletLifeTime;
        
        for (int i = 0; i < node.currentStatus.bulletCount; i++)
        {
            var bullet = Instantiate(bulletPrefab);
            
            bullet.transform.position = bulletStart.position;

            var dir = (bulletStart.position - transform.position).normalized;
            bullet.Initialization(dir, bulletSpeed, bulletPower, bulletLifeTime);
        }
    }
}
