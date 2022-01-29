using UnityEngine;

[RequireComponent(typeof(GroundDrawer))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerController other;
    [SerializeField] bool startActive;

    GroundDrawer groundDrawer;
    MovementController movementController;
    Rigidbody2D rigidbody2D;
    float horizontalMove;
    bool jump = false;
    bool swap = false;

    bool active;

    float lastX;

    bool IsGone { get; set; }

    public void OnPortalEnter()
    {
        IsGone = true;
        gameObject.SetActive(false);
        Swap();

        LevelManager.Instance.PlayerEnteredPortal(groundDrawer.PlayerType);
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
        movementController.onJumpEvent += () => groundDrawer.DrawColorUnder();
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

        lastX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (!active) return;

        if (Mathf.Approximately(rigidbody2D.velocity.y, 0) && groundDrawer.IsOnDeadly)
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
    }

    void Die()
    {
        SetActive(false);
        LevelManager.Instance.PlayerDied(groundDrawer.PlayerType);
    }
}