using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    public float distToShoot = 25f;
    public float distToMove = 25f;
    public int barrageLength = 5;
    public float timeBetweenBullets = 0.2f;
    public float barrageCoolDown = 2f;
    public EnemyProjectile enemyProjectilePrefab;
    public Vector3 spread = Vector3.zero;


    private bool isAttacking;
    public override void FixedUpdate()
    { 
        base.FixedUpdate();
        if (target == null || isDead)
        {
            return;
        }
        Vector3 playerHorizontal = target.position;
        playerHorizontal.y = 0;
        Vector3 lookPoint = playerHorizontal;
        lookPoint.y = this.transform.position.y;
        this.transform.LookAt(lookPoint);
        Vector3 thisHorizontal = this.transform.position;
        thisHorizontal.y = 0;
        if(!isAttacking)
        {
            if ((playerHorizontal - thisHorizontal).magnitude > distToMove)
            {
                MoveTowardsTarget();
            }
            else
            {
                StartCoroutine(ShootBarrage());
            }
        }
        else
        {
            //This is so it doesn't take lots of knockback when standing still
            Vector3 horizontalVelocity = this.rigidbody.velocity;
            horizontalVelocity.y = 0;
            horizontalVelocity *= 0.9f;
            this.rigidbody.velocity = new Vector3(horizontalVelocity.x, this.rigidbody.velocity.y, horizontalVelocity.z);
        }
        
    }

    private IEnumerator ShootBarrage()
    {
        isAttacking = true;
        for(int i = 0; i < barrageLength; i++)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenBullets);
        }
        yield return new WaitForSeconds(barrageCoolDown);
        isAttacking = false;
    }

    private void Shoot()
    {
        if (target == null || isDead)
        {
            return;
        }
        EnemyProjectile projectile = Instantiate(enemyProjectilePrefab);
        Vector3 futureMoveVector = target.gameObject.GetComponent<PlayerMovement>().moveDirection;
        //Slight leading of shots by a random amount
        Vector3 direction = target.position + Mathf.Lerp(0, 4f, Random.value) * futureMoveVector - this.transform.position;
        Ray shootRay = new Ray(this.transform.position, direction);
        projectile.FireProjectile(shootRay);
    }

    private Vector3 GetSpreadAngle()
    {
        return new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z));
    }
}
