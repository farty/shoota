using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth;

    void Start()
    {
        {
            curHealth = maxHealth;
        }
    }
    void Update()
    {
        Die();

    }

    void Die()
    {
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (gameObject.GetComponent<Enemy>())
        {
            gameObject.GetComponent<Enemy>().playerPositioinIdentified = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(collision.gameObject.GetComponent<Bullet>().bulletDamage);
        }
    }

}
