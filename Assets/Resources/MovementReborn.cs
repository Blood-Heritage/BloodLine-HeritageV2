using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementReborn : MonoBehaviour
{
    private float speed;
    public float moveSpeed;
    public float runningSpeed;
    
    [Header("Keybinds")]
    public KeyCode forRunning = KeyCode.LeftShift;
    public KeyCode forJumping = KeyCode.Space;
    
    public Transform orientation;
    
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("Ground Check")] 
    public float playerHeight;
    public float groundDrag;
    public bool grounded;
    
    private float horizontalInput;
    private float verticalInput;
    private int isJumpingHash = Animator.StringToHash("isJumping");
    
    Vector3 moveDirection;
    Rigidbody rb;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(forRunning)) speed = runningSpeed;
        else speed = moveSpeed;
        
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        MyInput();

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
        
        if (rb.velocity.y == 0) animator.SetBool(isJumpingHash, false);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // for jumping
        if (Input.GetKey(forJumping) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * (speed * 10f), ForceMode.Force);
            animator.SetBool(isJumpingHash, false);
        }
        
        // in air 
        else if (!grounded) 
            rb.AddForce(moveDirection.normalized * (speed * 10f * airMultiplier), ForceMode.Force);
    }


    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        animator.SetBool(isJumpingHash, true);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
