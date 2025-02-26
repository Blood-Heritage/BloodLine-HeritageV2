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
    
    private PlayerInput playerInput;
    
    Vector3 moveDirection;
    CharacterController cc;
    Animator animator;
    
    private void Awake()
    { 
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRunInput;
        playerInput.CharacterControls.Run.canceled += OnRunInput;
    }

    void OnRunInput(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void OnMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        Debug.Log(currentMovementInput);
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // Ignore les mouvements des autres joueurs

        Vector3 movement = (transform.forward * currentMovement.z) + (currentMovement.x * transform.right);
        if (isRunningPressed) movement *= runningSpeed;
        else movement *= moveSpeed;
        
        Debug.Log($"movement: {movement}");
        
        cc.Move(movement * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    
    
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
