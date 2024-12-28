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
        if (context.performed)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        Vector2 attackDirection = playerMovement.lastMoveDirection;

        // OverlapCircle to detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            transform.position + (Vector3)attackDirection * attackRange,
            attackRange
        );

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

        // Update animation
        anim.SetFloat("Horizontal", attackDirection.x);
        anim.SetFloat("Vertical", attackDirection.y);
        anim.SetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (playerMovement != null)
        {
            Vector3 attackPosition = transform.position + (Vector3)playerMovement.lastMoveDirection * attackRange;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition, attackRange);
        }
    }
}
