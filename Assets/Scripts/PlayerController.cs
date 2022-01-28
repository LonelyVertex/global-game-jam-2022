using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 40f;
    
    MovementController movementController;
    float horizontalMove;
    bool jump = false;
    
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        movementController.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}