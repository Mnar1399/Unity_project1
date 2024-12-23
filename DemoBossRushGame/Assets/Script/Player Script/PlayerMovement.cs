using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public InputActionAsset inputActions;
    private Animator anim;

    public float moveSpeed = 5f;
    public float dodgeSpeed = 15f;
    public float dodgeDuration = 0.2f;

    private bool isDodging = false;
    public Vector2 lastMoveDirection = Vector2.down; // Default direction is Down

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var playerActions = inputActions.FindActionMap("Player");
        playerActions.FindAction("Move").performed += OnMove;
        playerActions.FindAction("Move").canceled += OnMove;
        playerActions.FindAction("Dodge").performed += OnDodge;

        playerActions.Enable();
    }

    void FixedUpdate()
    {
        if (!isDodging && moveInput != Vector2.zero)
        {
            Vector2 move = moveInput * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    void Update()
    {
        // Set animator parameters
        anim.SetFloat("Horizontal", moveInput.x);
        anim.SetFloat("Vertical", moveInput.y);
        anim.SetFloat("Speed", moveInput.sqrMagnitude);

        // Check if the player is moving
        if (moveInput.sqrMagnitude > 0.01f)
        {
            // Update the last move direction
            lastMoveDirection = moveInput;
        }
        else
        {
            // Handle directional idle based on the last move direction
            if (Mathf.Abs(lastMoveDirection.y) > Mathf.Abs(lastMoveDirection.x))
            {
                // Vertical direction takes precedence
                if (lastMoveDirection.y > 0)
                    anim.Play("Up Idle");
                else
                    anim.Play("Down Idle");
            }
            else
            {
                // Horizontal direction
                if (lastMoveDirection.x > 0)
                    anim.Play("Right Idle");
                else
                    anim.Play("Left Idle");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && !isDodging)
        {
            StartCoroutine(Dodge());
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        Vector2 dodgeDirection = moveInput != Vector2.zero ? moveInput.normalized : lastMoveDirection;
        rb.velocity = dodgeDirection * dodgeSpeed;
        yield return new WaitForSeconds(dodgeDuration);
        rb.velocity = Vector2.zero;
        isDodging = false;
    }
}
