using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float maxHealth = 1f;
    public float health = 1f;
    public float damage = 1f;
    public float nextContactDamageTime = 0f;
    public float timeBetweenContactDamage = 1f;

    private Transform target;


    private Rigidbody rigidbody;

    public void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        this.tag = "Enemy";
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void MoveTowardsTarget()
    {
        if (target == null)
            return;
        Vector3 moveDirection = (target.position - this.transform.position);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;
        Vector3 horizontalMove = moveDirection * speed - rigidbody.velocity;
        horizontalMove.y = 0;
        rigidbody.AddForce(horizontalMove, ForceMode.VelocityChange);
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
        //Replace with ragdoll
    }

}
