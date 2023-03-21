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
    

    [SerializeField] private float dashDistance;
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
        //
    }
}
