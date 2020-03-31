using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : CharacterController2D
{
    private float JumpForce = 14f;
    private float MoveSpeed = 7f;

    private Animator Animator;

    // Start is called before the first frame update
    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 Move = Vector2.zero;

        Move.x = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && IsGrounded)
        {
            Velocity.y = JumpForce;
        }

        Animator.SetFloat("Speed", (Velocity.x)/MoveSpeed);
        Animator.SetBool("IsGrounded", IsGrounded);
        if (Animator.GetFloat("Speed") < -0.01)
        {
            Animator.SetBool("IsFacingRight", false);
        }
        else if(Animator.GetFloat("Speed") > 0.01)
        {
            Animator.SetBool("IsFacingRight", true);
        }
        else if(Animator.GetFloat("Speed") == 0 && Animator.GetBool("IsFacingRight") == true)
        {
            Animator.SetBool("IsFacingRight", true);
        }
        else
        {
            Animator.SetBool("IsFacingRight", false);
        }

        TargetVelocity = Move * MoveSpeed;
    }
}
