using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    Explodable exploder;

    private void Awake()
    {
        exploder = this.gameObject.GetComponent<Explodable>();
    }
    public override void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with " + collision.gameObject);
        GameObject other = collision.gameObject;
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ApplyForce(this.direction.normalized * knockback);

            //For testing
            //enemy.ApplyForce(new Vector3(0, 100, 0));

            enemy.ApplyDamage(damage);
            Debug.Log("Enemy health: " + enemy.health);
        }
        exploder.Explode();
    }
}
