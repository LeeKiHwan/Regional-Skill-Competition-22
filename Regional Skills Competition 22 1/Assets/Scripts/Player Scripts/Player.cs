using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] float hp;
    [SerializeField] float speed;
    public bool invincibility;

    [Header("Bullet Status")]
    [SerializeField] GameObject[] Bullet;
    [SerializeField] int bulletLevel;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletFireRate;
    float bulletReloadTime;

    void Update()
    {
        Movement();
        Fire();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        transform.Translate(x, y, 0);
    }

    public void TakeDamage(float damage)
    {
        if (!invincibility)
        {
            if (hp <= damage)
            {
                Die();
            }
            else
            {
                hp -= damage;
                StartCoroutine(HitInvincibility());
            }
        }
    }

    IEnumerator HitInvincibility()
    {
        invincibility = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(1.5f);

        invincibility = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield break;
    }

    IEnumerator ItemInvincibility()
    {
        invincibility = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(3f);

        invincibility = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield break;
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }

    void Fire()
    {
        if (bulletReloadTime > 0)
        {
            bulletReloadTime -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Mouse0) && bulletReloadTime <= 0)
        {
            Instantiate(Bullet[bulletLevel], transform.position, transform.rotation).GetComponent<Bullet>().SetBulletStatus(bulletDamage, bulletSpeed);
            bulletReloadTime = bulletFireRate;
        }
    }

    public void UseItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.BulletUpgrade:
                bulletLevel++;
                bulletDamage += 10f;
                break;

            case ItemType.InvincibilityOn:
                StopCoroutine(HitInvincibility());
                StopCoroutine(ItemInvincibility());
                StartCoroutine(ItemInvincibility());
                break;

            case ItemType.HpUp:
                hp += 10;
                break;

            case ItemType.PainDown:
                GameManager.Instance.TakePain(-10);
                break;

            case ItemType.BulletSpeed:
                bulletSpeed += 0.5f;
                break;

            case ItemType.BulletFireRate:
                bulletFireRate += 0.1f;
                break;
        }
    }
}
