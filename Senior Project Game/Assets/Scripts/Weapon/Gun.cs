using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Projectile projectilePrefab;
    public float recoil = 0f;

    private PlayerMovement player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }
    public override void Attack()
    {
        RaycastHit mouseLoc = player.RayCastToMouse(LayerMask.GetMask("Ground"));
        if(mouseLoc.collider != null)
        {
            Vector3 point = mouseLoc.point;

            if (player.isGrounded)
            {
                //If you are shooting somewhere lower than your feet, you will shoot at the point + 1/4 player model height
                if (point.y < player.groundCheck.position.y - 0.05f)
                {
                    point.y += (player.transform.position.y - player.groundCheck.position.y) / 2f;
                }
                
                else if (point.y > player.groundCheck.position.y + 0.05f)
                {
                    point.y += (player.transform.position.y - player.groundCheck.position.y);
                }
                //Will shoot straight when grounded and aiming at player level, will want to add to this later (see above) if you want to
                //aim at higher places on walls while grounded
                //Will also want to allow you to shoot at enemies when raycast hits them
                else
                {
                    point.y = player.transform.position.y;
                }
            }
            
            Shoot(point);
            player.ApplyForce(-(point - this.transform.position).normalized * recoil);
        }
    }

    //Idea for making aiming smarter. i.e. when aiming up ramps and when aiming at floor if cursor is really close to player
    //Raycast from gun and check distance, if distance is small enough, then just shoot on player's y level/hit location + player's y scale
    //else just shoot at hit location (where mouse is)
    //Actually might just want to universally add the player y scale offset to hit location

    private void Shoot(Vector3 point)
    {
        Projectile projectile = Instantiate(projectilePrefab);

        //Vector3 pointAboveFloor = hit.point + new Vector3(0, this.transform.position.y, 0);
        //Vector3 direction = pointAboveFloor - this.transform.position;
        Vector3 direction = point - this.transform.position;

        Ray shootRay = new Ray(this.transform.position, direction);
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100f, Color.green, 1);
        //Don't need this anymore, as bullet layer will not collide with player layer
        //foreach (Collider collider in player.GetComponents<Collider>())
        //{
        //    Physics.IgnoreCollision(collider, projectile.GetComponent<Collider>());
        //}
        projectile.FireProjectile(shootRay);
    }
}
