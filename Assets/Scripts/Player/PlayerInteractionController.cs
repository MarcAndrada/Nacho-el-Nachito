using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private PlayerController _playerController;
    
    private bool collideLever;
    private LeverManager lever;
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        collideLever = false;
    }

    // check if player collides with lever
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mechanism"))
        {
            collideLever = true;
            lever = other.gameObject.GetComponent<LeverManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mechanism"))
        {
            collideLever = false;
            lever = null;
        }
    }

    public void Interact()
    {
        if (collideLever)
        {
            Debug.Log("Collide with lever");
            lever.ActivateLever();
        }
        else
        {
            Debug.Log("Colliding with nothing");
        }
    }
}
