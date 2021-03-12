using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 10f;
    public float health = 10f;
    public float maxStamina = 10f;
    public float stamina = 10f;
    public float staminaRegenRate = 1f; //per second

    private bool isDead = false;

    private void Start()
    {
        health = maxHealth;
    }

    public void FixedUpdate()
    {
        stamina += Time.fixedDeltaTime * staminaRegenRate;
        stamina = Mathf.Min(stamina, maxStamina);
    }
    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(!enemy.isDead)
            {
                ApplyDamage(enemy.damage * Time.deltaTime);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Void"))
        {
            Die();
        }
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }
    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        health = 0f;
        Debug.Log("You died");
        Destroy(this.gameObject);
        //Disable pause menu when you dead
        GameObject.FindObjectOfType<PauseMenu>().gameObject.SetActive(false);
        //Show death menu instead
        GameObject.FindObjectOfType<DeathMenu>().Display();
    }
}
