using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArena : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Die()
    {
        Debug.Log("You died! ");
        // Death Animation

        gameObject.SetActive(false); //player hides
        Debug.Log("player is dead!!");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            Debug.Log("Welcome to the Arena! ");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            Die();
        }
    }

}
