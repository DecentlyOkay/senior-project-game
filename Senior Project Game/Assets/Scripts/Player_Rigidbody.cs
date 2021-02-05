using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Rigidbody : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dashSpeed = 20f;
    public float dashLength = 0.5f;
    public float dashCooldown = 0.5f;
    public float jumpPower = 10f;
    public float gravity = -9.8f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public Transform weaponHolder;
    public Transform models;

    private Vector3 velocity;
    private bool isDashing = false;
    private bool isGrounded;
    private bool isJumping;
    private float nextDashTime;
    //Might not want to keep direction fixed during dash, might be more fun for dash just to be a speed increase
    private Vector3 dashDirection;
    private Vector3 moveDirection;
    private Rigidbody playerRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && playerRigidbody.velocity.y < 0)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);

        }
        if(isJumping)
        {
            UpdateVelocity(0, jumpPower, 0);
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
        //Can try this out if setting velocity don't work no more
        //playerRigidbody.AddForce(move + dash - playerRigidbody.velocity, ForceMode.VelocityChange);


        SetHorizontalVelocity(move + dash);
        UpdateVelocity(0, gravity * Time.fixedDeltaTime, 0);
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

    private void UpdateVelocity(Vector3 v)
    {
        UpdateVelocity(v.x, v.y, v.z);
    }
    private void UpdateVelocity(float x, float y, float z)
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x + x, playerRigidbody.velocity.y + y, playerRigidbody.velocity.z + z);
    }
    private void SetHorizontalVelocity(Vector3 v)
    {
        playerRigidbody.velocity = new Vector3(v.x, playerRigidbody.velocity.y, v.z);
    }
    public RaycastHit RayCastToMouse()
    {
        RaycastHit hit;
        //will want to augment this when adding controller support
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Debug.DrawRay(rayToFloor.origin, rayToFloor.direction * 100.1f, Color.red, 1);
        Physics.Raycast(rayToFloor, out hit, 100.0f, groundMask, QueryTriggerInteraction.Collide);

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

        if (/*isGrounded*/ Physics.CheckSphere(groundCheck.position, groundDistance + 0.2f, groundMask))
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
        //Obviously need to augment this up when adding more weapons
        foreach (Transform weapon in weaponHolder)
        {
            weapon.gameObject.GetComponent<Weapon>().Attack();
        }
    }
    #endregion
}
