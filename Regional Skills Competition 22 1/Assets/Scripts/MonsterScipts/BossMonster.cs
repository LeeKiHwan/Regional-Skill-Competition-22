using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour
{
    GameObject Player;

    [Header("Monster Status")]
    [SerializeField] float maxHp;
    [SerializeField] float hp;
    [SerializeField] int score;
    [SerializeField] float skillCoolTime;
    [SerializeField] int stage;

    [Header("Bullet Status")]
    [SerializeField] GameObject BulletObject;
    public float bulletDamage;
    [SerializeField] float bulletSpeed;

    float skillNumber;
    public Slider HpSlider;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        HpSlider = GameManager.Instance.BossHp;
    }

    private void Start()
    {
        maxHp = hp;
        HpSlider.gameObject.SetActive(true);
    }



    private void Update()
    {
        SkillLogic();
        HpValue();

        if (transform.position.y > 3)
        {
            transform.Translate(new Vector3(0, -2 * Time.deltaTime, 0));
        }
    }

    private void HpValue()
    {
        HpSlider.value = hp / maxHp;
    }

    private void SkillLogic()
    {
        if (skillCoolTime > 0)
        {
            skillCoolTime -= Time.deltaTime;
        }
        else
        {
            switch(skillNumber)
            {
                case 0:
                    StartCoroutine(BurstShoting());
                    break;
                case 1:
                    StartCoroutine(ShotgunShoting());
                    break;
                case 2:
                    MonsterSpawn();
                    break;
            }
            skillCoolTime = 10f;
        }
    }

    IEnumerator BurstShoting()
    {
        int cnt = 0;

        while (cnt < 15)
        {
            Vector2 dir = (Vector2)Player.transform.position - (Vector2)transform.position;
            BulletObject.transform.up = dir.normalized;
            BulletObject.transform.position = transform.position;

            Instantiate(BulletObject).GetComponent<MonsterBullet>().SetBulletStatus(bulletDamage, bulletSpeed);

            cnt++;

            yield return new WaitForSeconds(0.25f);
        }

        skillNumber++;
        yield break;
    }

    IEnumerator ShotgunShoting()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                Vector2 dir = (Vector2)Player.transform.position - (Vector2)transform.position;
                Vector2 ranVec = new Vector2(Random.Range(-8f, 8f), 0);

                dir += ranVec;

                BulletObject.transform.up = dir.normalized;
                BulletObject.transform.position = transform.position;

                Instantiate(BulletObject).GetComponent<MonsterBullet>().SetBulletStatus(bulletDamage, bulletSpeed);
            }
            yield return new WaitForSeconds(0.75f);
        }

        skillNumber++;

        yield break;
    }

    private void MonsterSpawn()
    {
        StartCoroutine(GameManager.Instance.MonsterSpawnableActive());
        skillNumber = 0;
    }

    public void SetBossStatus(float hp, int score, float skillCoolTime, float bulletDamage, float bulletSpeed, int stage)
    {
        this.hp = hp;
        this.score = score;
        this.skillCoolTime = skillCoolTime;
        this.bulletDamage = bulletDamage;
        this.bulletSpeed = bulletSpeed;
        this.stage = stage;
    }

    public void TakeDamage(float damage)
    {
        if (hp <= damage)
        {
            Die();
        }
        else
        {
            hp -= damage;
        }
    }

    private void Die()
    {
        GameManager.Instance.AddScore(score);
        if (stage == 1)
        {
            GameManager.Instance.SecondStageOn();
        }
        else if (stage == 2)
        {
            GameManager.Instance.GameClear();
        }
        HpSlider.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(bulletDamage * 0.5f);
        }
    }
}
