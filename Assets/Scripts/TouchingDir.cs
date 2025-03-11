using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Player on ground/walls/ceiling
public class TouchingDir : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    public ContactFilter2D castFilter;
    public float groundDist = 0.05f;
    public float wallDist = 0.2f;
    public float ceilingDist = 0.05f;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    Animator animator;

    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded { get 
        {
            return _isGrounded;
        } private set
        { 
            _isGrounded = value;
            animator.SetBool(Animations.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(Animations.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;
    private Vector2 wallCheckDir => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(Animations.isOnCeiling, value);
        }
    }
    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDist) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDir, castFilter, wallHits, wallDist) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDist) > 0;
    }
}
