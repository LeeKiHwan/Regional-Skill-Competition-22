using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBloodCell : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject[] ItemList;

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
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerBullet"))
        {
            int randItem = Random.Range(0, ItemList.Length);

            Instantiate(ItemList[randItem], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
