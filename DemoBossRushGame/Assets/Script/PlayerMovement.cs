using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
   public float moveSpeed=5f;
   public float dodgeSpeed=15f;
   public float dodgeDuration=0.2f;
   public InputActionAsset inputActions; //connect it to the input system

  public float circleRadius = 5f; // radius of circle
   private Vector2 moveInput;
   private Rigidbody2D rb;
   private bool isDodging=false;
   public float attackRange=1.5f;
  public int damage = 5; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();



        var playerActions = inputActions.FindActionMap("Player");
        playerActions.FindAction("Move").performed += OnMove;
        playerActions.FindAction("Move").canceled += OnMove;//cancel the player move if he dose not press the button
        playerActions.FindAction("Dodge").performed += OnDodge;
        playerActions.FindAction("Attack").performed += OnAttack;
      playerActions.Enable(); //activate the action map

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDodging){
             if (moveInput != Vector2.zero){
            Vector2 move = moveInput * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position+move);}
        }
    }

public void OnMove(InputAction.CallbackContext context)
{
    moveInput = context.ReadValue<Vector2>();
}

public void OnDodge(InputAction.CallbackContext context)
{
   if(context.performed && !isDodging){
    StartCoroutine(Dodge());
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

private void OnTriggerExit2D(Collider2D other)
{
    
    if(other.gameObject.CompareTag("CircleBoundary")){
        Die();
    }
}

private void Die()
{
    gameObject.SetActive(false); //player hides
    Debug.Log("player is dead!!");
}

private void OnAttack(InputAction.CallbackContext context)
{
   if(context.performed )
   {
    Debug.Log("Attack performed!");
    PerformAttack();
   }
}
 private void PerformAttack()
{
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

    Debug.Log("Enemies in range: " + hitEnemies.Length); 

    foreach (Collider2D enemy in hitEnemies)
    {
        if (enemy.CompareTag("Enemy"))
        {
            Debug.Log("Found an enemy with the 'Enemy' tag.");
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy attacked!");
            }
            else
            {
                Debug.Log("Enemy does not have an EnemyHealth component.");
            }
        }
        else
        {
            Debug.Log("Found object, but it does not have the 'Enemy' tag.");
        }
    }
}


 private void  OnDrawGizmosSelected()
 {
    // Draw the attack range when the object is selected in the scene
    Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
 }


}
