using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private int score = 0;
    [SerializeField] private float pain;

    public GameObject[] MonsterObjects;

    public GameObject BackGround;

    float spawnDelay;

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
        BackGroundMove();
    }

    void SpawnMonster()
    {
        if (spawnDelay > 0)
        {
            spawnDelay -= Time.deltaTime;
        }

        if (spawnDelay <= 0)
        {
            int ranMonster = Random.Range(0, 3);

            Instantiate(MonsterObjects[ranMonster], new Vector3(Random.Range(-8.0f, 8.0f), 6, 0), Quaternion.identity);
            spawnDelay = 2f;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void BackGroundMove()
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
        if (this.pain <= pain)
        {

        }
        else
        {
            pain -= pain;
        }
    }
}
