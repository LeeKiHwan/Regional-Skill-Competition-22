using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Game Status")]
    [SerializeField] private int score = 0;
    [SerializeField] private float pain;
    [SerializeField] bool isGameover;

    [Header("Game Objects")]
    public GameObject[] MonsterObjects;
    public GameObject ItemObject;
    public GameObject BackGround;

    float monsterSpawnDelay = 2f;
    float itemSpawnDelay = 10f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("GameManage Instance Error");
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    void Update()
    {
        SpawnMonster();
        SpawnItem();
        BackGroundMove();
    }

    private void SpawnMonster()
    {
        if (monsterSpawnDelay > 0)
        {
            monsterSpawnDelay -= Time.deltaTime;
        }
        else
        {
            int ranMonster = Random.Range(0, 3);

            Instantiate(MonsterObjects[ranMonster], new Vector3(Random.Range(-8.0f, 8.0f), 6, 0), Quaternion.identity);
            monsterSpawnDelay = 2f;
        }
    }

    private void SpawnItem()
    {
        if (itemSpawnDelay > 0)
        {
            itemSpawnDelay -= Time.deltaTime;
        }
        else
        {
            Instantiate(ItemObject, new Vector3(Random.Range(-8.0f, 8.0f), 6, 0), Quaternion.identity);
            itemSpawnDelay = 10f;
        }
    }

    public void AddScore(int score)
    {
        if (!isGameover)
        {
            this.score += score;
        }
    }

    private void BackGroundMove()
    {
        Vector3 curPos = BackGround.transform.position;
        Vector3 nextPos = Vector3.down * 1 * Time.deltaTime;

        BackGround.transform.position = curPos + nextPos;

        if (BackGround.transform.position.y <= -12)
        {
            BackGround.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void TakePain(float pain)
    {
        if (this.pain + pain >= 100)
        {

        }
        else
        {
            this.pain += pain;
        }
    }

    public void GameOver()
    {

    }
}
