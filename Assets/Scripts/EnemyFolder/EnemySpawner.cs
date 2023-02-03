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
    public GetNodeDelegate GetLeafNode; // 여기서 tree 리스트 가져오기
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
        // 에네미의 성능을 스크립터블 오브젝트로 처리해야함.
        yield return waitTime;
    }

    void EnemyBuff() // 스탯에 따른 분기 구현해야함
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Damage = 1.1f;
        }
    }
}
