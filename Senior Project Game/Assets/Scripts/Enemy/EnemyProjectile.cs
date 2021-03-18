using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 1f;
    public float knockback = 0f;

    protected Vector3 direction;

    void FixedUpdate()
    {
        this.transform.Translate(direction * speed, Space.World);
    }

    public void FireProjectile(Ray rayDirection)
    {
        this.direction = rayDirection.direction;
        this.transform.position = rayDirection.origin;
        RotateInShootDirection();
    }
    public void FireProjectile(Ray rayDirection, Vector3 spread)
    {
        FireProjectile(rayDirection);
        AddSpread(spread);
    }
    private void RotateInShootDirection()
    {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, direction, (float)(2 * System.Math.PI), 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    private void AddSpread(Vector3 spread)
    {
        transform.Rotate(spread);
        this.direction = transform.forward;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.ApplyForce(this.direction.normalized * knockback);
            player.ApplyDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
