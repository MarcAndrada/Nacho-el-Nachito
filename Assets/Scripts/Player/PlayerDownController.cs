using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownController : MonoBehaviour
{
    [HideInInspector]
    public bool goingDown;
    private bool _collideOneWay;
    private OneWayPlatformColision _oneWayPlatform;

    private void Start()
    {
        _collideOneWay = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OneWayPlatform"))
        {
            _collideOneWay = true;
            _oneWayPlatform = other.gameObject.GetComponent<OneWayPlatformColision>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OneWayPlatform"))
        {
            _collideOneWay = false;
            _oneWayPlatform = null;
        }
    }
    public void CheckIfCanGoOneWayPlat()
    {
        if (_collideOneWay && goingDown)
        {
            _oneWayPlatform.Activate();
        }
    }
}

