using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDir), typeof(Damageable))]
public class KnightEnemy : MonoBehaviour
{
    public DetectionZone attackZone;
    public float walkSpeed = 3f;
    public float walkStopRate = 0.06f;

    Rigidbody2D rb;
    TouchingDir touchingDir;
    Animator animator;
    Damageable damageable;

    public enum WalkingDirection { Right, Left };
    public Vector2 walkDirectionVector = Vector2.right;
    private WalkingDirection _walkDirection;


    public WalkingDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value) 
            {
                //Flip direction
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                
                    if(value == WalkingDirection.Right)
                    {
                    walkDirectionVector = Vector2.right;
                    }
                    else if(value == WalkingDirection.Left)
                    {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }

    public bool _hastarget = false;
    public bool HasTarget
    {
        get
        {
            return _hastarget;
        }
    private set
        {
            _hastarget = value;
            animator.SetBool(Animations.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(Animations.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDir = GetComponent<TouchingDir>();
        animator = GetComponent<Animator>();
        animator.SetBool(Animations.canMove, true);
        damageable = GetComponent<Damageable>();

    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }


    private void FixedUpdate()
    {
        if (touchingDir.IsGrounded && touchingDir.IsOnWall)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }


        
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkingDirection.Right)
        {
            WalkDirection = WalkingDirection.Left;
        }
        else if (WalkDirection == WalkingDirection.Left)
        {
            WalkDirection = WalkingDirection.Right;
        }
        else
        {
            Debug.LogError("Nincs helyes érték megadva: left/right");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
