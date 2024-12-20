using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement  : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Transform player; // Reference to the player
    public float attackRange=1.5f; // range at which enemy can attack

    void Update()
    {
         float distanceToPlayer= Vector2.Distance(transform.position,player.position);

         if(distanceToPlayer < attackRange)
         {
                Attack();
         }
         else
         {
             MoveTowardsPlayer();
         }


    }


    private void   MoveTowardsPlayer () 
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);


    }


    private void Attack () 
    {
        // Enter Attack
        // Debug.Log("Enmey attack the player");
    }




}
