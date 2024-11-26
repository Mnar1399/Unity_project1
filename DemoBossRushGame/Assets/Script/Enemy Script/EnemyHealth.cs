using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

 public int maxHelath=20;
 public int currentHealth; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHelath;
    }
   public void TakeDamage(int damage)
    {
     currentHealth -=damage;
     Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }

   private void Die()
    {
        
        Debug.Log("Enemy died!");
        gameObject.SetActive(false); 
    }

}
