using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    private float hp;
    private float damage;
    private float moveSpeed;
    

    public float Hp { get { return hp; } set { hp = value; } }
    public float Damage { get { return damage; } set { damage *= value; } }
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
    private void Start()
    {
    }
    private void Update()
    {
        MoveToNode();
    }
    public void Initialize()
    {
        Hp = enemyData.Hp;
        Damage = enemyData.Damage;
        MoveSpeed = enemyData.MoveSpeed;
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

    private void MoveToNode()
    {
        if (currentNode == null)
        {
            currentNode = nextTargetNode;
            SetNextNode();
        }
        transform.Translate(currentNode.transform.position * Time.deltaTime * enemyData.MoveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            collision.gameObject.GetComponent<NodeObject>().OnDamage(Damage);
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
