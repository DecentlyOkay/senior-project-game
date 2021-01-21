using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    protected Vector3 direction;

    void FixedUpdate()
    {
        this.transform.Translate(direction * speed);
    }

    public void FireProjectile (Ray rayDirection)
    {
        this.direction = rayDirection.direction;
        this.transform.position = rayDirection.origin;
    }

    public void OnCollisionEnter()
    {
        Destroy(this);
    }
}
