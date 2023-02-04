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
        // ��尡 ���� �Ĵ� ��� ó���ұ�?
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

    /*���� �����ϴ� �ó�����.
    �� �Ǵ� �ð��� ���� ���� �ĵ��´�.
    ���� ������ ��ġ���� spawner�� ���� �����ȴ�.
    �����ɶ� ���� ������ �������� �����ȴ� (���� ��ü, ���� ��ü)
    ���� ������带 ���� ������ �����Ѵ�. -> ������� Ž��

    ���� ������忡 ������� �ش� ������Ʈ�� �����Ѵ�. (��, ���, �Ǵ� ��ž)
    ���� ��带 �����ϸ� ���� ü���� ȸ���ǰ� ��ü���� �����Ѵ�. (�������� ����?)
    ���� ��Ʈ ��忡 ������ ������ ����ȴ�.
    */

}
