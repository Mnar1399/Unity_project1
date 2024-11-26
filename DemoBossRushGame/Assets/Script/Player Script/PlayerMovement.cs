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

    public float moveSpeed=5f;
    public float dodgeSpeed=15f;
    public float dodgeDuration=0.2f;

    public float circleRadius = 5f; // radius of circle
    private bool isDodging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var playerActions = inputActions.FindActionMap("Player");
        playerActions.FindAction("Move").performed += OnMove;
        playerActions.FindAction("Move").canceled += OnMove;//cancel the player move if he dose not press the button
        playerActions.FindAction("Dodge").performed += OnDodge;
        
        playerActions.Enable(); //activate the action map

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDodging)
        {
            if (moveInput != Vector2.zero)
            {
                Vector2 move = moveInput * moveSpeed * Time.deltaTime; //  Calculate the input
                rb.MovePosition(rb.position+move); // Move 
            }
        }
    }

    public void Update()
    {
        
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read the movement input
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
       if(context.performed && !isDodging)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            Die();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false); //player hides
        Debug.Log("player is dead!!");
    }


}
