using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDashController : MonoBehaviour
{
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;
    
    // DASH VARIABLES

    public bool _canDash;

    private Vector2 vdirection;
    public Vector2 _dashDirection;
    [HideInInspector] public  Vector2 dashDir;
    public bool _isDirectional;
    
    [SerializeField] private float _dashSpeed = 25f;
    [SerializeField] private float _dashTime = 0.2f;
    private float _dashTimePassed = 0f;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        
        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        Debug.Log(_dashDirection);
    }

    public void Dash()
    {
        vdirection = (Vector2)transform.right * _playerController._movementController._lastDir * _dashSpeed;
        if (_isDirectional)
        {
            vdirection = dashDir * _dashSpeed;
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