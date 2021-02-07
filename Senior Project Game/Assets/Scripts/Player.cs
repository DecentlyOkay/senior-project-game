using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 10f;
    public float health = 10f;

    public void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy.nextContactDamageTime <= Time.time)
            {
                enemy.nextContactDamageTime = Time.time + enemy.timeBetweenContactDamage;
                ApplyDamage(enemy.damage);
                Debug.Log("Player health: " + health);
            }
        }
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("You died");
        Destroy(this.gameObject);
    }
}
