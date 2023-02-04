using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    private const float TempSpeedValue = 1;
    
    private AudioEnum sound = AudioEnum.StartSound;

    private float hp;
    private int damage;
    private float attackSpeed;
    private float moveSpeed;
    private string enemyName;

    [Header("���� �ּ� �Ÿ�")]
    public float attackRange = 0.5f;
    
    public int Damage { get { return damage; } set { damage += value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed *= value; } }
    public string Name { get { return enemyName; }}


    private Node currentNode;
    private Node nextTargetNode;

    private bool canMoving = false;
    private bool isAttacking = false;

    private float lastAttackTime;

    private UnityAction<Enemy> onDie;

    public void Initialize(EnemyData enemyData, UnityAction<Enemy> onDie)
    {
        canMoving = false;
        
        hp = enemyData.Hp;
        damage = enemyData.Damage;
        attackSpeed = enemyData.AttackSpeed;
        moveSpeed = enemyData.MoveSpeed;
        enemyName = enemyData.name;

        this.onDie = onDie;
        
        SoundManager.PlaySound(sound);
    }
    
    public void SetFisrtNode(Node node)
    {
        currentNode = node;
        SetNextNode();
    }

    private void SetNextNode()
    {
        nextTargetNode = currentNode.parent;
    }
    public void SetNullNode()
    {
        nextTargetNode = null;
        // ��尡 ���� �Ĵ� ��� ó���ұ�?
    }

    public void StartMove()
    {
        canMoving = true;
    }
    
    private void Update()
    {
        if (canMoving)
        {
            if (currentNode == null)
            {
                currentNode = nextTargetNode;
                SetNextNode();
                isAttacking = false;
            }
            
            if (isAttacking)
            {
                AttackNode();
            }
            else
            {
                MoveToNode();
            }
        }

        if (tempShakeIntensity > 0 && isShaking == true)
        {
            transform.position = originPosition + Random.insideUnitSphere * tempShakeIntensity;
            tempShakeIntensity -= shakeDecay;
        }
        else
        {
            isShaking = false;
        }
    }

    
    private void MoveToNode()
    {
        var dir = (transform.position - currentNode.transform.position);
        var moveDelta = Time.deltaTime * moveSpeed * TempSpeedValue;
        
        transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position, moveDelta);
        
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        
        if (dir.sqrMagnitude <= attackRange * attackRange)
        {
            isAttacking = true;
            lastAttackTime = Time.time;
        }
        else
        {
            tempShakeIntensity = 0;
        }
    }

    private void AttackNode()
    {
        if (lastAttackTime + attackSpeed < Time.time)
        {
            lastAttackTime = Time.time;
            
            currentNode.OnDamage(damage);
            
            Shake();
        }
        
        if ((transform.position - currentNode.transform.position).sqrMagnitude > attackRange * attackRange * 2)
        {
            isAttacking = false;
        }
    }
    
    
    
    private Vector3 originPosition ;
    public float shakeDecay = 0.01f;
    public float shakeIntensity = .4f;
    public bool isShaking = false;

    private float tempShakeIntensity = 0;

    public void Shake()
    {
        if (!isShaking)
        {
            isShaking = true;
            originPosition = transform.position;
            tempShakeIntensity = shakeIntensity;
        }
    }
    
    public void OnDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        onDie?.Invoke(this);
        
        switch (enemyName)
        {
            case "normal":
                DataManager.GetMoney(5);
                break;
            case "strong":
                DataManager.GetMoney(10);
                DataManager.GetGas(5);
                break;
            case "range":
                DataManager.GetMoney(7);
                break;
            default:
                break;
        }
        
        Destroy(gameObject);
    }
    
    

}
