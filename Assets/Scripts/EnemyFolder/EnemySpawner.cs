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
    List<Enemy> enemies = new List<Enemy>();
    float time = 5;
    WaitForSeconds waitTime;
    public GameObject enemyPrefab;
    EnemyType enemyType;

    private List<EnemyData> enemydata = new List<EnemyData>();

    public delegate List<Node> GetNodeDelegate();
    public GetNodeDelegate GetLeafNode; // ���⼭ tree ����Ʈ ��������
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
    }

    // Start is called before the first frame update
    void Start()
    {
        FindNode();
        waitTime = new WaitForSeconds(time);
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        var enemy = Instantiate(enemyPrefab,transform.position,Quaternion.identity).GetComponent<Enemy>();
        var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
        enemies.Add(enemy);
        enemy.EnemyData = enemydata[_randomNum];
        enemy.SetFisrtNode(leafNodeList[Random.Range(0,leafNodeList.Count)]);
        // ���׹��� ������ ��ũ���ͺ� ������Ʈ�� ó���ؾ���.
        yield return waitTime;
    }

    void EnemyBuff() // ���ȿ� ���� �б� �����ؾ���
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Damage = 1.1f;
        }
    }
}
