using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser_Enemy : Enemy
{
    public override void FixedUpdate()
    {
        MoveTowardsTarget();
        base.FixedUpdate();
        Vector3 horizontalVelocity = rigidbody.velocity;
        horizontalVelocity.y = 0;
        Debug.Log(isGrounded + " " + horizontalVelocity.magnitude);
        if(horizontalVelocity.magnitude > 20)
        {
            horizontalVelocity = horizontalVelocity.normalized * 20;
            rigidbody.velocity = horizontalVelocity + new Vector3(0, rigidbody.velocity.y, 0);
        }

    }

    public override void MoveTowardsTarget()
    {
        if (target == null || isDead)
            return;
        Vector3 targetMovement = target.GetComponent<PlayerMovement>().totalMoveDirection;
        Vector3 forceDirection = target.position - this.transform.position;
        if(forceDirection.magnitude > 10f)
        {
            Debug.Log("correcting course");
            forceDirection += targetMovement;
        }
        forceDirection.y = 0;
        forceDirection = forceDirection.normalized;
        if (isGrounded)
        {
            ApplyForce(forceDirection * speed);
        }
        else
        {
            ApplyForce(forceDirection * speed * 0.2f);
        }
    }
}
