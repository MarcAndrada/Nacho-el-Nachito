using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class PlayerDashController : MonoBehaviour
{
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;
    
    [SerializeField]
    private LayerMask floorLayer;
    
    // DASH VARIABLES

    public bool _canDash;

    private Vector2 vdirection;
    [HideInInspector] public  Vector2 _dashDirection;
    public bool _isDirectional;
    
    [SerializeField] private float _dashSpeed = 28f;
    [SerializeField] private float _dashTime = 0.3f;
    private float _dashTimePassed = 0f;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        
        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    public void Dash()
    {
        if (_dashDirection != Vector2.zero)
        {
            vdirection = _dashDirection * _dashSpeed;
        }
        else
        {
            vdirection = (Vector2)transform.right * _playerController._movementController._lastDir * _dashSpeed;
        }
        
        rb2d.velocity = vdirection;

        _canDash = false;
           
    }

    public void DashTimer()
    {
        _dashTimePassed += Time.deltaTime;
        if (_dashTimePassed >= _dashTime)
        {
            float _ySpeed = Mathf.Clamp(rb2d.velocity.y, -5, 5);
            rb2d.velocity = new Vector2(rb2d.velocity.x, _ySpeed);
            _playerController.playerState = PlayerController.PlayerStates.NONE;
            _dashTimePassed = 0;
        }
    }

    public void DashCheckWall()
    {
        if (Physics2D.Raycast(transform.position, transform.right * _dashDirection.x, 1, floorLayer))
        {
            // Stop Dash
        }
        else
        {
            bool hitUp = Physics2D.Raycast(transform.position + new Vector3(0, 0.55f), transform.right * _dashDirection.x + new Vector3(0.5f, 0.55f), 1, floorLayer);
            bool hitDown = Physics2D.Raycast(transform.position + new Vector3(0, 0.55f), transform.right * _dashDirection.x + new Vector3(0.5f, -0.55f), 1, floorLayer);

            if (hitUp && hitDown)
            {
                // Stop Dash
            }
            else if (hitUp)
            {
                // Move Down                
            }
            else if (hitDown)
            {
                // Move Up
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.55f) , transform.position + new Vector3(0, 0.55f) + transform.right * _dashDirection.x);
        Gizmos.DrawLine(transform.position , transform.position + transform.right * _dashDirection.x);
        Gizmos.DrawLine(transform.position + new Vector3(0, -0.55f) , transform.position + new Vector3(0, -0.55f) + transform.right * _dashDirection.x);
    }
}