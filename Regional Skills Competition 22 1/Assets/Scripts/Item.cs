using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BulletUpgrade,
    InvincibilityOn,
    HpUp,
    PainDown,
    BulletSpeed,
    BulletFireRate
}

public class Item : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] ItemType itemType;
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().UseItem(itemType);
            GameManager.Instance.AddScore(30);
            Destroy(gameObject);
        }
    }
}
