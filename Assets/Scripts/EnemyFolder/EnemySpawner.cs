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

    [SerializeField]
    private List<EnemyData> enemydata = new List<EnemyData>();


    // Start is called before the first frame update
    void Start()
    {
        waitTime = new WaitForSeconds(time);
        StartCoroutine(GenerateEnemy());
    }

    IEnumerator GenerateEnemy()
    {
        var enemy = Instantiate(enemyPrefab,transform.position,Quaternion.identity).GetComponent<Enemy>();
        var _randomNum = System.Enum.GetValues(typeof(EnemyType)).Length;
        enemies.Add(enemy);
        enemy.EnemyData = enemydata[_randomNum];
        // ���׹��� ������ ��ũ���ͺ� ������Ʈ�� ó���ؾ���.
        yield return waitTime;
    }
}
