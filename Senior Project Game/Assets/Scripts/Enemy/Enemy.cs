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
    public float forceFallOffFactor = 0.5f;

    public float groundDistance = 0.4f;
    private LayerMask groundMask;
    private LayerMask enemyMask;
    public bool isGrounded;

    //Corpse will disappear after deathTimer seconds, or after an additional corpseResilience proportion of health is dealt as damage
    //Idea: shootup corpses for extra points
    public float deathTimer = 5f;
    public float corpseResilience = 0.2f;

    private Renderer renderer;
    private Color deadColor = Color.Lerp(Color.grey, Color.white, 0.75f);
    private Color healthyColor;
    public bool isDead = false;
    private Transform target;
    private Vector3 forces;
    private Rigidbody rigidbody;

    public void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        this.tag = "Enemy";
        target = GameObject.FindGameObjectWithTag("Player").transform;
        renderer = this.GetComponent<Renderer>();
        healthyColor = renderer.material.color;

        //Both enemies and the ground count as ground mask for grounded purposes
        groundMask = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
    }
    public void FixedUpdate()
    {
        Vector3 groundCheck = this.transform.position;
        groundCheck.y -= this.transform.localScale.y / 2;
        isGrounded = Physics.CheckSphere(groundCheck, groundDistance * transform.localScale.y, groundMask | enemyMask);

        rigidbody.AddForce(forces, ForceMode.VelocityChange);
        forces *= forceFallOffFactor;
        if (isDead)
        {
            forces *= 0.1f;
        }
    }
    public void Update()
    {
        if (isDead)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void MoveTowardsTarget()
    {
        if (target == null || isDead)
            return;
        Vector3 moveDirection = (target.position - this.transform.position);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;
        Vector3 horizontalMove = moveDirection * speed - rigidbody.velocity;
        
        //Move slower in the air
        if (!isGrounded)
        {
            horizontalMove = moveDirection * speed * 0.4f - rigidbody.velocity;
        }
        horizontalMove.y = 0;
        rigidbody.AddForce(horizontalMove, ForceMode.VelocityChange);
    }
    public void ApplyForce(Vector3 force)
    {
        forces.x += force.x;
        forces.z += force.z;
        rigidbody.AddForce(0, force.y, 0, ForceMode.Impulse);
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            //Just died
            if (!isDead)
            {
                Die();
            }
            //Corpse is destroyed
            else if(health <= -maxHealth * corpseResilience)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            UpdateColor(Color.Lerp(deadColor, healthyColor, health / maxHealth));
        }
    }
    public virtual void Die()
    {
        isDead = true;
        UpdateColor(deadColor);
        health = 0;
    }

    private void UpdateColor(Color c)
    {
        renderer.material.color = c;
    }


}
