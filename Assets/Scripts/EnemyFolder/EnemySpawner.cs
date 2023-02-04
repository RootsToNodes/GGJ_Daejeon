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

    public void SetNullNode() // �ش� �޼ҵ�� Ʈ���� �����ڸ��� ���� �ߵ�
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
            // ���׹��� ������ ��ũ���ͺ� ������Ʈ�� ó���ؾ���.
            yield return wait;
        }
    }

    void EnemyBuff() // ���ȿ� ���� �б� �����ؾ���
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Damage = damageBuffValue;
        }
    }
}
