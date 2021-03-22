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
    public float dashStamina = 2f;
    public float jumpPower = 10f;
    public float jumpStamina = 2f;
    public float gravity = -9.8f;
    public float forceFallOffFactor = 0.5f;

    private LayerMask groundMask;
    private LayerMask enemyMask;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    public Transform weaponHolder;
    public Transform models;

    private Weapon currentWeapon;

    //So I guess the plan is to store forces from explosions/recoil here and handle them manually
    private Vector3 forces;

    private bool isDashing = false;
    public bool isGrounded;
    private bool isJumping;
    private float nextDashTime;
    //Might not want to keep direction fixed during dash, might be more fun for dash just to be a speed increase
    private Vector3 dashDirection;
    public Vector3 moveDirection;
    public Vector3 totalMoveDirection;
    private new Rigidbody rigidbody;

    private bool isAttacking = false;

    private Player playerInfo;

    public Transform aimPoint;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInfo = this.GetComponent<Player>();
        this.tag = "Player";
        //Both enemies and the ground will count as mask for jumping and grounded purposes
        groundMask = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
    }
    void Start()
    {
        moveDirection = Vector3.zero;
        forces = Vector3.zero;

        //For when playerdata is not initialized
        PlayerData.gunsUnlocked[0] = true;

        SwitchWeapon(0);
        

        ToggleAimIndicator(PlayerData.aimIndicatorEnabled);
    }

    //Will want to move these to separate methods
    private void FixedUpdate()
    {
        //Grounded check
        //Might not need mask even, can just check for not player? That way, can jump off of enemies' heads
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | enemyMask);
        if (isGrounded && rigidbody.velocity.y < 0)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }
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
        totalMoveDirection = move + dash;
        //Better movement code
        Vector3 horizontalMove = move + dash - rigidbody.velocity;
        UpdateForces(horizontalMove);


        //Old movement code
        //SetHorizontalVelocity(move + dash);
        //UpdateVelocity(0, gravity * Time.fixedDeltaTime, 0);


        DoAttack();
    }

    private void Update()
    {
        //look at mouse cursor
        //probably will change to the model later
        //handle when grounded and in air separately?
        if (!PauseMenu.gameIsPaused)
        {
            LookAtMouse();
        }

        //Might want to do some fancy code to make it where you can move within 90 degrees of a dash direction
        //And movement in anything outside of that range will not register

        //Actually might want to consider binding dash to right click, this will lose weapon functionality, i.e. scoping in
        //but will allow you to dash in the mouse direction.
    }

    private void LookAtMouse()
    {
        RaycastHit mouseLoc = RayCastToMouse(groundMask | enemyMask);
        if (mouseLoc.collider != null)
        {
            Vector3 lookPoint = mouseLoc.point;
            if (isGrounded)
            {
                lookPoint.y = transform.position.y;
            }
            models.transform.LookAt(lookPoint);
            aimPoint.position = mouseLoc.point;
        }
    }
    public void ToggleAimIndicator(bool toggle)
    {
        PlayerData.aimIndicatorEnabled = toggle;
        aimPoint.gameObject.SetActive(toggle);
    }
    
    private void UpdateForces(Vector3 movement)
    {
        movement.y = 0;
        rigidbody.AddForce(movement, ForceMode.VelocityChange);
        rigidbody.AddForce(0, gravity * Time.fixedDeltaTime, 0, ForceMode.VelocityChange);
        rigidbody.AddForce(forces, ForceMode.Impulse);
        forces *= forceFallOffFactor;
        if (forces.magnitude < 10f)
            forces *= Mathf.Pow(forces.magnitude / 10f, 2);
    }

    public void ApplyForce(Vector3 force)
    {
        forces.x += force.x;
        forces.z += force.z;
        rigidbody.AddForce(0, force.y, 0, ForceMode.Impulse);
    }

    public RaycastHit RayCastToMouse(LayerMask mask)
    {
        RaycastHit hit;
        //will want to augment this when adding controller support
        Ray rayToFloor = Camera.main.ScreenPointToRay(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Debug.DrawRay(rayToFloor.origin, rayToFloor.direction * 1000f, Color.red, 1);
        Physics.Raycast(rayToFloor, out hit, 1000f, mask, QueryTriggerInteraction.Collide);

        //Make it so that when player is on lower ground and mouse clicks over higher ground, they y position of the hit
        //will be at the player's feet's y position
        //if(hit.collider != null)
        //{
        //    hit.point = new Vector3(hit.point.x, Mathf.Min(hit.point.y, transform.position.y), hit.point.z);
        //}
        return hit;
    }

    private void DoAttack()
    {
        if (!isAttacking)
            return;
        if (currentWeapon.nextFireTime > Time.time)
            return;
        currentWeapon.nextFireTime = Time.time + 1f / currentWeapon.fireRate;
        currentWeapon.Attack();
        //foreach (Transform w in weaponHolder)
        //{
        //    Debug.Log(w);
        //    Weapon weapon = w.GetComponent<Weapon>();
        //    if (weapon.nextFireTime > Time.time)
        //        return;
        //    weapon.nextFireTime = Time.time + 1f / weapon.fireRate;
        //    weapon.Attack();
        //}
    }

    private void SwitchWeapon(int change)
    {
        int weaponIndex = PlayerData.weaponIndex;
        weaponHolder.GetChild(weaponIndex).gameObject.SetActive(false);
        Debug.Log("gun " + weaponIndex + " " + PlayerData.gunsUnlocked[weaponIndex]);
        do
        {
            weaponIndex += change;
            //if index goes below 0 or above max index
            weaponIndex = (weaponIndex + weaponHolder.childCount) % weaponHolder.childCount;

            //only have 1 or 0 weapons unlocked, so we looped through all the weapons without finding on unlocked
            if (weaponIndex == PlayerData.weaponIndex)
                break;
        }
        while (!PlayerData.gunsUnlocked[weaponIndex]);

        weaponHolder.GetChild(weaponIndex).gameObject.SetActive(true);
        currentWeapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        PlayerData.weaponIndex = weaponIndex;
    }

    #region Handling Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVec = context.action.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
    public void OnJump()
    {
        //If you don't have enough stamina;
        if (playerInfo.stamina < jumpStamina)
            return;
        //Trying to make jumping feel better when walking down ramps. There will be greater distance leeway allowed when
        //checking if you can perform a jump than in checking if you are grounded.

        //Probably want to add coyote time + buffered jumps as well
        
        if (/*not currently jumping*/ !isJumping && /*isGrounded*/ Physics.CheckSphere(groundCheck.position, groundDistance + 0.1f, groundMask | enemyMask))
        {
            playerInfo.stamina -= jumpStamina;
            Debug.Log("jump");
            isJumping = true;
        }

    }
    public void OnDash()
    {
        //If dash is currently on cooldown, you will not dash
        if (nextDashTime > Time.time)
            return;

        //If you don't have enough stamina;
        if (playerInfo.stamina < dashStamina)
            return;
        playerInfo.stamina -= dashStamina;

        Debug.Log("dashing");
        dashDirection = moveDirection;
        nextDashTime = Time.time + dashCooldown;
        isDashing = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        //Debug.Log("started: " + context.started + " " + context.action.ReadValueAsObject() + " canceled : " + context.canceled + " performed " + context.performed + " duration: " + context.duration);
        isAttacking = context.action.ReadValue<float>() > 0;
        //Debug.Log(isAttacking + " " + context.action.ReadValue<float>());
    }
    public void OnNextWeapon(InputAction.CallbackContext context)
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (context.performed)
        {
            SwitchWeapon(1);
        }
    }
    public void OnPreviousWeapon(InputAction.CallbackContext context)
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (context.performed)
        {
            SwitchWeapon(-1);
        }
    }

    #endregion
}
