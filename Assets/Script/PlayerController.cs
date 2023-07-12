using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpImpulse = 10f;
    public float airwalkSpeed = 3f;
    Vector2 moveInput;
    TouchingDirection touchingDirection;

    public float CurrentMoveSpeed { get
        {
            if(canMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (touchingDirection.IsGrounded)
                    {
                        if (IsMoving)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        // Air Move
                        return airwalkSpeed;
                    }
                }
                else
                {
                    // Idle speed is 0
                    return 0;
                }
            } else
            {
                // Movement locked
                return 0;
            }
           
        } }





    

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.IsMoving, value); // sering error // i kapital bukan i kecil
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set{
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 2);
            }
            _isFacingRight = value;

        } }

    public bool canMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
            }
   
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    } 


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

      //  SetFacingDirection(moveInput);
    }

    // hadap belakang & hadap depan--------------------------
    /* private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    } */
    // ---------------------------------------------

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsMoving = true;
        }   else if (context.canceled)
        {
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && canMove )
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        } 
    }

    public void OnAttack (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
