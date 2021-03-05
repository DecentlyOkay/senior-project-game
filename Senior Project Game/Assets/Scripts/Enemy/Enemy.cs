using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float maxHealth = 1f;
    public float health = 1f;
    public float damage = 1f;
    public float forceFallOffFactor = 0.5f;


    public float groundDistance = 0.4f;
    private LayerMask groundMask;
    private LayerMask enemyMask;
    public bool isGrounded;

    //Corpse will disappear after an additional corpseResilience proportion of health is dealt as damage, when dead, corpse takes %health as dmg per tick
    //Idea: shootup corpses for extra points
    public float corpseResilience = 0.2f;

    private MeshRenderer meshRenderer;
    private Color deadColor = Color.Lerp(Color.grey, Color.white, 0.75f);
    private Color healthyColor;
    public float dissolveSpeed = 0.5f;
    private float dissolveTimeElapsed = 0;
    public bool isDead = false;
    private bool isDissolving = false;

    public Transform target;
    private Vector3 forces;
    public Rigidbody rigidbody;

    private GameController gameController;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        this.tag = "Enemy";
        meshRenderer = this.GetComponent<MeshRenderer>();
        healthyColor = meshRenderer.material.color;
        groundMask = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
    }

    public void Start()
    {
        GameObject t = GameObject.FindGameObjectWithTag("Player");
        if(t != null)
        {
            target = t.transform;
        }
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.enemiesRemaining++;
    }
    public void FixedUpdate()
    {
        Vector3 groundCheck = this.transform.position;
        groundCheck.y -= this.transform.localScale.y / 2;
        isGrounded = Physics.CheckSphere(groundCheck, groundDistance * transform.localScale.y, groundMask);

        rigidbody.AddForce(forces, ForceMode.VelocityChange);
        forces *= forceFallOffFactor;
        if (isDead)
        {
            forces *= 0.1f;
        }
    }
    public void Update()
    {
        if(isDissolving)
        {
            Dissolve();
        }
        else if (isDead)
        {
            health -= maxHealth * corpseResilience * Time.deltaTime;
            if (health <= -maxHealth * corpseResilience)
            {
                Dissolve();
            }
        }
    }
    public virtual void MoveTowardsTarget()
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
            horizontalMove = moveDirection * speed * 0.5f - rigidbody.velocity;
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
            //Corpse is destroyed (begin dissolving sequence)
            else if(health <= -maxHealth * corpseResilience)
            {
                Dissolve();
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
        gameController.enemiesRemaining--;
    }

    private void UpdateColor(Color c)
    {
        meshRenderer.material.color = c;
    }
    private void Dissolve()
    {
        isDissolving = true;

        Material[] mats = meshRenderer.materials;

        mats[0].SetFloat("_Cutoff", Mathf.Sin(dissolveTimeElapsed * dissolveSpeed));
        dissolveTimeElapsed += Time.deltaTime;
        // Unity does not allow meshRenderer.materials[0]...
        meshRenderer.materials = mats;

        if(dissolveTimeElapsed >= dissolveSpeed)
        {
            Debug.Log("destroying enemy");
            Destroy(this.gameObject);
        }
    }
}
