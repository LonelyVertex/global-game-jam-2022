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

    const float groundedRadius = .2f;
    bool grounded;
    Rigidbody2D rigidbody2D;
    Vector3 velocity = Vector3.zero;

    public event Action onLandEvent;

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
        var wasGrounded = grounded;

        var collidersLeft = Physics2D.OverlapCircleAll(groundCheckLeft.position, groundedRadius, whatIsGround);
        var collidersRight = Physics2D.OverlapCircleAll(groundCheckRight.position, groundedRadius, whatIsGround);

        grounded = collidersLeft.Union(collidersRight).Any(col => col.gameObject != gameObject);

        if (grounded && !wasGrounded)
        {
            onLandEvent?.Invoke();
        }
    }

    public void Move(float move, bool jump)
    {
        if (grounded || airControl)
        {
            var targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
            rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        if (grounded && jump)
        {
            grounded = false;
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, 0);
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }
}