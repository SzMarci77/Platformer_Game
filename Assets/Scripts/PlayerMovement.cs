using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDir))]

public class PlayerMovement : MonoBehaviour
{
    public float jumpStrength = 10f;
    public float walkSpeed = 5f;
    public float airWalkSpeed = 3f;
    public float runSpeed = 8f;
    Vector2 moveInput;
    TouchingDir touchingDirections;

    public float CurrentMoveSpeed
    {
        get
        {
            //Földön
            if (touchingDirections.IsGrounded)
            {
                return IsRunning ? runSpeed : walkSpeed;
            }
            //Falon
            else if (touchingDirections.IsOnWall) 
            {
                return airWalkSpeed;
            }
            //Levegőben
            else
            {
                return airWalkSpeed;
            }
        }
    }


    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        }
        private set 
        {
            _isMoving = value;
            animator.SetBool(Animations.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning { get 
        {
        return _isRunning;
        } 
        private set
        {
            _isRunning = value;
            animator.SetBool(Animations.isRunning, value);
        }
    }

    public bool IsFacingRight { get 
        {
            return _isFacingRight;
         }
        private set 
        {
            if (_isFacingRight != value)
            {
                //Flip direction
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    public bool _isFacingRight = true;
   
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDir>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(Animations.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
      moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            //Facing right
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            //Facing left
            IsFacingRight= false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Földön van-e a játékos + életben
        if(context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(Animations.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }
}
