using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterAnimation characterAnimation;

    //input
    private ThirdPersonInput playerInputActionAsset;
    [HideInInspector] public InputAction move;
    [Header("Movement")]
    private bool allowMovement = true;
    public Rigidbody rigidbody;
    [SerializeField]
    private float movementForce = 1f;
    private float og_movementForce = 1f;
    [SerializeField]
    private float currentMaxSpeed = 4f;
    private float baseMaxSpeed = 4f;
    public Vector3 forceDirection = Vector3.zero;

    [Header("Jump/ Gravity")]
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float doubleJumpForce = 5f;
    private bool canDoubleJump = true;
    [SerializeField]
    private float groundcheckRaycastDistance = 0.5f;
    public bool isGroundedState;
    [SerializeField]
    private float fallForce = 65f;
    private float current_timeBeforeHardFall = 1f;
    [SerializeField] private float timeBeforeHardFall = 1f;

    [Header("Sprint")]
    [SerializeField]
    private float sprintMultiplier = 1.5f;
    [SerializeField]
    public bool isSprinting = false;
    [SerializeField]
    private float sprintDuration = 5f;
    public BeatClicker beatClicker;
    [SerializeField] float sprintCurrentDuration;
    [SerializeField] Image sprintFillImageR;
    [SerializeField] Image sprintFillImageL;

    [Header("Camera")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float sprintFovCamIncrease = 5f;

    [Header("WallRun")]
    [SerializeField]
    private LayerMask wallMask;
    [SerializeField]
    private bool isWallRunning;
    [SerializeField] float wallRayCastDistance = 0.7f;
    [SerializeField]
    private float wallRunSpeedMultiplier;
    [SerializeField]
    private float wallRunDuration = 0.23f;
    [SerializeField]
    private float wallRunCooldown = 0.5f;
    private bool canWallRun = true;
    [SerializeField]
    private float wallRunExitJumpForce;
    [SerializeField] private Transform wallJumpDirection_left;
    [SerializeField] private Transform wallJumpDirection_right;

    private bool onWall_left;
    private bool onWall_right;
    RaycastHit leftWall_rayHit;
    RaycastHit rightWall_rayHit;
    Vector3 wallNormal;
    Vector3 forwardDirection;

    [Header("Vault")]
    [SerializeField]
    private float wallForwardDistanceCheck = 0.6f;
    [SerializeField]
    private float wallDownDistanceCheck = 3f;
    [SerializeField]
    private float vaultDuration = 0.3f;
    [SerializeField]
    private Transform vaultfinder;
    [SerializeField]
    private float playerHeight = 2f;
    private bool onWall_forward;
    RaycastHit forwardWall_rayHit;
    bool isVaulting = false;

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
        if (allowMovement)
        {
            IsGrounded();
            LookAt(rigidbody.velocity);

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

            //if falling check ground
            if (rigidbody.velocity.y < 0f && !isGroundedState)
            {
                current_timeBeforeHardFall -= Time.deltaTime;//lower time before hard fall

                if (current_timeBeforeHardFall <= 0)//if timed out
                {
                    //fix gravity
                    rigidbody.velocity += Vector3.down * fallForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                current_timeBeforeHardFall = timeBeforeHardFall;//reset hard fall
            }

            CheckWallRun();
        }
        //sprint duration timer and ui
        if (isSprinting)
        {
            sprintCurrentDuration -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(sprintCurrentDuration / sprintDuration);
            sprintFillImageR.fillAmount = fillAmount;
            sprintFillImageL.fillAmount = fillAmount;

            if (sprintCurrentDuration <= 0.1f)
            {
                ExitSprint();
            }
        }
    }
    //-----------------
    private void LookAt(Vector3 direction)
    {
        direction.y = 0;

        //rotate rigidbody in move direction
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {//not moving
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
    public void EnableMovement()
    {
        //movementForce = 0.1f;
        //Debug.Log("Enable");
    }
    public void DisableMovement()
    {
        //movementForce = og_movementForce;
        //Debug.Log("Disable");
        forceDirection = Vector3.zero;
    }
    //-----------------
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (isWallRunning)
        {
            WallJump();
            ExitWallRun();
            return;
        }

        if (IsGrounded())
        {
            characterAnimation.DoJump();
            forceDirection += Vector3.up * jumpForce;
        }
        else
        {
            CheckandTryVault();

            if (canDoubleJump)
            {
                canDoubleJump = false;

                characterAnimation.DoDoubleJumpAnimation();

                if (OnWallForward())//if on wall jump back
                {
                    forceDirection -= transform.forward * doubleJumpForce;
                }
                else
                {
                    forceDirection += transform.forward * jumpForce;
                }

                forceDirection += Vector3.up * doubleJumpForce;
            }
        }

        TryResetSprintTimer();
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

        AudioManager.instance.PLayOneShot(AudioManager.instance.slideSound, transform.position);

        TryResetSprintTimer();

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
            StartCoroutine(PerformSlide());
        }
    }

    //-----------------
    public void DoSprint(InputAction.CallbackContext context)
    {
        if (context.started && !isSprinting && !isWallRunning && !isSliding && IsGrounded())
        {
            if (movementForce < (og_movementForce * sprintMultiplier))
            {
                StartSprint();
            }
        }
        if (context.started && isSprinting)
        {
            TryResetSprintTimer();
        }
    }
    void StartSprint()
    {
        movementForce = og_movementForce * sprintMultiplier;
        cameraController.maxFOV = cameraController.maxFOV + sprintFovCamIncrease;
        sprintCurrentDuration = sprintDuration;

        isSprinting = true;

        sprintFillImageR.fillAmount = 1;
        sprintFillImageL.fillAmount = 1;
    }
    void TryResetSprintTimer()//called in jump and slide, if performed on beat then reset sprint timer
    {
        if (isSprinting && beatClicker.recentHitState != beatClicker.failTag)
        {
            movementForce = og_movementForce * sprintMultiplier;
            sprintCurrentDuration = sprintDuration;

            isSprinting = true;

            sprintFillImageR.fillAmount = 1;
            sprintFillImageL.fillAmount = 1;
        }
    }
    public void ExitSprint()
    {
        cameraController.maxFOV = cameraController.maxFOV - sprintFovCamIncrease;
        movementForce = og_movementForce;
        isSprinting = false;

        sprintFillImageR.fillAmount = 1;
        sprintFillImageL.fillAmount = 1;
    }
    //-----------------
    private bool IsGrounded()//overrideGroundedState set in attack lerp
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundcheckRaycastDistance))
        {//is grounded
            isGroundedState = true;

            canDoubleJump = true;

            characterAnimation.ExitFallAnimation();
            characterAnimation.WallHangAnimation(false);

            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            return true;
        }
        else
        {//not grounded
            isGroundedState = false;

            characterAnimation.DoFallAnimation();
            characterAnimation.WallHangAnimation(OnWallForward());

            if (rigidbody.velocity.y < 0.2f)
            {
                rigidbody.AddForce(-transform.up * 10, ForceMode.Force);
            }

            return false;
        }
    }
    //-----------------
    private bool wasWallrunning = false;
    private void CheckWallRun()
    {
        onWall_left = Physics.Raycast(transform.position, -transform.right, out leftWall_rayHit, wallRayCastDistance, wallMask);
        onWall_right = Physics.Raycast(transform.position, transform.right, out rightWall_rayHit, wallRayCastDistance, wallMask);

        if (canWallRun)
        {
            if ((onWall_right || onWall_left) && !isWallRunning)
            {
                rigidbody.interpolation = RigidbodyInterpolation.None;
                WallRun();
                WallRunMovement();

                if (wallNormal != rightWall_rayHit.normal || wallNormal != leftWall_rayHit.normal)//if not on same wall as previus
                {
                    if (onWall_right)
                    {
                        wallNormal = rightWall_rayHit.normal;
                        onWall_left = false;
                    }
                    else if (onWall_left)
                    {
                        wallNormal = leftWall_rayHit.normal;
                        onWall_right = false;
                    }
                    characterAnimation.WallRun(onWall_left, onWall_right);

                }
            }
        }

        if ((!onWall_right || !onWall_left) && isWallRunning)
        {
            if (wasWallrunning)
            {
                ExitWallRun();
                wasWallrunning = false;
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

            forceDirection.y = 1f;
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
        characterAnimation.WallRun(false, false);
        characterAnimation.ExitWallRun();
        canDoubleJump = true;
        isWallRunning = false;

        if (isSprinting)
        {
            movementForce = og_movementForce * sprintMultiplier;
        }
        else
        {
            movementForce = og_movementForce;
        }

        StartCoroutine(DoWallRunCooDdown());
    }
    public void WallJump()
    {
        if (onWall_left)
        {
            forceDirection += transform.right * wallRunExitJumpForce;
            forceDirection += transform.up * doubleJumpForce;
        }
        if (onWall_right)
        {
            forceDirection += -transform.right * wallRunExitJumpForce;
            forceDirection += transform.up * doubleJumpForce;
        }
    }
    IEnumerator DoWallRunCooDdown()
    {
        if (isWallRunning)
        {
            isWallRunning = false;
            canWallRun = false;
            yield return new WaitForSeconds(wallRunCooldown);

            wallNormal = Vector3.zero;
            canWallRun = true;
        }
    }
    //-----------------
    private bool CheckandTryVault()
    {
        if (!isGroundedState && !onWall_left && !onWall_right && OnWallForward())
        {
            if (Physics.Raycast(vaultfinder.position, -vaultfinder.up, out var secondHit, wallDownDistanceCheck, wallMask))
            {
                characterAnimation.DoVaultAnimation();//start vault
                StartCoroutine(LerpVault(secondHit.point, vaultDuration));
            }

            return true;//on wall hang
        }
        else
        {
            return false;
        }
    }
    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        targetPosition = new Vector3(transform.position.x, targetPosition.y + 1.3f, transform.position.z);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
    private bool OnWallForward()
    {
        if (!isGroundedState && !onWall_left && !onWall_right && Physics.Raycast(transform.position, transform.forward, out var firstHit, wallForwardDistanceCheck, wallMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
