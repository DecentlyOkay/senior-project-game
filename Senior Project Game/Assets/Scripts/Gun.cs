using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Projectile projectilePrefab;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public override void Attack()
    {
        RaycastHit mouseLoc = player.RayCastToMouse();
        if(mouseLoc.collider != null)
        {
            Shoot(mouseLoc);
        }
    }
    private void Shoot(RaycastHit hit)
    {
        Projectile projectile = Instantiate(projectilePrefab);
        Vector3 pointAboveFloor = hit.point + new Vector3(0, this.transform.position.y, 0);
        Vector3 direction = pointAboveFloor - this.transform.position;
        Ray shootRay = new Ray(this.transform.position, direction);
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100.1f, Color.green, 1);
        Physics.IgnoreCollision(player.GetComponent<Collider>(), projectile.GetComponent<Collider>());
        projectile.FireProjectile(shootRay);
    }
}
