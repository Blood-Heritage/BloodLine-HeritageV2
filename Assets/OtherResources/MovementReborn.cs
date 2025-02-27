using System;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class MovementReborn : MonoBehaviourPun
{
    private float speed;
    public float moveSpeed = 5;
    public float runningSpeed = 8;
    
    // [Header("Keybinds")]
    // public KeyCode forRunning = KeyCode.LeftShift;
    // public KeyCode forJumping = KeyCode.Space;
    //
    // public Transform orientation;
    //
    // [Header("Jumping")]
    // public float jumpForce;
    // public float jumpCoolDown;
    // public float airMultiplier;
    // public bool readyToJump = true;
    //
    // [Header("Ground Check")] 
    // public float groundDrag = 0.2f;
    //
    // public bool grounded
    // {
    //     get
    //     {
    //         return cc.isGrounded;
    //     }
    // }
    
    // private float horizontalInput;
    // private float verticalInput;
    // private int isJumpingHash = Animator.StringToHash("isJumping");
    
    
    public CinemachineFreeLook vc;
    
    public void SetCameraObjectCustom(CinemachineFreeLook cameraObject)
    {
        vc = cameraObject;
    }
    public void ChangeCameraPriority(int priority)
    {
        vc.Priority = priority;
    }
    
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    
    private bool isMovementPressed;
    private bool isRunningPressed;
    private bool isShootingPressed;
    
    private PlayerInput playerInput;
    
    Vector3 moveDirection;
    CharacterController cc;
    Animator animator;
    
    
    int isWalkingHash; 
    int isRunningHash;
    int isShootingHash;
    
    private void Awake()
    { 
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isShootingHash = Animator.StringToHash("isShooting");

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRunInput;
        playerInput.CharacterControls.Run.canceled += OnRunInput;
        
        
        playerInput.CharacterControls.Fire.started += OnFireInput;
        playerInput.CharacterControls.Fire.canceled += OnFireInput;
    }

    void OnRunInput(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void OnFireInput(InputAction.CallbackContext context)
    {
        isShootingPressed = context.ReadValueAsButton();
    }

    void OnMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // Ignore les mouvements des autres joueurs
        
        Vector3 movement = (transform.forward);

        if (isMovementPressed)
        {
            // speed = runningSpeed
            if (isRunningPressed)
            {
                if (isShootingPressed) speed = moveSpeed / 2;
                else speed = runningSpeed;
            }
            else speed = moveSpeed;

            movement *= speed;
            
            cc.Move(movement * Time.deltaTime);
        }
        
        Animation();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    
    
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
    
    
    void Animation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        
        // bool runningKeyPressed = Input.GetKey();
        // bool shootingKeyPressed = Input.GetKey(forShooting);
        
        // if it moves
        if (!isWalking && isMovementPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        // if is not moving
        if (isWalking && !isMovementPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // if player is walking and presses Shift Left
        if (!isRunning && (isMovementPressed && isRunningPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        
        // if player stops walking or running
        if (isRunning &&  (!isMovementPressed || !isRunningPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }

}
