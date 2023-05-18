using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    
    private bool _collideLever;
    private LeverManager _lever;
    private void Start()
    {
        _collideLever = false;
    }

    // check if player collides with lever
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mechanism"))
        {
            _collideLever = true;
            _lever = other.gameObject.GetComponent<LeverManager>();
            _lever.ShowLeverButton();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mechanism"))
        {
            _collideLever = false;
            _lever.HideLeverButton();
            _lever = null;
        }
    }

    public void Interact()
    {
        if (_collideLever)
        {
            Debug.Log("Collide with lever");
            _lever.ActivateLever();
        }
        else
        {
            Debug.Log("Colliding with nothing");
        }
    }
}
