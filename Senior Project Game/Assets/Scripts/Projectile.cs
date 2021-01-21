using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

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
    void RotateInShootDirection()
    {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, direction, (float)(2*System.Math.PI), 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("b");
        Destroy(this.gameObject);
    }
}
