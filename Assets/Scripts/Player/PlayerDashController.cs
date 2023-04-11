using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // directional dash like celeste
           
    }

    public void DashTimer()
    {
        _dashTimePassed += Time.deltaTime;
        if (_dashTimePassed >= _dashTime)
        {
            if (CheckWall())
            {
                
            }
            float _ySpeed = Mathf.Clamp(rb2d.velocity.y, -5, 5);
            rb2d.velocity = new Vector2(rb2d.velocity.x, _ySpeed);
            _playerController.playerState = PlayerController.PlayerStates.NONE;
            _dashTimePassed = 0;
        }
    }

    private bool CheckWall()
    {
        return ((Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0), 0.2f, floorLayer) != null ||
                 Physics2D.OverlapCircle(transform.position - new Vector3(coll.size.x / 2, 0), 0.2f, floorLayer) != null) &&
                !Physics2D.Raycast(transform.position - new Vector3(0, coll.size.y / 2), Vector2.down, 0.2f,
                    floorLayer));
    }
}