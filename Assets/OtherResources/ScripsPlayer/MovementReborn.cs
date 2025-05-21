using System;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class MovementReborn : MonoBehaviourPun
{
    private float speed;
    public float moveSpeed;
    public float runningSpeed;
    public int acceleration;
    public int deceleration;
    
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 moveDirection;

    [Header("State Manager")]
    public bool pauseIsNotPressed;
    public bool isRunningPressed;
    public bool isMovementPressed;
    public bool isShootingPressed;
    public bool isJumpingPressed;
    public bool isAimingPressed;
    
    [Header("ThirdPersonController Unity")]
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f; // 0.12f
    
    private PlayerInput playerInput;
    
    CharacterController cc;
    public Animator animator;
    
    
    int isShootingHash;
    int isJumpingHash;
    int velocityXHash;
    int velocityZHash;

    int TorsoLayer;
    int ShootingLayer;

    private float Velocity_X = 0.0f;
    private float Velocity_Z = 0.0f;

    [Header("Rigging")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private float aimRigWeight;


    [Header("Jumping")] 
    public bool isJumping = false;
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    private float _initialJumpVelocity = 0.0f;
    private float _initialJumpVelocityRunning = 0.0f;
    
    [Header("gravity")]
    public float gravity = -9.8f;
    public float groundedGravity = -0.05f;

    private Stats _stats;
    
    public ICinemachineCamera normal;
    public ICinemachineCamera aimCam;
    
    private GameObject _mainCamera;
    
    
    private GameObject activeCam
    {
        get
        {
            if (normal.Priority > aimCam.Priority)
                return normal.VirtualCameraGameObject;
            else return aimCam.VirtualCameraGameObject;
        }
    }


    public bool GoingBackwards
    {
        get
        {
            return currentMovement.z < 0.0f;
        }
    }
    
    public bool IsOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }

    public void SetCameras(ICinemachineCamera freelook, ICinemachineCamera aim, Transform _activeCam)
    {
        normal = freelook;
        aimCam = aim;

        // priorize freelook
        normal.Priority = 20;
        aimCam.Priority = 10;
        
        _cinemachineTargetYaw = activeCam.transform.rotation.eulerAngles.y;
    }
    
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (float)((-2 * maxJumpHeight) / Math.Pow(timeToApex, 2));
        _initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        _initialJumpVelocityRunning = 0.75f * (2 * maxJumpHeight) / timeToApex;
    }
    
    private void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        _stats = GetComponent<Stats>();
        
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        velocityXHash = Animator.StringToHash("Velocity X");
        velocityZHash = Animator.StringToHash("Velocity Z");
        isShootingHash = Animator.StringToHash("isShooting");
        isJumpingHash = Animator.StringToHash("isJumping");

        TorsoLayer = animator.GetLayerIndex("Torso");
        ShootingLayer = animator.GetLayerIndex("Aim");
        
        // para empezar
        animator.SetLayerWeight(TorsoLayer, 0.0f);
        
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRunInput;
        playerInput.CharacterControls.Run.canceled += OnRunInput;
        
        
        playerInput.CharacterControls.Fire.started += OnFireInput;
        playerInput.CharacterControls.Fire.canceled += OnFireInput;
        
        playerInput.CharacterControls.Aim.started += OnAimingInput;
        playerInput.CharacterControls.Aim.canceled += OnAimingInput;
 
        playerInput.CharacterControls.Jump.started += OnJumpInput;
        playerInput.CharacterControls.Jump.canceled += OnJumpInput;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        setupJumpVariables();
    }
    
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    
    
    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    
    private const float _threshold = 0.01f;
    
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    
    
    
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [SerializeField] private float Sensitivity = 1f;
    
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        var look = playerInput.CharacterControls.Look.ReadValue<Vector2>();
        
        if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;
    
            _cinemachineTargetYaw += look.x * deltaTimeMultiplier * Sensitivity;
            _cinemachineTargetPitch += look.y * deltaTimeMultiplier * Sensitivity;
        }
    
        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
    
        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
        
        // Debug.Log($"rotation active camera: {activeCam.transform.rotation}");
        // transform.forward = new Vector3(activeCam.transform.forward.x, 0, activeCam.transform.forward.z);
    }

    private void Start()
    {
        pauseIsNotPressed = true;
        if (IsOnline())
        {
            // fix bug of new spawn
            transform.position = new Vector3(-57, 10, 38);
        }
    }


    void OnRunInput(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void OnAimingInput(InputAction.CallbackContext context)
    {
        isAimingPressed = context.ReadValueAsButton();
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
        
        // Debug.Log($"currentMovementInput: {currentMovementInput}");
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumpingPressed = context.ReadValueAsButton();
    }
    

    private void Update()
    {
        if (!photonView.IsMine) return; // Ignore les mouvements des autres joueurs
        if (!pauseIsNotPressed)
        {
            ZeroAnimation();
            return;
        }
        
        Animation();
        UpdateCameras();
        
        Move();
        Rotate(RotationSmoothTime);
        
        handleGravity();
        handleJump();
    }

    private void handleJump()
    {
        if (!isJumping && cc.isGrounded && isJumpingPressed)
        {
            isJumping = true;

            if (isRunningPressed)
                currentMovement.y = _initialJumpVelocityRunning;
            else
                currentMovement.y = _initialJumpVelocity;
            
        } else if (!isJumpingPressed && cc.isGrounded && isJumping)
        {
            isJumping = false;
        }
    }

    private void handleGravity()
    {
        if (cc.isGrounded)
        {
            currentMovement.y = groundedGravity;
        }

        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            
            currentMovement.y = nextYVelocity;
        }
    }

    private void UpdateCameras()
    {
        if (isAimingPressed || isShootingPressed)
        {
            aimCam.Priority = 30;
            Sensitivity = 0.5f;
            aimRigWeight = 1f;
            RotationSmoothTime = 0.05f;
        }
        else
        {
            aimCam.Priority = 10;
            Sensitivity = 1f;
            aimRigWeight = 0f;
            RotationSmoothTime = 0.12f;
        }
        
        // for pointing the gun to the center of the screen
        // I don't remember how it works
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }
    
    private float RotationFloat(float duration)
    {
        _targetRotation = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            duration);

        return rotation;
    }


    private void Rotate(float duration)
    {
        if (!GoingBackwards) RotateFoward(duration);
        else RotateBackwards(duration);
    }
    private void RotateFoward(float duration)
    {
        float rotation = RotationFloat(duration);
        
        // rotate to face input direction relative to camera position
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    
    
    private void RotateBackwards(float duration)
    {
        _targetRotation = Mathf.Atan2(currentMovement.x, -currentMovement.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            duration);
        
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    private void Move()
    {
        Vector3 yVelocity = new Vector3(0, currentMovement.y, 0);
        
        if (isMovementPressed)
        {
            // speed = runningSpeed
            if (isRunningPressed)
            {
                if (isShootingPressed || isAimingPressed || GoingBackwards || !_stats.CanRun) speed = moveSpeed / 2;
                else speed = runningSpeed;
            }
            else speed = moveSpeed;
            
            Vector3 targetDirection = new Vector3();
            
            if (!GoingBackwards)
                targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            else 
                targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.back;
            
            cc.Move(targetDirection.normalized * 
                    (speed * Time.deltaTime)); //+ new Vector3(0.0f, 0, 0.0f) * Time.deltaTime);
        }
        
        cc.Move(yVelocity * Time.deltaTime);
    }

    
    private void LateUpdate()
    {
        // Permet de ne plus bouger la camera pdnt le menu pause
        if(pauseIsNotPressed)
            CameraRotation();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    
    
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }


    void ZeroAnimation()
    {
        animator.SetLayerWeight(TorsoLayer, 0.0f);
        animator.SetLayerWeight(ShootingLayer, 0.0f);
        animator.SetBool(isJumpingHash, false);
        animator.SetBool(isShootingHash, false);
        animator.SetFloat(velocityXHash, 0.0f);
        animator.SetFloat(velocityZHash, 0.0f);
    }

    void Animation()
    {
        bool isShooting = animator.GetBool(isShootingHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        if (isShootingPressed || isAimingPressed) animator.SetLayerWeight(TorsoLayer, 0.5f);
        else animator.SetLayerWeight(TorsoLayer, 0.0f);

        if (isAimingPressed || isShootingPressed)
            animator.SetLayerWeight(ShootingLayer, 1.0f);
        
        if (!isAimingPressed && !isShootingPressed)
            animator.SetLayerWeight(ShootingLayer, 0.0f);
        
        
        if (isShootingPressed && !isShooting)
        {
            // animator.SetLayerWeight(ShootingLayer, 1.0f);
            animator.SetBool(isShootingHash, true);
        }

        if (!isShootingPressed && isShooting)
        {
            // animator.SetLayerWeight(ShootingLayer, 0.0f);
            animator.SetBool(isShootingHash, false);
        }
        
         
        // Jumping
         
        if (isJumpingPressed && !isJumping)
        {
            // animator.SetLayerWeight(JumpingLayer, 1.0f);
            animator.SetBool(isJumpingHash, true);
        }

        if (!isJumpingPressed && isJumping && animator)
        {
            // animator.SetLayerWeight(JumpingLayer, 0.0f);
            animator.SetBool(isJumpingHash, false);
        }
        

        // --- Movement logic with acceleration ---
        Vector2 targetVelocity = Vector2.zero;

        if (isMovementPressed)
        {
            float baseZ = isRunningPressed && _stats.CanRun && !isAimingPressed && !GoingBackwards ? 2.0f : 0.5f;

            // Direcciones correctas (adelante o atrás)
            float zSign = Mathf.Sign(currentMovement.z);
            // float xSign = Mathf.Sign(currentMovement.x);

            targetVelocity = new Vector2(currentMovement.x, baseZ * zSign);        
        }

        // Smooth acceleration/deceleration for Z
        if (Mathf.Abs(Velocity_Z - targetVelocity.y) > 0.01f)
        {
            float zDirection = Mathf.Sign(targetVelocity.y - Velocity_Z);
            Velocity_Z += zDirection * acceleration * Time.deltaTime;

            if (zDirection > 0 && Velocity_Z > targetVelocity.y) Velocity_Z = targetVelocity.y;
            if (zDirection < 0 && Velocity_Z < targetVelocity.y) Velocity_Z = targetVelocity.y;
        }

        // Smooth acceleration/deceleration for X
        if (Mathf.Abs(Velocity_X - targetVelocity.x) > 0.01f)
        {
            float xDirection = Mathf.Sign(targetVelocity.x - Velocity_X);
            Velocity_X += xDirection * acceleration * Time.deltaTime;

            if (xDirection > 0 && Velocity_X > targetVelocity.x) Velocity_X = targetVelocity.x;
            if (xDirection < 0 && Velocity_X < targetVelocity.x) Velocity_X = targetVelocity.x;
        }
        
        animator.SetFloat(velocityXHash, Velocity_X);
        animator.SetFloat(velocityZHash, Velocity_Z);
    }
}
