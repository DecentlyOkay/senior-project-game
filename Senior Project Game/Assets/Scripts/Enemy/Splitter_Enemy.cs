using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter_Enemy : Enemy
{
    public Enemy spawnPrefab;
    public int spawnCount = 4;
    public override void FixedUpdate()
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
            Enemy enemy = Instantiate(spawnPrefab, this.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-0.5f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
            enemy.ApplyForce(new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 10f), Random.Range(-10f, 10f)));
        }
        
    }
}
