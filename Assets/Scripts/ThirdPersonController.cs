using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{

    //input
    private ThirdPersonInput playerInputActionAsset;
    private InputAction move;

    //movement fields
    private Rigidbody rigidbody;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 4f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        playerInputActionAsset = new ThirdPersonInput();
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
        if(horzontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rigidbody.velocity = horzontalVelocity.normalized * maxSpeed + Vector3.up * rigidbody.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rigidbody.velocity;
        direction.y = 0;

        //rotate rigidbody in move direction
        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
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
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
