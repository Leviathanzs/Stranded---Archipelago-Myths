using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    Animator animator;
    ContactFilter2D castFilter;
    private string[] groundTags = {"Ground", "Enemies"};
    [SerializeField] float groundDistance = 0.01f;
    [SerializeField] float wallDistance = 0.02f;
    [SerializeField] float ceilingDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[10];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isOnWall;
    [SerializeField] private bool _isOnCeiling;


    public bool IsGrounded {get {return _isGrounded;} 
        set 
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }}
    
    public bool IsOnWall {get {return _isOnWall;} 
        private set 
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }}

    public bool IsOnCeiling {get {return _isOnCeiling;} 
        private set 
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }}
    

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left; 
    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    bool IsTagInList (string tag)
    {
        return System.Array.Exists(groundTags, element => element == tag);
    }

    void FixedUpdate() 
    {
        // Perform the cast to check for the ground (as before)
        IsGrounded = false;  // Default to not grounded

        if (touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0)
        {
            for (int i = 0; i < groundHits.Length; i++)
            {
                if (groundHits[i].collider != null && IsTagInList(groundHits[i].collider.tag))
                {
                    IsGrounded = true;
                    break;
                }
            }
        }

        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

        if (animator.GetFloat(AnimationStrings.yVelocity) > 0)
        {
            IsGrounded = false;
        }
    }
}
