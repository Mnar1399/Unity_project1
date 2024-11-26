using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerAttack : MonoBehaviour
{
    public InputActionAsset inputActions; //connect it to the input system
    public float attackRange = 1.5f;
    public int damage = 5;

    void Start()
    {
        var playerActions = inputActions.FindActionMap("Player");

        playerActions.FindAction("Attack").performed += OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
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


    private void OnDrawGizmosSelected()
    {
        // Draw the attack range when the object is selected in the scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
