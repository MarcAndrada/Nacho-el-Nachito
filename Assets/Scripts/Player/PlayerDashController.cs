using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerDashController : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;
    
    // DASH VARIABLES

    public bool _canDash;

    private Vector2 vdirection;

    [SerializeField] private float _dashSpeed = 5f;
    [SerializeField] private float _dashTime = 0.3f;
    [SerializeField] private float _dashDistance = 5f;
    void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _playerController = GetComponent<PlayerController>();

        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll =  GetComponent<CapsuleCollider2D>();
        _canDash = true;
    }

    public void Dash()
    {
        vdirection = (Vector2)transform.right * _dashSpeed * _dashDistance * _movementController._lastDir;
        // 
        _playerController.GetComponent<Rigidbody2D>().velocity = vdirection;
        StartCoroutine(StopDashing());

    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashTime);
        _playerController.playerState = PlayerController.PlayerStates.NONE;
        
        
        
    }
}
