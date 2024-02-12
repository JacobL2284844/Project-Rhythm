using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterAnimation characterAnimation;

    //input
    private ThirdPersonInput playerInputActionAsset;
    private InputAction move;

    [Header("Movement")]
    private Rigidbody rigidbody;
    [SerializeField]
    private float movementForce = 1f;
    private float og_movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float currentMaxSpeed = 4f;
    private float baseMaxSpeed = 4f;
    private Vector3 forceDirection = Vector3.zero;
    [SerializeField]
    private float fallForce = 65f;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float groundcheckRaycastDistance = 0.5f;

    [Header("Sprint")]
    [SerializeField]
    private float sprintMultiplier = 1.5f;
    [SerializeField]
    private bool isSprinting = false;
    [SerializeField]
    private float sprintDuration = 5f;

    [Header("WallRun")]
    [SerializeField]
    private LayerMask wallMask;
    [SerializeField]
    private bool isWallRunning;
    [SerializeField]
    private float wallRunSpeedMultiplier;
    [SerializeField]
    private float wallRunDuration = 0.23f;
    [SerializeField]
    private float wallRunExitJumpForce;

    private bool onWall_left;
    private bool onWall_right;
    RaycastHit leftWall_rayHit;
    RaycastHit rightWall_rayHit;
    Vector3 wallNormal;
    Vector3 forwardDirection;

    [Header("Roll")]
    public float rollDuration = 2.5f;
    [SerializeField]
    private float rollForce = 2f;
    [SerializeField]
    private bool isRolling = false;
    [SerializeField] float maxSpeed_Rolling = 7.5f;

    [Header("Slide")]
    [SerializeField]
    private float slideVelocityMultiplier = 1.2f;
    [SerializeField]
    private float slideBoostForce = 3f;
    public float slideCooldown = 2.5f;
    [SerializeField]
    private float slideDuration = 0.5f;
    private bool isSliding = false;
    [SerializeField] float maxSpeedSliding = 20f;

    [Header("Slam")]
    [SerializeField]
    private float slamForce = 10f;

    //-----------------
    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInputActionAsset = new ThirdPersonInput();
        baseMaxSpeed = currentMaxSpeed;
        og_movementForce = movementForce;
    }
    //-----------------
    private void OnEnable()
    {
        playerInputActionAsset.Player.Jump.started += DoJump;
        move = playerInputActionAsset.Player.Move;
        playerInputActionAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActionAsset.Player.Jump.started -= DoJump;
        playerInputActionAsset.Player.Disable();
    }
    //-----------------
    private void FixedUpdate()
    {
        //movement
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
        //imploment movement
        rigidbody.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        //cap velocity
        Vector3 horzontalVelocity = rigidbody.velocity;
        horzontalVelocity.y = 0;
        if (horzontalVelocity.sqrMagnitude > currentMaxSpeed * currentMaxSpeed)
        {
            rigidbody.velocity = horzontalVelocity.normalized * currentMaxSpeed + Vector3.up * rigidbody.velocity.y;
        }

        LookAt();

        //if falling check ground
        if (rigidbody.velocity.y < 0f)
        {
            //fix gravity
            rigidbody.velocity += Vector3.down * fallForce * Time.fixedDeltaTime;
        }

        CheckWallRun();
        IsGrounded();
    }
    //-----------------
    private void LookAt()
    {
        Vector3 direction = rigidbody.velocity;
        direction.y = 0;

        //rotate rigidbody in move direction
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;

        return right.normalized;
    }
    //-----------------
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            characterAnimation.DoJump();
            forceDirection += Vector3.up * jumpForce;
        }
        if (isWallRunning)
        {
            ExitWallRun();
        }
    }
    //-----------------
    public void DoRoll(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded() && !isRolling)
        {
            StartCoroutine(PerformRoll());
        }
    }
    IEnumerator PerformRoll()
    {
        isRolling = true;

        currentMaxSpeed = maxSpeed_Rolling;
        // Apply dodge force
        rigidbody.AddForce(transform.forward * rollForce, ForceMode.Impulse);

        // Wait for the dodge duration
        yield return new WaitForSeconds(rollDuration);
        isRolling = false;
        currentMaxSpeed = baseMaxSpeed;
    }
    //-----------------
    public void DoSlide(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded() && !isSliding)
        {
            StartCoroutine(PerformSlide());
        }
    }
    IEnumerator PerformSlide()
    {
        isSliding = true;

        currentMaxSpeed = currentMaxSpeed * slideVelocityMultiplier;
        rigidbody.AddForce(transform.forward * slideBoostForce, ForceMode.Impulse);

        yield return new WaitForSeconds(slideDuration);
        isSliding = false;
        currentMaxSpeed = baseMaxSpeed;
    }
    //-----------------
    public void DoSlam(InputAction.CallbackContext context)
    {
        if (context.started && !IsGrounded())
        {
            rigidbody.AddForce(-transform.up * slamForce, ForceMode.Impulse);
        }
    }
    //-----------------
    public void DoSprint(InputAction.CallbackContext context)
    {
        if (context.started && !isSprinting)
        {
            isSprinting = true;
            StartCoroutine(PerformSprint());
        }
    }
    IEnumerator PerformSprint()
    {
        movementForce = movementForce * sprintMultiplier;
        yield return new WaitForSeconds(sprintDuration);
        movementForce = og_movementForce;
        isSprinting = false;
    }
    //-----------------
    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundcheckRaycastDistance))
        {//is grounded
            characterAnimation.ExitFallAnimation();
            return true;
        }
        else
        {//not grounded
            characterAnimation.DoFallAnimation();
            return false;
        }
    }
    //-----------------
    private bool wasWallrunning = false;
    private void CheckWallRun()
    {
        onWall_left = Physics.Raycast(transform.position, -transform.right, out leftWall_rayHit, 0.7f, wallMask);
        onWall_right = Physics.Raycast(transform.position, transform.right, out rightWall_rayHit, 0.7f, wallMask);

        if ((onWall_right || onWall_left) && !isWallRunning)
        {
            Debug.Log("wallruning");
            WallRun();
            WallRunMovement();

            if (onWall_right)
            {
                onWall_left = false;
            }
            if (onWall_left)
            {
                onWall_right = false;
            }
            characterAnimation.WallRun(onWall_left, onWall_right);
        }


        if ((!onWall_right || !onWall_left) && isWallRunning)
        {
            if (wasWallrunning)
            {
                ExitWallRun();
                wasWallrunning = false;
                Debug.Log("exit");
            }
        }
        wasWallrunning = isWallRunning;
    }
    private void WallRunMovement()
    {
        if (forceDirection.z > (forwardDirection.z - 10f) && forceDirection.z < (forwardDirection.z + 10f))
        {
            forceDirection += forwardDirection;
        }
        else if (forceDirection.z < (forwardDirection.z - 10f) && forceDirection.z > (forwardDirection.z + 10f))
        {
            forceDirection.x = 0f;
            forceDirection.z = 0f;
            ExitWallRun();
        }
    }
    private void WallRun()
    {
        isWallRunning = true;

        if (movementForce < og_movementForce * wallRunSpeedMultiplier)
        {
            movementForce = og_movementForce * wallRunSpeedMultiplier;
        }

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }
    }
    public void ExitWallRun()
    {
        isWallRunning = false;
        movementForce = og_movementForce;
        characterAnimation.ExitWallRun();

        if (onWall_left)
        {
            rigidbody.AddForce(forwardDirection * wallRunExitJumpForce, ForceMode.Impulse);
        }
        if (onWall_right)
        {
            rigidbody.AddForce(forwardDirection * wallRunExitJumpForce, ForceMode.Impulse);
        }
    }
    //-----------------
}
