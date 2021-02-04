using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dashSpeed = 20f;
    public float dashLength = 0.5f;
    public float dashCooldown = 0.5f;
    public float jumpPower = 10f;
    public float gravity = -9.8f;
    public CharacterController controller;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public Transform weaponHolder;

    private Vector3 velocity;
    private bool isDashing = false;
    private bool isGrounded;
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

    
    private void Update()
    {
        //look at mouse cursor
        //probably will change to the model later
        //handle when grounded and in air separately?
        RaycastHit hit;
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));

        if (Physics.Raycast(rayToFloor, out hit, 100, groundMask, QueryTriggerInteraction.Collide))
        {
            transform.LookAt(hit.point + new Vector3(0, this.transform.position.y, 0));
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        //Character controller movement
        Vector3 move = Vector3.zero;
        move = moveDirection * moveSpeed;

        Vector3 dash = Vector3.zero;
        if (isDashing)
        {
            float dashStartTime = nextDashTime - dashCooldown;
            if (dashStartTime + dashLength < Time.time)
                isDashing = false;

            //dash == speed boost
            dashDirection = moveDirection;
            dash = dashDirection * dashSpeed;
            
        }
        controller.Move((move + dash) * Time.deltaTime);

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public RaycastHit RayCastToMouse()
    {
        RaycastHit hit;
        //will probably want to change this when adding controller support
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Debug.DrawRay(rayToFloor.origin, rayToFloor.direction * 100.1f, Color.red, 1);
        Physics.Raycast(rayToFloor, out hit, 100.0f, groundMask, QueryTriggerInteraction.Collide);
        return hit;
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
        Debug.Log("jump");
    }
    public void OnDash()
    {
        //Dash currently on cooldown
        if (nextDashTime > Time.time)
            return;
        
        Debug.Log("dashing");
        dashDirection = moveDirection;
        nextDashTime = Time.time + dashCooldown;
        isDashing = true;
    }

    public void OnAttack()
    {
        foreach (Transform weapon in weaponHolder)
        {
            Debug.Log(weapon);
            weapon.gameObject.GetComponent<Weapon>().Attack();
        }
    }
    #endregion
}
