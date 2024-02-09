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
    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInputActionAsset = new ThirdPersonInput();
        baseMaxSpeed = currentMaxSpeed;
    }

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

        IsGrounded();
    }

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

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            characterAnimation.DoJump();
            forceDirection += Vector3.up * jumpForce;
        }
    }

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
    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundcheckRaycastDistance))
        {
            characterAnimation.ExitFallAnimation();
            return true;
        }
        else
        {
            characterAnimation.DoFallAnimation();
            return false;
        }
    }
}
