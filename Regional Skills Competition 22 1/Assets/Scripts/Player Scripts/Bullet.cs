using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    float lifeTime;


    private void Awake()
    {
        lifeTime = 2f;
    }
    void Update()
    {
        Move();
        LifeTime();
    }

    private void Move()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    private void LifeTime()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletStatus(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<Monster>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossMonster>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
