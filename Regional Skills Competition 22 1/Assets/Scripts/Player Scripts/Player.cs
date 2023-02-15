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
    [SerializeField] float bulletFireRate;
    float bulletReloadTime;

    public GameObject NormalBullet;

    void Update()
    {
        Movement();
        Fire();
    }

    void Movement()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -8.5f && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 8.5 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 4.6f && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -4.6f)
        {
            gameObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);   
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
        isDie = true;
    }

    void Fire()
    {
        if (bulletReloadTime > 0)
        {
            bulletReloadTime -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Mouse0) && bulletReloadTime <= 0)
        {
            Instantiate(NormalBullet, transform.position, transform.rotation).GetComponent<Bullet>().SetBulletStatus(bulletDamage, bulletSpeed);
            bulletReloadTime = bulletFireRate;
        }
    }
}
