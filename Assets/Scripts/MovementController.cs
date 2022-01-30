using System;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float jumpForce = 400f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    [SerializeField] bool airControl;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheckLeft;
    [SerializeField] Transform groundCheckRight;
    [SerializeField] float runSpeed = 30f;

    const float groundedRadius = .2f;
    bool grounded = true;
    bool groundedOnTheGround;
    bool justJumped = false;
    Rigidbody2D rigidbody2D;
    Vector3 velocity = Vector3.zero;

    bool active;
    Vector3 velocityBeforeDeactivate;

    public event Action onLandEvent;
    public event Action onJumpEvent;

    public bool IsGrounded()
    {
        return groundedOnTheGround;
    }

    public void SetActive(bool newActive)
    {
        active = newActive;

        if (newActive)
        {
            velocity = velocityBeforeDeactivate;
            rigidbody2D.velocity = velocityBeforeDeactivate;
            rigidbody2D.isKinematic = false;
        }
        else
        {
            velocityBeforeDeactivate = rigidbody2D.velocity;
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.isKinematic = true;
        }
    }

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheckLeft.position, groundedRadius);
        Gizmos.DrawWireSphere(groundCheckRight.position, groundedRadius);
    }

    void FixedUpdate()
    {
        if (!active) return;

        var wasGrounded = grounded;

        var collidersLeft = Physics2D.OverlapCircleAll(groundCheckLeft.position, groundedRadius, whatIsGround);
        var collidersRight = Physics2D.OverlapCircleAll(groundCheckRight.position, groundedRadius, whatIsGround);
        var allColliders = collidersLeft.Union(collidersRight).ToList();
        
        groundedOnTheGround = !justJumped && allColliders.Any(col => col.gameObject.layer == LayerMask.NameToLayer("Ground"));
        grounded = !justJumped && (groundedOnTheGround || allColliders.Any(col => col.gameObject != gameObject));

        if (grounded && !wasGrounded)
        {
            onLandEvent?.Invoke();
        }

        justJumped = false;
    }

    public void Move(float move, bool jump)
    {
        if (!active) return;

        if (grounded || airControl)
        {
            var targetVelocity = new Vector2(move * runSpeed * 10f, rigidbody2D.velocity.y);
            rigidbody2D.velocity =
                Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        if (grounded && jump)
        {
            grounded = false;
            justJumped = true;
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, 0);
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            onJumpEvent?.Invoke();
        }
    }
}