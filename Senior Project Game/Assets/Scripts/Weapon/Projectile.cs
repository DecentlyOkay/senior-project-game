using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 1f;
    public float knockback = 0f;

    protected Vector3 direction;

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
    public void FireProjectile(Ray rayDirection, Vector3 spread)
    {
        FireProjectile(rayDirection);
        AddSpread(spread);
    }
    private void RotateInShootDirection()
    {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, direction, (float)(2*System.Math.PI), 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    private void AddSpread(Vector3 spread)
    {
        transform.Rotate(spread);
        this.direction = transform.forward;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with " + collision.gameObject);
        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy")) {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ApplyForce(this.direction.normalized * knockback);

            //For testing
            //enemy.ApplyForce(new Vector3(0, 100, 0));

            enemy.ApplyDamage(damage);
            Debug.Log("Enemy health: " + enemy.health);
        }
        Destroy(this.gameObject);
    }

}
