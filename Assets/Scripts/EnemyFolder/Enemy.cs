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
        // ��尡 ���� �Ĵ� ��� ó���ұ�?
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
