using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Projectile projectilePrefab;

    private PlayerMovement player;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    public override void Attack()
    {
        RaycastHit mouseLoc = player.RayCastToMouse();
        if(mouseLoc.collider != null)
        {
            Shoot(mouseLoc);
        }
    }

    //Idea for making aiming smarter. i.e. when aiming up ramps and when aiming at floor if cursor is really close to player
    //Raycast from gun and check distance, if distance is small enough, then just shoot on player's y level/hit location + player's y scale
    //else just shoot at hit location (where mouse is)
    //Actually might just want to universally add the player y scale offset to hit location

    private void Shoot(RaycastHit hit)
    {
        Projectile projectile = Instantiate(projectilePrefab);

        //Vector3 pointAboveFloor = hit.point + new Vector3(0, this.transform.position.y, 0);
        //Vector3 direction = pointAboveFloor - this.transform.position;
        Vector3 direction = hit.point - this.transform.position;

        Ray shootRay = new Ray(this.transform.position, direction);
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100.1f, Color.green, 1);
        foreach (Collider collider in player.GetComponents<Collider>())
        {
            Physics.IgnoreCollision(collider, projectile.GetComponent<Collider>());
        }
        
        projectile.FireProjectile(shootRay);
    }
}
