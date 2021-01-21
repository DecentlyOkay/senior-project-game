﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float dashSpeed = 20;
    public float dashLength = 0.3f;
    public float dashRecoverTime = 0.3f;

    protected bool isDashing = false;
    protected float dashTimeStamp;
    //Might not want to keep direction fixed during dash, might be more fun for dash just to be a speed increase
    protected Vector3 dashDirection;

    protected Vector3 moveDirection;
    protected Rigidbody rigidbody;

    public Projectile projectilePrefab;
    public LayerMask groundMask;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = UpdateVelocity();
    }

    private Vector3 UpdateVelocity()
    {
        Vector3 move = Vector3.zero;
        move = moveDirection * moveSpeed;

        Vector3 dash = Vector3.zero;
        if (isDashing)
        {
            dash = dashDirection * dashSpeed;
            if (dashTimeStamp + dashLength < Time.time)
                isDashing = false;
        }

        return move + dash;
    }
    private void RayCastOnMouseClick()
    {
        RaycastHit hit;
        //will probably want to change this when adding controller support
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Debug.DrawRay(rayToFloor.origin, rayToFloor.direction * 100.1f, Color.red, 2);
        if (Physics.Raycast(rayToFloor, out hit, 100.0f, groundMask, QueryTriggerInteraction.Collide))
        {
            //if we have gun euipped
            Shoot(hit);

            //will eventually add more functions
        }
    }
    private void Shoot(RaycastHit hit)
    {
        Projectile projectile = Instantiate(projectilePrefab);
        Vector3 pointAboveFloor = hit.point + new Vector3(0, this.transform.position.y, 0);
        Vector3 direction = pointAboveFloor - this.transform.position;
        Ray shootRay = new Ray(this.transform.position, direction);
        Debug.DrawRay(shootRay.origin, shootRay.direction * 100.1f, Color.green, 2);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), projectile.GetComponent<Collider>());
        projectile.FireProjectile(shootRay);
    }

    #region Handling Inputs
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        Debug.Log(inputVec);
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
    public void OnJump()
    {

    }
    public void OnDash()
    {
        if (dashTimeStamp + dashRecoverTime > Time.time)
            return;
        
        Debug.Log("dashing");
        dashDirection = moveDirection;
        dashTimeStamp = Time.time;
        isDashing = true;
    }

    public void OnAttack()
    {
        RayCastOnMouseClick();
    }
    #endregion
}
