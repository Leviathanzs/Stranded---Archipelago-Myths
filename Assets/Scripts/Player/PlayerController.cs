using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;
    new Transform transform;
    [SerializeField] HealthBar healthBar;
    [SerializeField] InventoryInput inventoryInput;

    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float airWalkSpeed = 3f;
    [SerializeField] float jumpImpulse = 10f;
    [SerializeField] float attackImpulse = 0.2f;
    [SerializeField] bool _isMoving = false;
    [SerializeField] bool _isRunning = false;
    [SerializeField] bool _isFacingRight = true;
    [SerializeField] bool isJumping = false;

    //get set method
    public float CurrentMoveSpeed 
    {
        get
        {
            if(CanMove && IsAlive)
            {
                    if(IsMoving && !touchingDirections.IsOnWall)
                    {
                        if(touchingDirections.IsGrounded)
                        {
                            if(IsRunning)
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
                            return airWalkSpeed;
                        }
                    }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            } 
        }
    }

    public bool IsRunning{get {return _isRunning;} private set {
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }}

    public bool IsMoving {get {return _isMoving;} private set {
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }}

    public bool IsFacingRight {get {return _isFacingRight;} private set {
        if(_isFacingRight != value)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
        _isFacingRight = value;
    }}

    public bool CanMove
    {
        get {
            return animator.GetBool(AnimationStrings.canMove); 
        } 
    }

    public bool IsAlive 
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }


    //get set method end

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        transform = GetComponent<Transform>();
    }

    private void FixedUpdate() 
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if(touchingDirections.IsGrounded)
        {
            isJumping = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        if(IsAlive)
        {
            SetFacingDirection(moveInput);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        } 
    }

    void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && !inventoryInput.IsOpen)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);

            if(IsFacingRight)
            {
                transform.position = new Vector2(transform.position.x + attackImpulse, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - attackImpulse, transform.position.y);
            }
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        healthBar.SetHealth(damageable.Health);
    }
}
