using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float nextFireTime = 0f;
    public float fireRate = 10f; //per second
    public Projectile projectilePrefab;
    public float recoil = 0f;
    public Vector3 spread = Vector3.zero;

    protected PlayerMovement player;

    private void Awake()
    {
        player = this.GetComponentInParent<PlayerMovement>();
    }

    public abstract void Attack();

    public Vector3 GetAimPoint()
    {
        RaycastHit mouseLoc = player.RayCastToMouse(LayerMask.GetMask("Ground") | LayerMask.GetMask("Enemy"));
        Vector3 point = Vector3.zero;
        if (mouseLoc.collider != null)
        {
            point = mouseLoc.point;
            if (player.isGrounded)
            {
                if (point.y < player.groundCheck.position.y - 0.1f)
                {
                    //This is for aiming down ramps
                    point.y += (player.weaponHolder.position.y - player.groundCheck.position.y);
                    //If aiming at enemy, y will be closer to enemy's center
                    if (mouseLoc.collider.gameObject.CompareTag("Enemy"))
                    {
                        point.y = Mathf.Lerp(point.y, mouseLoc.collider.transform.position.y, 0.2f);
                    }
                }

                else if (point.y > player.groundCheck.position.y + 0.1f)
                {
                    //This is for aiming up ramps
                    point.y += (player.weaponHolder.position.y - player.groundCheck.position.y);
                    //If aiming at enemy, y will be closer to enemy's center
                    if (mouseLoc.collider.gameObject.CompareTag("Enemy"))
                    {
                        point.y = Mathf.Lerp(point.y, mouseLoc.collider.transform.position.y, 0.5f);
                    }
                }
                //Will shoot straight when grounded and aiming at player level, will want to add to this later (see above) if you want to
                //aim at higher places on walls while grounded
                //Will also want to allow you to shoot at enemies when raycast hits them
                else
                {
                    point.y = player.weaponHolder.position.y;
                }
            }
        }
        return point;
    }

    public Vector3 GetRandomSpreadAngle()
    {
        return new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z));
    }

    public void ApplyRecoil(Vector3 point)
    {
        Vector3 force = -(point - this.transform.position).normalized * recoil;
        force.y *= 0.1f;
        player.ApplyForce(force);
    }
}
