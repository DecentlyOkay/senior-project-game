using System.Collections;
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


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    #endregion
}
