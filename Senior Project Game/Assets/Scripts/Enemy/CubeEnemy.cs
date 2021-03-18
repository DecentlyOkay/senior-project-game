using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
    public override void FixedUpdate()
    {
        MoveTowardsTarget();
        base.FixedUpdate();
    }
}
