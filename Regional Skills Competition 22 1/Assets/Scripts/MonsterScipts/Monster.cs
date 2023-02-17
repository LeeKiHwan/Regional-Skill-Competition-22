using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [Header("Monster Status")]
    [SerializeField] float hp;
    [SerializeField] float speed;
    [SerializeField] int score;

    [Header("Bullet Status")]
    public float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletFireRate;
    float bulletReloadTime;

    public GameObject MonsterBullet;
    GameObject Player;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
            Vector2 dir = (Vector2)Player.transform.position - (Vector2)transform.position;

            MonsterBullet.transform.up = dir.normalized;
            MonsterBullet.transform.position = transform.position;

            Instantiate(MonsterBullet).GetComponent<MonsterBullet>().SetBulletStatus(bulletDamage, bulletSpeed);

            bulletReloadTime = bulletFireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(bulletDamage * 0.5f);
        }
    }
}
