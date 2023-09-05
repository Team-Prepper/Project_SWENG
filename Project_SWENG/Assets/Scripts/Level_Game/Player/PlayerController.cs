using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    /*
    protected static PlayerController staticInstance;
    public static PlayerController instance { get { return staticInstance; } }

    private PlayerInputBase playerInput;
    private CharacterController charCtrl;
    private Animator ani;

    private GameObject mainCamera;

    private bool isMove;
    private bool isJump;
    public bool isGrounded = true;
    private float currentSpeed;
    private float currentMovementSmoothnes;

    private Vector3 moveDirection;
    private Vector3 smoothMoveDirection;
    private Vector3 smoothMoveVelocity;

    public GameObject CinemachineCameraTarget;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    //Rotation offset
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 10.0f;
    public float rotationSpeed = 10f;
    private float targetRotation = 0.0f;
    private float rotationVelocity;

    //Jump
    public float JumpHeight = 1.2f;
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    [Header("Debug  Jump")]
    [SerializeField] private float verticalVelocity = 0;
    [SerializeField]private float terminalVelocity = 53.0f;


    [Header("CamSet")]
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;
    private const float threshold = 0.01f;

    [Header("Variables")]
    [Range(0.01f, 0.5f)]
    [SerializeField] private float movementSmoothnes = 0.1f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float gravity = -15f;

    //Animation
    private bool hasAnimator;

    private int aniIDMove;
    private int aniIDGrounded;
    private int aniIDJump;
    private int aniIDFreeFall;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInputBase>().GetComponent<PlayerInputBase>();
        charCtrl = GetComponent<CharacterController>();

        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        staticInstance = this;
    }

    private void Start()
    {
        hasAnimator = TryGetComponent(out ani);
        AssignAnimationIDs();
    }

    void FixedUpdate()
    {
        SetSpd();
        Movement();
        CameraRotation();
        JumpAndGravity();
        GroundedCheck();
        move();
    }
    private void AssignAnimationIDs()
    {
        aniIDMove = Animator.StringToHash("Move");
        aniIDGrounded = Animator.StringToHash("Grounded");
        aniIDJump = Animator.StringToHash("Jump");
        aniIDFreeFall = Animator.StringToHash("FreeFall");
    }

    private void SetSpd()
    {
        if (playerInput.sprint) // Sprint
        {
            currentSpeed = runSpeed;
            currentMovementSmoothnes = movementSmoothnes * 3f;
        }
        else                    // Walk
        {
            currentSpeed = walkSpeed;
            currentMovementSmoothnes = movementSmoothnes;
        }
    }

    private void Movement()
    {
        //Movement
        smoothMoveDirection = Vector3.SmoothDamp(smoothMoveDirection, moveDirection, ref smoothMoveVelocity, currentMovementSmoothnes);
        
        moveDirection = new Vector3(playerInput.move.x, 0, playerInput.move.y).normalized * currentSpeed;

        if (moveDirection.sqrMagnitude == 0) { isMove = false; }
        else { isMove = true; }
    }

    private void move()
    {
        float targetSpeed = currentSpeed;

        Vector3 moveThisDirection = new Vector3(smoothMoveDirection.x, 0f, smoothMoveDirection.z);

        if (isMove)
        {
            targetRotation = Mathf.Atan2(moveThisDirection.x, moveThisDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            targetSpeed = 0;
        }
        
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        charCtrl.Move(targetDirection.normalized * (Time.deltaTime * targetSpeed) +
                      new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        if (hasAnimator)
        {
            ani.SetFloat(aniIDMove, targetSpeed);
        }
    }

    private void GroundedCheck()
    {

        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);


        if (hasAnimator)
        {
            ani.SetBool(aniIDGrounded, isGrounded);
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            fallTimeoutDelta = FallTimeout;

            if (hasAnimator)
            {
                ani.SetBool(aniIDJump, false);
                ani.SetBool(aniIDFreeFall, false);
            }

            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (playerInput.jump && jumpTimeoutDelta <= 0.0f)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * gravity);
                if (hasAnimator)
                {
                    ani.SetBool(aniIDJump, true);
                }
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = JumpTimeout;
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    ani.SetBool(aniIDFreeFall, true);
                }
            }
            playerInput.jump = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void CameraRotation()
    {
        if (playerInput.look.sqrMagnitude >= threshold && !LockCameraPosition)
        {
            cinemachineTargetYaw += playerInput.look.x * rotationSpeed;
            cinemachineTargetPitch += playerInput.look.y * rotationSpeed;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    */
}
