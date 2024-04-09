using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public Animator padAnimator;
    public float upForce;
    public float sprintForwardForce;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ThirdPersonController playerController = other.GetComponent<ThirdPersonController>();
            padAnimator.SetTrigger("Jump");

            playerController.characterAnimation.DoJump();
            playerController.forceDirection += Vector3.up * upForce;
        }
    }
}
