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
        //destination = ; 델리게이트 또는 값 받아오기
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            // attack 구현 딜레이 포함
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
