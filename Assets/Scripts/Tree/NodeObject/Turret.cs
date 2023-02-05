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

    private Animator animator;
    private bool isOn;

    public override void OnDamage(float amount)
    {
        hp -= amount;
        //hp -= Mathf.Max(amount - node.currentStatus.defense, 0);

        if (hp < 0)
        {
            SetEnable(false);
        }
    }

    public override void OnHealing(float amount)
    {
        gameObject.SetActive(true);
        SetEnable(true);

        hp = amount;
    }

    public void SetEnable(bool enable)
    {
        animator = GetComponent<Animator>();
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
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + node.currentStatus.rotationSpeed * Time.deltaTime);
            
            targetEnemy = GameManager.instance.GetEnemyInRange(transform.position, node.currentStatus.shotRange);
            lastAttackTime = Time.time;
            return;
        }

        var dir = (transform.position - targetEnemy.transform.position);

        var curAngle = transform.rotation.eulerAngles.z;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        var rotationDelta = node.currentStatus.rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(curAngle, angle, rotationDelta));

        if (lastAttackTime + node.currentStatus.attackSpeed < Time.time)
        {
            lastAttackTime = Time.time;
            SoundManager.PlaySound(AudioEnum.TurretAttack);
            animator.SetTrigger("attackTrigger");

            FireBullet();
        }
    }

    private void FireBullet()
    {
        var bulletCount = node.currentStatus.bulletCount;

        var baseDir = (bulletStart.position - transform.position).normalized;

        for (int i = 0; i < bulletCount; i++)
        {
            var pos = bulletStart.position;
            var dir = baseDir;

            switch (node.currentStatus.bulletForm)
            {
                case NodeStatus.BulletForm.Straight:
                {
                    var line = straightFormRange;
                    line -= straightFormRange / bulletCount * i;

                    pos += dir * line;
                    break;
                }
                case NodeStatus.BulletForm.Radial:
                {
                    var angle = -radialFormAngle * 0.5f;
                    angle += radialFormAngle / bulletCount * i;

                    dir = Quaternion.Euler(0, 0, angle) * dir;
                    break;
                }
                    ;
            }

            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = pos;
            bullet.Initialization(dir, node.currentStatus.bulletSpeed, node.currentStatus.attackPower,
                node.currentStatus.bulletLifeTime);
            SoundManager.PlaySound(AudioEnum.TurretAttack);
        }
    }
}