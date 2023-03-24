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

    [SerializeField] private float _dashSpeed = 5f;
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
        vdirection = (Vector2)transform.right * _dashSpeed * _playerController._movementController._lastDir;
        rb2d.velocity = vdirection;
    }

    public void DashTimer()
    {
        _dashTimePassed += Time.deltaTime;
        if (_dashTimePassed >= _dashTime)
        {
            _playerController.playerState = PlayerController.PlayerStates.NONE;
            _dashTimePassed = 0;
        }
    }
    
}
