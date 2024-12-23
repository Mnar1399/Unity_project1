using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool facingRight = true;
    private Vector2 lastMove = Vector2.down; // Last Move Direction // Default direction is Down Animation //

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
        // Animation Parameter
        anim.SetFloat("Horizontal", moveInput.x);
        anim.SetFloat("Vertical", moveInput.y);
        anim.SetFloat("Speed", moveInput.sqrMagnitude);

        if(moveInput.sqrMagnitude > 0.01f) // Player is moving
        {
            lastMove = moveInput; // Update the last move direction
        }
        else
        {
            if(Mathf.Abs(lastMove.y) > MathF.Abs(lastMove.x))
            {
                if(lastMove.y > 0)
                {
                    anim.Play("Up Idle");
                }
                else
                {
                    anim.Play("Down Idle");
                }
            }
            else
            {
                if (lastMove.x > 0)
                {
                    anim.Play("Right Idle");
                }
                else
                {
                    anim.Play("Left Idle");
                }
            }
        }
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read the movement input
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && !isDodging)
        {
            StartCoroutine(Dodge()); // Dodge
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

   /* private void Flip()
    {
        if ((moveInput.x < 0 && facingRight) || (moveInput.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f; // Flip the sprite
            transform.localScale = localScale;
        }
    }*/
}
