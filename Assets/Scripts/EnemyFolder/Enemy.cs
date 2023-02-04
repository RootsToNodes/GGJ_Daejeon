using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    private int hp;
    private int damage;
    private float moveSpeed;
    

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
    private void Update()
    {
        MoveToNode();
    }
    public void Initialize()
    {
        hp = enemyData.Hp;
        damage = enemyData.Damage;
        moveSpeed = enemyData.MoveSpeed;
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
        transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position,Time.deltaTime * moveSpeed);
        Debug.Log(Vector3.MoveTowards(this.transform.position, currentNode.transform.position, Time.deltaTime * moveSpeed));
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
