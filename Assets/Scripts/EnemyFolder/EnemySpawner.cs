using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EnemyType
{
    normal,
    strong,
    range
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<EnemyData> enemydata = new List<EnemyData>();
    List<Enemy> enemies = new List<Enemy>();
    WaitForSeconds wait;
    public GameObject enemyPrefab;

    float time = 5;
    int damageBuffValue = 1;

    public delegate List<Node> GetNodeDelegate();
    public GetNodeDelegate GetLeafNode;
    List<Node> leafNodeList;

    public void SetNullNode() // 해당 메소드는 트리의 가위자르기 사용시 발동
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetNullNode();
        }
    }
    public void FindNode()
    {
        leafNodeList = GetLeafNode();
        Debug.Log(leafNodeList);
    }

    // Start is called before the first frame update
    void Start()
    {
        FindNode();
        wait = new WaitForSeconds(time);
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        while (true)
        {
            var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.EnemyData = enemydata[_randomNum-1];
            enemy.Initialize();
            enemies.Add(enemy);
            enemy.SetFisrtNode(leafNodeList[Random.Range(0, leafNodeList.Count)]);
            // 에네미의 성능을 스크립터블 오브젝트로 처리해야함.
            yield return wait;
        }
    }

    void EnemyBuff() // 스탯에 따른 분기 구현해야함
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Damage = damageBuffValue;
        }
    }
}
