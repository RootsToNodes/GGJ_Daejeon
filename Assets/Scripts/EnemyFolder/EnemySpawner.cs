using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


enum EnemyType
{
    normal,
    strong,
    range
}

public class EnemySpawner : MonoBehaviour
{
    const float time = 1;
    
    public GameObject enemyPrefab;
    
    [SerializeField] private List<EnemyData> enemydata = new List<EnemyData>();


    public readonly List<Enemy> enemiseList = new List<Enemy>();
    private List<Node> leafNodeList;

    private Dictionary<int, int> waveLevelDictionary = new Dictionary<int, int>();
    private int waveLevel = 0;
    private WaitForSeconds wait;

    public GameObject trailObject;

    public void SetLeafNodeList(List<Node> leafNodes)
    {
        leafNodeList = leafNodes;
        if (enemiseList?.Count != 0)
        {
            for (int i = 0; i < enemiseList.Count; i++)
            {
                enemiseList[i]?.SetFisrtNode(leafNodeList[Random.Range(0, leafNodeList.Count)]);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wait = new WaitForSeconds(time);
        waveLevelDictionary.Add(1, 2);
        waveLevelDictionary.Add(2, 5);
        waveLevelDictionary.Add(3, 7);
        waveLevelDictionary.Add(4, 10);
        waveLevelDictionary.Add(5, 20);
        waveLevelDictionary.Add(6, 22);
        waveLevelDictionary.Add(7, 25);
        waveLevelDictionary.Add(8, 28);
        waveLevelDictionary.Add(9, 32);
        waveLevelDictionary.Add(10, 50);
    }

    public void AttackStart()
    {
        waveLevel++;
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        Debug.Log(leafNodeList.Count);
        while (true)
        {
            var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
            var _randomPosX = Random.Range(-1f, 1f);
            var _randomPosY = Random.Range(-1f, 1f);

            var pos = new Vector3(transform.position.x + _randomPosX, transform.position.y + _randomPosY);
            var enemyData = enemydata[Random.Range(0,enemydata.Count-1)];
            var enemy = Instantiate(enemyData.EnemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();
            enemy.Initialize(enemyData, OnEnemyDie);

            enemiseList.Add(enemy);


            if (enemiseList.Count >= waveLevelDictionary[waveLevel])
            {
                SoundManager.PlaySound(AudioEnum.StartSound);
                yield return new WaitForSeconds(2f);
                for (int i = 0; i < enemiseList.Count; i++)
                {
                    enemiseList[i].SetFisrtNode(leafNodeList[Random.Range(0, leafNodeList.Count)]);
                    enemiseList[i].StartMove();
                }
                Debug.Log(enemiseList.Count);
                yield break;
            }
            yield return wait;
        }
    }

    private void OnEnemyDie(Enemy enemy)
    {
        enemiseList.Remove(enemy);
    }
}
