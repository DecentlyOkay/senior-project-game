using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter_Enemy : Enemy
{
    public Enemy spawnPrefab;
    public int spawnCount = 4;
    public void FixedUpdate()
    {
        MoveTowardsTarget();
        base.FixedUpdate();
    }
    public override void Die()
    {
        base.Die();
        
        Destroy(this.gameObject);
        for(int i = 0; i < spawnCount; i++)
        {
            Enemy enemy = Instantiate(spawnPrefab, this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.identity);
            enemy.ApplyForce(new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 5f), Random.Range(-5f, 5f)));
        }
        
    }
}
