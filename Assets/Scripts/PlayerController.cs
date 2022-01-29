using System;
using UnityEngine;

[RequireComponent(typeof(GroundDrawer))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerController other;
    [SerializeField] bool startActive;
    [SerializeField] PlayerAnimator playerAnimator;

    GroundDrawer groundDrawer;
    MovementController movementController;
    Rigidbody2D rigidbody2D;
    float horizontalMove;
    bool jump = false;
    bool swap = false;

    bool active;

    float lastX;

    bool IsGone { get; set; }

    public event Action onSwapEvent;
    public event Action onPortalEnter;
    public event Action onDied;

    public void OnPortalEnter()
    {
        IsGone = true;
        Swap();
        onPortalEnter?.Invoke();

        LevelManager.Instance.PlayerEnteredPortal(groundDrawer.PlayerType);

        Invoke(nameof(Deactivate), 1f);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        if (!other)
        {
            Debug.LogWarning("No other player set for " + gameObject.name);
        }

        groundDrawer = GetComponent<GroundDrawer>();
        movementController = GetComponent<MovementController>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        SetActive(startActive);

        lastX = transform.position.x;
    }

    void SetActive(bool newActive)
    {
        active = newActive;
        movementController.SetActive(newActive);
        groundDrawer.SetActive(newActive);
        jump = false;

        movementController.onLandEvent += () => groundDrawer.ResetContinuous();
        movementController.onJumpEvent += () =>
        {
            if (movementController.IsGrounded())
            {
                groundDrawer.DrawColorUnder();
            }
        };
    }

    void Update()
    {
        if (!active) return;

        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Swap"))
        {
            swap = true;
        }

        if (movementController.IsGrounded() && !Mathf.Approximately(lastX, transform.position.x))
        {
            groundDrawer.Draw(horizontalMove);
        }

        playerAnimator.SetVelocity(horizontalMove);

        lastX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (!active) return;

        if (movementController.IsGrounded() && groundDrawer.IsOnDeadly)
        {
            Die();
            return;
        }

        movementController.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        if (swap && !other.IsGone)
        {
            Swap();
        }
    }

    void Swap()
    {
        SetActive(false);
        other.SetActive(true);
        swap = false;
        onSwapEvent?.Invoke();
    }

    void Die()
    {
        onDied?.Invoke();
        SetActive(false);
        LevelManager.Instance.PlayerDied(groundDrawer.PlayerType);
    }
}