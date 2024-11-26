using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArena : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponent<Animator>();
    }
    private void Die()
    {
        Debug.Log("You died! ");
        
        Debug.Log("player is dead!!");
    }

    public void DestroyPlayer()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
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
            anim.SetTrigger("IsDied");
        }
    }

}
