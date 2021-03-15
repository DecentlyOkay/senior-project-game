using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{
    public float radius = 10f;
    public float power = 10f; //knockback
    public float upwardForce = 0f; //knockup
    public float damage = 10f;
    

    public ParticleSystem explosionPrefab;
    public void Explode()
    {
        Destroy(this.gameObject);
        CreateExplosion();
    }

    public void CreateExplosion()
    {
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        Debug.Log("exploding");
        //Will player get double processed because of its two colliders? Yes
        bool playerSeen = false;
        foreach (Collider other in Physics.OverlapSphere(this.transform.position, radius))
        {
            Debug.Log(other + " " + other.gameObject);
            Vector3 point = other.ClosestPoint(this.transform.position);

            Debug.DrawRay(this.transform.position, new Vector3(radius, 0, 0), Color.blue, 2f);
            Debug.DrawRay(this.transform.position, new Vector3(-radius, 0, 0), Color.blue, 2f);
            Debug.DrawRay(this.transform.position, new Vector3(0, 0, radius), Color.blue, 2f);
            Debug.DrawRay(this.transform.position, new Vector3(0, 0, -radius), Color.blue, 2f);

            float damage = GetDamage(point);
            Vector3 force = GetForce(point);
            if (!playerSeen && other.gameObject.CompareTag("Player"))
            {
                playerSeen = true;
                other.gameObject.GetComponent<PlayerMovement>().ApplyForce(force);
                other.gameObject.GetComponent<Player>().ApplyDamage(damage);
                Debug.Log("Player took " + damage + " damage");
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().ApplyForce(force);
                other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
                Debug.Log("Enemy took " + damage + " damage");
            }
        }

    }

    public Vector3 GetForce(Vector3 point)
    {
        float distance = (this.transform.position - point).magnitude;
        Debug.Log(this.transform.position + " " + point + " " + distance);
        //Knockback goes linearly from 1 at point blank to 0.25 at max range
        float distMultiplier = Mathf.Max((radius - distance / 1.33f) / radius, 0);
        Debug.Log("dist multiplier " + distMultiplier);
        return (point - this.transform.position).normalized * power * distMultiplier + Vector3.up * upwardForce * distMultiplier;
    }
    
    public float GetDamage(Vector3 point)
    {
        float distance = (this.transform.position - point).magnitude;
        //Damage goes linearly from 1 at point blank to 0 at max range
        return Mathf.Max((radius - distance) / radius * damage, 0);
    }
}
