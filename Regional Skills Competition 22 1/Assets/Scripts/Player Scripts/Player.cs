using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
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
        CheatKey();
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
            GameManager.Instance.TakeDamage(damage);
            StartCoroutine(HitInvincibility());
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

        yield return new WaitForSeconds(2.5f);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.5f);
        invincibility = false;

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
                if (bulletLevel < 5)
                {
                    bulletLevel++;
                    bulletDamage += 5;
                }
                break;

            case ItemType.InvincibilityOn:
                StopCoroutine(HitInvincibility());
                StopCoroutine(ItemInvincibility());
                StartCoroutine(ItemInvincibility());
                break;

            case ItemType.HpUp:
                GameManager.Instance.TakeDamage(-10);
                break;

            case ItemType.PainDown:
                GameManager.Instance.TakePain(-10);
                break;

            case ItemType.BulletSpeed:
                bulletSpeed += 0.75f;
                break;

            case ItemType.BulletFireRate:
                bulletFireRate -= 0.025f;
                break;
        }
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (bulletLevel > 0)
            {
                bulletLevel--;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (bulletLevel < 4)
            {
                bulletLevel++;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            invincibility = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            invincibility = false;
        }
    }
}
