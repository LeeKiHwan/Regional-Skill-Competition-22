using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            float painDamage = collision.GetComponent<Monster>().bulletDamage * 0.5f;

            GameManager.Instance.TakePain(painDamage);

            Destroy(collision.gameObject);
        }
    }
}
