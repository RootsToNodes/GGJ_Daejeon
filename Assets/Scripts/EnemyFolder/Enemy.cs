using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private AudioEnum sound = AudioEnum.StartSound;

    private int hp;
    private int damage;
    private float moveSpeed;
    private string enemyName;
    private float enemyWaitTime = 5f;

    private float destinationDistance;
    private float minRange = 0.5f;

    public int Hp { get { return hp; }}
    public int Damage { get { return damage; } set { damage += value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed *= value; } }
    private EnemyData enemyData;

    public EnemyData EnemyData { get { return enemyData; } set { enemyData = value; } }

    Node currentNode;
    Node nextTargetNode;


    public void SetFisrtNode(Node node)
    {
        currentNode = node;
        SetNextNode();
    }

    public void Initialize()
    {
        hp = enemyData.Hp;
        damage = enemyData.Damage;
        moveSpeed = enemyData.MoveSpeed;
        enemyName = enemyData.name;
        TestSound();
        SoundManager.PlaySound(sound);
    }

    public void TestSound()
    {
        var random = Random.Range(1,System.Enum.GetValues(typeof(AudioEnum)).Length);
        sound = (AudioEnum)(random-1);
    }

    private void SetNextNode()
    {
        nextTargetNode = currentNode.parent;
    }
    public void SetNullNode()
    {
        nextTargetNode = null;
        // 노드가 끊긴 후는 어떻게 처리할까?
    }

    public void StartMove()
    {
        InvokeRepeating("MoveToNode", 2f, 0.01f);
    }
    private void MoveToNode()
    {
        if (currentNode == null)
        {
            currentNode = nextTargetNode;
            SetNextNode();
        }
        transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position, Time.deltaTime * moveSpeed * 20);
        CheckDistance();
    }
    private void Update()
    {
        
    }

    void CheckDistance()
    {
        destinationDistance =  Vector3.Distance(transform.position ,currentNode.transform.position);
        if (destinationDistance > minRange)
        {
            
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Object"))
    //    {
    //        collision.gameObject.GetComponent<NodeObject>().OnDamage(Damage);
    //    }
    //}


    public void GetDamaged()
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
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
    }

    /*적이 공격하는 시나리오.
    턴 또는 시간에 따라 적이 쳐들어온다.
    적은 정해진 위치에서 spawner를 통해 생성된다.
    생성될때 적의 스탯은 동적으로 생성된다 (강한 개체, 약한 개체)
    적은 리프노드를 향해 공격을 개시한다. -> 리프노드 탐색

    적이 리프노드에 닿을경우 해당 오브젝트에 공격한다. (방어벽, 노드, 또는 포탑)
    적이 노드를 공략하면 적의 체력이 회복되고 개체수가 증가한다. (데미지도 증가?)
    적이 루트 노드에 닿으면 게임이 종료된다.
    */

}
