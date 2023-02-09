using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    enum MonsterType
    {
        NormalMonster,
        BossMonster
    }

    [Header("Monster Status")]
    [SerializeField] MonsterType monsterType;
    [SerializeField] float hp;
    [SerializeField] float damage;
    [SerializeField] int score;


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
}
