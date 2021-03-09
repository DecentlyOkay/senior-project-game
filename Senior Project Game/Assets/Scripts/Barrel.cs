using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Enemy
{

    private Explodable exploder;

    protected override void Awake()
    {
        base.Awake();
        //Barrels won't count as enemies you need to clear to continue
        gameController.enemiesRemaining--;
        exploder = this.gameObject.GetComponent<Explodable>();
    }
    public override void Die()
    {
        isDead = true;
        UpdateColor(deadColor);
        health = 0;
        exploder.Explode();
    }
}
