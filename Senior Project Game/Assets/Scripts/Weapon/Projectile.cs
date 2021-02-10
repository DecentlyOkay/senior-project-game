using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 1f;
    public float knockback = 0f;

    private Vector3 direction;

    void FixedUpdate()
    {
        this.transform.Translate(direction * speed, Space.World);
    }

    public void FireProjectile (Ray rayDirection)
    {
        this.direction = rayDirection.direction;
        this.transform.position = rayDirection.origin;
        RotateInShootDirection();
    }
    void RotateInShootDirection()
    {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, direction, (float)(2*System.Math.PI), 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy")) {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.AddForce(this.direction.normalized * knockback);
            enemy.ApplyDamage(damage);
            Debug.Log("Enemy health: " + enemy.health);
        }
        Destroy(this.gameObject);
    }

}
