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
    List<Enemy> enemiesList = new List<Enemy>();
    List<Enemy> remainEnemiseList= new List<Enemy>();

    WaitForSeconds wait;
    public GameObject enemyPrefab;

    readonly float time = 1;
    int damageBuffValue = 1;

    private List<Node> leafNodeList;

    Dictionary<int, int> waveLevelDictionary = new Dictionary<int, int>();
    int waveLevel = 0;

    public void SetLeafNodeList(List<Node> leafNodes)
    {
        leafNodeList = leafNodes;
    }
    
    public void SetNullNode() // �ش� �޼ҵ�� Ʈ���� �����ڸ��� ���� �ߵ�
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].SetNullNode();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wait = new WaitForSeconds(time);
        waveLevelDictionary.Add(1, 5);
        waveLevelDictionary.Add(2, 10);
        waveLevelDictionary.Add(3, 15);
        waveLevelDictionary.Add(4, 20);
        waveLevelDictionary.Add(5, 40);
        waveLevelDictionary.Add(6, 45);
        waveLevelDictionary.Add(7, 50);
        waveLevelDictionary.Add(8, 55);
        waveLevelDictionary.Add(9, 60);
        waveLevelDictionary.Add(10, 90);
    }

    public void AttackStart()
    {
        FindNode();
        waveLevel++;
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        remainEnemiseList.Clear();
        while (true)
        {
            var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.EnemyData = enemydata[_randomNum-1];
            enemy.Initialize();
            enemiesList.Add(enemy);
            remainEnemiseList.Add(enemy);
            enemy.SetFisrtNode(leafNodeList[Random.Range(0, leafNodeList.Count)]);
            
            // ���׹��� ����� ��ũ���ͺ� ����Ʈ�� ó���ؾ���.
            if (remainEnemiseList.Count > waveLevelDictionary[waveLevel])
            {
                for (int i = 0; i < remainEnemiseList.Count-1; i++)
                {
                    remainEnemiseList[i].StartMove();
                }
                Debug.Log(remainEnemiseList.Count-1);
                yield break;
            }
            yield return wait;
        }
    }

    void EnemyBuff() // ���ȿ� ���� �б� �����ؾ���
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].Damage = damageBuffValue;
        }
    }
}
