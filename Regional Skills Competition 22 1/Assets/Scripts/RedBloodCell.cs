using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float hp;
    [SerializeField] float lifeTime;

    private void Update()
    {
        Move();
        LifeTime();
    }

    private void Move()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    void LifeTime()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
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

    void Die()
    {
        GameManager.Instance.TakePain(10);
        Destroy(gameObject);
    }
}
