using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum MonsterType
    {
        NormalMonster,
        BossMonster
    }

    [Header("Monster Status")]
    [SerializeField] MonsterType monsterType;
    [SerializeField] float hp;
    [SerializeField] float speed;
    [SerializeField] int score;
    public float painDamage;

    [Header("Bullet Status")]
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletFireRate;
    float bulletReloadTime;

    public GameObject MonsterBullet;
    GameObject Player;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        Move();
        Fire();
    }

    void Move()
    {
        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
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
        Destroy(gameObject);
    }

    void Fire()
    {
        if (bulletReloadTime > 0)
        {
            bulletReloadTime -= Time.deltaTime;
        }

        if (bulletReloadTime <= 0)
        {
            Vector3 dir = Player.transform.position - transform.position;

            Instantiate(MonsterBullet, gameObject.transform.position, Quaternion.Euler(dir.normalized)).GetComponent<MonsterBullet>().SetBulletStatus(bulletDamage, bulletSpeed);

            bulletReloadTime = bulletFireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
