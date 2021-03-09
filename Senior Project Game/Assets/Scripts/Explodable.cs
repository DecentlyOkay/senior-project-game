using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{
    public float radius = 2f;
    public float power = 10f; //knockback
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
                other.gameObject.GetComponent<Player>().ApplyDamage(damage);
                other.gameObject.GetComponent<PlayerMovement>().ApplyForce(force);
                Debug.Log("Player took " + damage + " damage");
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().ApplyDamage(damage);
                other.gameObject.GetComponent<Enemy>().ApplyForce(force);
                Debug.Log("Enemy took " + damage + " damage");
            }
        }
            
    }

    public float GetDamage(Vector3 point)
    {
        float distance = (this.transform.position - point).magnitude;
        return Mathf.Max((radius - distance) / radius * damage, 0);
    }

    public Vector3 GetForce(Vector3 point)
    {
        float distance = (this.transform.position - point).magnitude;
        return (point - this.transform.position).normalized * Mathf.Max((radius - distance) / radius * power, 0);
    }
}
