using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Game Status")]
    [SerializeField] private int score = 0;
    [SerializeField] private float hp;
    [SerializeField] private float pain;
    [SerializeField] bool isGameover;
    [SerializeField] int currentStage;
    [SerializeField] int difficulty; // 0:Normal   1:Hard
    [SerializeField] bool MonsterSpawnable;

    [Header("Game Objects")]
    public GameObject Player;
    public GameObject[] MonsterObjects;
    public GameObject BossObjects;
    public GameObject ItemObject;
    public GameObject RedBloodCell;
    public GameObject BackGround;

    [Header("UI")]
    public Slider PainSlider;
    public Slider HpSlider;
    public TextMeshProUGUI ScoreText;
    public Slider BossHp;
    public TMP_InputField NameInputField;
    public GameObject GameResultUI;

    float monsterSpawnDelay = 2f;
    float itemSpawnDelay = 10f;
    float redBloodCellSpawnDelay = 15f;

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
        SpawnRedBloodCell();
        BackGroundMove();
        UICheck();
        CheatKey();
    }

    private void SpawnMonster()
    {
        if (MonsterSpawnable && monsterSpawnDelay <= 0)
        {
            int ranMonster = Random.Range(0, 3);
            Instantiate(MonsterObjects[ranMonster], new Vector3(Random.Range(-8.0f, 8.0f), 6, 0), Quaternion.identity);

            if (difficulty == 1)
            {
                if (currentStage == 1) monsterSpawnDelay = 4f;
                else if (currentStage == 2) monsterSpawnDelay = 2f;
            }

            else if (difficulty == 2)
            {
                if (currentStage == 1) monsterSpawnDelay = 2.5f;
                else if (currentStage == 2) monsterSpawnDelay = 1f;
            }
        }
        else if (MonsterSpawnable && monsterSpawnDelay > 0)
        {
            monsterSpawnDelay -= Time.deltaTime;
        }
    }

    private void SpawnItem()
    {
        if (currentStage == 1 || currentStage == 2)
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
    }

    private void SpawnRedBloodCell()
    {
        if (currentStage ==1 || currentStage == 2)
        {
            if (redBloodCellSpawnDelay > 0)
            {
                redBloodCellSpawnDelay -= Time.deltaTime;
            }
            else
            {
                Instantiate(RedBloodCell, new Vector3(-10, Random.Range(0f, -3.5f), 0),Quaternion.identity);
                redBloodCellSpawnDelay = 15f;
            }
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
            GameOver();
        }
        else if (this.pain + pain <= 0)
        {
            this.pain = 0;
        }
        else
        {
            this.pain += pain;
        }
    }

    public void TakeDamage(float damage)
    {
        if (hp <= damage)
        {
            GameOver();
        }
        else if (hp - damage >= 100)
        {
            hp = 100f;
        }
        else
        {
            hp -= damage;
        }
    }

    private void UICheck()
    {
        PainSlider.value = pain / 100f;

        HpSlider.value = hp / 100f;

        ScoreText.text = "Score : " + score.ToString();
    }
    
    public void SecondStageOn()
    {
        StartCoroutine(SecondStageStart());
    }

    public IEnumerator FirstStageStart()
    {
        if (GameObject.FindGameObjectWithTag("Player")) Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(Player, new Vector3(0, -4f, 0), Quaternion.identity);
        MonsterSpawnable = true;

        currentStage = 1;

        yield return new WaitForSeconds(40);

        SpawnBoss();

        yield break;
    }

    public IEnumerator SecondStageStart()
    {
        MonsterSpawnable = true;
        currentStage = 2;

        yield return new WaitForSeconds (40);

        SpawnBoss();

        yield break;
    }

    public IEnumerator MonsterSpawnableActive()
    {
        MonsterSpawnable = true;

        yield return new WaitForSeconds(15);

        MonsterSpawnable = false;

        yield break;
    }

    private void SpawnBoss()
    {
        MonsterSpawnable = false;
        if (currentStage == 1)
        {
            if (difficulty == 1) Instantiate(BossObjects, new Vector3(0, 8, 0), Quaternion.identity).GetComponent<BossMonster>().SetBossStatus(2500, 1000, 7.5f, 10, 5, 1);
            else if (difficulty == 2) Instantiate(BossObjects, new Vector3(0, 8, 0), Quaternion.identity).GetComponent<BossMonster>().SetBossStatus(4000, 2000, 5, 15, 7.5f, 1);
        }
        if (currentStage == 2)
        {
            if (difficulty == 1) Instantiate(BossObjects, new Vector3(0, 8, 0), Quaternion.identity).GetComponent<BossMonster>().SetBossStatus(4000, 2000, 5, 15, 7.5f, 2);
            else if (difficulty == 2) Instantiate(BossObjects, new Vector3(0, 8, 0), Quaternion.identity).GetComponent<BossMonster>().SetBossStatus(8000, 4000, 3, 25, 9, 2);
        }
    }

    public void DifficultySelect(int difficulty)
    {
        this.difficulty = difficulty;
        StartCoroutine(FirstStageStart());
    }

    public void GameOver()
    {

    }

    public void GameClear()
    {
        MonsterSpawnable = false;
        Destroy(GameObject.FindGameObjectWithTag("Monster"));

        score += (int)hp * 10;
        score += (100 - (int)pain) * 10;

        PainSlider.gameObject.SetActive(false);
        HpSlider.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            StartCoroutine(FirstStageStart());
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartCoroutine(SecondStageStart());
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Destroy(GameObject.FindGameObjectWithTag("Monster"));
            Destroy(GameObject.FindGameObjectWithTag("Boss"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {

        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {

        }
    }
}
