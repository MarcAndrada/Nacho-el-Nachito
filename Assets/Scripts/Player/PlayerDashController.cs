using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDashController : MonoBehaviour
{
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;
    
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
        vdirection = (Vector2)transform.right * _playerController._movementController._lastDir * _dashSpeed;
        if (_isDirectional)
        {
            vdirection = _dashDirection * _dashSpeed;
        }
        rb2d.velocity = vdirection;
        
        _canDash = false;
        // directional dash like celeste
           
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
    
}