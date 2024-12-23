using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    public InputActionAsset inputActions;

    public float attackRange = 1.5f;
    public int damage = 5;
    private bool isAttacking = false; // Prevent repeated attacks

    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        var playerActions = inputActions.FindActionMap("Player");
        playerActions.FindAction("Attack").performed += OnAttack;

        playerActions.Enable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            // PerformAttack();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isAttacking)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (isAttacking) return;

        Debug.Log("Attack On!");
        isAttacking = true; // Set flag to prevent repeated attacks

        Vector2 lastMoveDirection = playerMovement.lastMoveDirection;

        // Determine the attack animation based on movement direction
        if (Mathf.Abs(lastMoveDirection.y) > Mathf.Abs(lastMoveDirection.x))
        {
            if (lastMoveDirection.y > 0f)
            {
                anim.SetTrigger("UpAttack");
            }
            else
            {
                anim.SetTrigger("DownAttack");
            }
        }
        else
        {
            if (lastMoveDirection.x > 0f)
            {
                anim.SetTrigger("RightAttack");
            }
            else
            {
                anim.SetTrigger("LeftAttack");
            }
        }

        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + (Vector3)lastMoveDirection * attackRange, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }

    // Animation event functions to reset the attack flag
    public void AttackAnimationComplete()
    {
        isAttacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        // Draw the attack range when the object is selected in the sceneS
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)playerMovement.lastMoveDirection * attackRange, attackRange);
    }
}
