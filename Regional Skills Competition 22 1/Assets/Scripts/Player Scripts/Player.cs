using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] float hp;
    [SerializeField] float speed;
    [SerializeField] bool isDie;

    [Header("Bullet Status")]
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifeTime;

    public Camera camera;
    public GameObject NormalBullet;
    public GameObject PlayerCircle;

    void Update()
    {
        Movement();
        Fire();
        PlayerRotate();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed * 0.01f;
        float y = Input.GetAxisRaw("Vertical") * speed * 0.01f;

        gameObject.transform.Translate(x, y, 0);
    }

    void PlayerRotate()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position;
        PlayerCircle.transform.up = dirVec.normalized;
    }

    void TakeDamage(float damage)
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
        isDie = true;
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(NormalBullet, transform.position, PlayerCircle.transform.rotation).GetComponent<Bullet>().SetBulletStats(bulletDamage, bulletSpeed, bulletLifeTime);
        }
    }
}
