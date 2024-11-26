using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public InputActionAsset inputActions; //connect it to the input system
    private Animator anim;

    public float moveSpeed = 5f;
    public float dodgeSpeed = 15f;
    public float dodgeDuration = 0.2f;

    private bool isDodging = false;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var playerActions = inputActions.FindActionMap("Player");
        playerActions.FindAction("Move").performed += OnMove;
        playerActions.FindAction("Move").canceled += OnMove; // Cancel the movement when the button is released
        playerActions.FindAction("Dodge").performed += OnDodge;

        playerActions.Enable(); // Activate the action map
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            if (moveInput != Vector2.zero)
            {
                Vector2 move = moveInput * moveSpeed * Time.deltaTime; // Calculate the input
                rb.MovePosition(rb.position + move); // Move the player
            }
        }
    }

    void Update()
    {
        // Trigger run animation when the player is moving
        anim.SetBool("isMoving", moveInput != Vector2.zero);

        // Flip the player sprite horizontally based on horizontal input
        Flip();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read the movement input
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && !isDodging)
        {
            StartCoroutine(Dodge()); // Perform the dodge
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        rb.velocity = moveInput.normalized * dodgeSpeed;
        yield return new WaitForSeconds(dodgeDuration);
        rb.velocity = Vector2.zero;
        isDodging = false;
    }

    private void Flip()
    {
        // Only flip if moving left or right
        if ((moveInput.x < 0 && facingRight) || (moveInput.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f; // Flip the sprite by inverting the X scale
            transform.localScale = localScale;
        }
    }
}
