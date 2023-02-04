using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{

    /*
     *
     * 이름, hp, 공격속도, 데미지, 이동 속도, 프리팹 개체, 오디오, 사냥금
     * 
     */

    [SerializeField]
    private string enemyName;
    public string EnemyName { get { return enemyName; } }
    /*--------------------------------------------------*/

    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    /*--------------------------------------------------*/

    [SerializeField] 
    private float attackSpeed;
    public float AttackSpeed { get { return attackSpeed; } }

    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
    /*--------------------------------------------------*/

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    /*--------------------------------------------------*/

    [SerializeField]
    private GameObject enemyPrefab;
    public GameObject EnemyPrefab { get { return enemyPrefab; } }
    /*--------------------------------------------------*/


    /*--------------------------------------------------*/
    [SerializeField]

    private AudioEnum enemyAudio;
    public AudioEnum EnemyAudio { get { return enemyAudio; } set { EnemyAudio = value; } }
    /*--------------------------------------------------*/


    /*--------------------------------------------------*/
    [SerializeField]

    private int enemyMoney;
    public int EnemyMoney { get { return enemyMoney; } set { enemyMoney = value; } }
    /*--------------------------------------------------*/

}