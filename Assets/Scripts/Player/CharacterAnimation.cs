using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Animator hitboxAnimator;
    [SerializeField]
    private Rigidbody rigidbody;
    private float maxSpeed = 5f;

    void Update()
    {
        animator.SetFloat("Speed", rigidbody.velocity.magnitude / maxSpeed);
    }

    public void DoJump()
    {
        animator.SetTrigger("Jump");
    }
    public void DoFallAnimation()
    {
        animator.SetBool("IsGrounded", false);
    }
    public void ExitFallAnimation()
    {
        animator.SetBool("IsGrounded", true);
    }
    public void DoSlideAnimation(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("Slide");
            hitboxAnimator.SetTrigger("Slide");
        }
    }
    public void WallRun(bool onWallLeft, bool onWallRight)
    {
        if (onWallLeft)
        {
            animator.SetBool("WallRunRight", false);
            animator.SetBool("WallRunLeft", true);
        }
        if (onWallRight)
        {
            animator.SetBool("WallRunLeft", false);
            animator.SetBool("WallRunRight", true);
        }
        else if (!onWallLeft && !onWallRight)
        {
            ExitWallRun();
        }
    }
    public void ExitWallRun()
    {
        animator.SetBool("WallRunLeft", false);
        animator.SetBool("WallRunRight", false);
    }
    public void DoDoubleJumpAnimation()
    {
        animator.SetTrigger("DoubleJump");
    }
    public void DoVaultAnimation()
    {
        animator.SetTrigger("Vault");
    }
    public void WallHangAnimation(bool onWallForward)
    {
        if (onWallForward)
        {
            animator.SetTrigger("EnterWallHang");
        }
        else
        {
            animator.SetTrigger("ExitWallHang");
        }
    }
    //-----
    public void PlayMusic1()
    {
        animator.SetFloat("MusicPlayingLevel", 0);
    }
    public void PlayMusic2()
    {
        animator.SetFloat("MusicPlayingLevel", 2);
    }
    public void PlayMusic3()
    {
        animator.SetFloat("MusicPlayingLevel", 3);
    }
}
