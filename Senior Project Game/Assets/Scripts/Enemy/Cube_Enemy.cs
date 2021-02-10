using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Enemy : Enemy
{
    public void FixedUpdate()
    {
        base.FixedUpdate();
        MoveTowardsTarget();
    }
}
