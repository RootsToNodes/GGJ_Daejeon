using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private int hp;
    private int damage;
    private float moveSpeed;

    public int Hp { get { return hp; } set { hp = value; } }
    public int Damage { get { return damage; } set { damage *= value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed *= value; } }
    private EnemyData enemyData;

    public EnemyData EnemyData { get { return enemyData; } set { enemyData = value; } }

    Node destination;

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

    private void MoveToNode()
    {
        if (destination == null)
        {
            FindNode();
        }
        transform.Translate(destination.transform.position * Time.deltaTime * enemyData.MoveSpeed);
    }
    private void FindNode()
    {
        //destination = ; ��������Ʈ �Ǵ� �� �޾ƿ���
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            // attack ���� ������ ����
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
