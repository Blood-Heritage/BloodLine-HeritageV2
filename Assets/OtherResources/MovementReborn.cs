using Cinemachine;
using UnityEngine;
using Photon.Pun;

public class MovementReborn : MonoBehaviourPun
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
    public CinemachineFreeLook vc;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    public void SetCameraObjectCustom(CinemachineFreeLook cameraObject)
    {
        vc = cameraObject;
    }
    public void ChangeCameraPriority(int priority)
    {
        vc.Priority = priority;
    }

    private void Update()
    {
        // Verifie si le joueur controle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }
        
        if (Input.GetKey(forRunning)) speed = runningSpeed;
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
            Debug.Log($"The current speed: {speed}");
            rb.AddForce(moveDirection.normalized * (speed * 10f), ForceMode.Force);
            animator.SetBool(isJumpingHash, false);
        }
        
        // in air 
        else if (!grounded) 
            rb.AddForce(moveDirection.normalized * (speed * 10f * airMultiplier), ForceMode.Force);
    }


    private void Jump()
    {
        animator.SetBool(isJumpingHash, true);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
