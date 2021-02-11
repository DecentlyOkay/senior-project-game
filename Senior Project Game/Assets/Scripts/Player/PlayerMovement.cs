using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dashSpeed = 20f;
    public float dashLength = 0.5f;
    public float dashCooldown = 0.5f;
    public float jumpPower = 10f;
    public float gravity = -9.8f;
    public float forceFallOffFactor = 0.5f;
    //Might just want to make groundMask everything but the player layer, something like ^(playerMask)
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    public Transform weaponHolder;
    public Transform models;

    //So I guess the plan is to store forces from explosions/recoil here and handle them manually
    private Vector3 forces;

    private bool isDashing = false;
    public bool isGrounded;
    private bool isJumping;
    private float nextDashTime;
    //Might not want to keep direction fixed during dash, might be more fun for dash just to be a speed increase
    private Vector3 dashDirection;
    private Vector3 moveDirection;
    private Rigidbody rigidbody;


    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        forces = Vector3.zero;
    }

    //Will want to move these to separate methods
    private void FixedUpdate()
    {
        //Grounded check
        //Might not need mask even, can just check for not player? That way, can jump off of enemies' heads
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && rigidbody.velocity.y < 0)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }
        Debug.Log(isGrounded);
        //Jumping
        if(isJumping)
        {
            rigidbody.AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
            isJumping = false;
            isGrounded = false;
        }

        //Calculate movement
        Vector3 move = Vector3.zero;
        move = moveDirection * moveSpeed;
        Vector3 dash = Vector3.zero;
        if (isDashing)
        {
            float dashStartTime = nextDashTime - dashCooldown;
            if (dashStartTime + dashLength < Time.time)
                isDashing = false;

            //for if you want dash == speed boost
            //dashDirection = moveDirection;
            
            dash = dashDirection * dashSpeed;

        }

        //Better movement code
        Vector3 horizontalMove = move + dash - rigidbody.velocity;
        UpdateForces(horizontalMove);


        //Old movement code
        //SetHorizontalVelocity(move + dash);
        //UpdateVelocity(0, gravity * Time.fixedDeltaTime, 0);
    }

    private void Update()
    {
        //look at mouse cursor
        //probably will change to the model later
        //handle when grounded and in air separately?
        RaycastHit mouseLoc = RayCastToMouse();
        if (mouseLoc.collider != null)
        {
            Vector3 lookPoint = mouseLoc.point;
            if (isGrounded)
            {
                lookPoint.y = transform.position.y;
            }
            models.transform.LookAt(lookPoint);
        }

        //Might want to do some fancy code to make it where you can move within 90 degrees of a dash direction
        //And movement in anything outside of that range will not register

        //Actually might want to consider binding dash to right click, this will lose weapon functionality, i.e. scoping in
        //but will allow you to dash in the mouse direction.
    }
    
    private void UpdateForces(Vector3 movement)
    {
        movement.y = 0;
        rigidbody.AddForce(movement, ForceMode.VelocityChange);
        rigidbody.AddForce(0, gravity * Time.fixedDeltaTime, 0, ForceMode.VelocityChange);
        rigidbody.AddForce(forces, ForceMode.VelocityChange);
        forces *= forceFallOffFactor;
    }

    public void ApplyForce(Vector3 force)
    {
        Debug.Log("force being applied " + force);
        forces.x += force.x;
        forces.z += force.z;
        rigidbody.AddForce(0, force.y, 0, ForceMode.Impulse);
    }

    public RaycastHit RayCastToMouse()
    {
        RaycastHit hit;
        //will want to augment this when adding controller support
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Debug.DrawRay(rayToFloor.origin, rayToFloor.direction * 1000f, Color.red, 1);
        Physics.Raycast(rayToFloor, out hit, 1000f, groundMask, QueryTriggerInteraction.Collide);

        //Make it so that when player is on lower ground and mouse clicks over higher ground, they y position of the hit
        //will be at the player's feet's y position
        //if(hit.collider != null)
        //{
        //    hit.point = new Vector3(hit.point.x, Mathf.Min(hit.point.y, transform.position.y), hit.point.z);
        //}
        return hit;
    }

    #region Handling Inputs
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
    public void OnJump()
    {
        //Trying to make jumping feel better when walking down ramps. There will be greater distance leeway allowed when
        //checking if you can perform a jump than in checking if you are grounded.

        //Probably want to add coyote time + buffered jumps as well

        if (/*isGrounded*/ Physics.CheckSphere(groundCheck.position, groundDistance + 0.1f, groundMask))
        {
            Debug.Log("jump");
            isJumping = true;
        }

    }
    public void OnDash()
    {
        //If dash is currently on cooldown, you will not dash
        if (nextDashTime > Time.time)
            return;

        Debug.Log("dashing");
        dashDirection = moveDirection;
        nextDashTime = Time.time + dashCooldown;
        isDashing = true;
    }

    //Will probably change the input to take in context in order to have automatic fire
    public void OnAttack()
    {
        Debug.Log("attack");
        //Obviously need to augment this up when adding more weapons
        foreach (Transform weapon in weaponHolder)
        {
            weapon.gameObject.GetComponent<Weapon>().Attack();
        }
    }
    #endregion
}
