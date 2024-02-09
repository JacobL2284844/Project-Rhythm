using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rigidbody;
    private float maxSpeed = 5f;
    void Update()
    {
        animator.SetFloat("Speed", rigidbody.velocity.magnitude / maxSpeed);
    }
}
