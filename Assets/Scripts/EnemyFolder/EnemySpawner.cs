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

    private List<Node> leafNodeList;

    public void SetLeafNodeList(List<Node> leafNodes)
    {
        leafNodeList = leafNodes;
    }
    
    public void SetNullNode() // �ش� �޼ҵ�� Ʈ���� �����ڸ��� ���� �ߵ�
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetNullNode();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wait = new WaitForSeconds(time);
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        while (true)
        {
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
            enemies.Add(enemy);
            enemy.EnemyData = enemydata[_randomNum-1];
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
