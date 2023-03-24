using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpController : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;

    // WALL SLIDE VARIABLES

    public bool isWallSliding;
    public float wallSlidingSpeed = 0.2f;

    [SerializeField]
    private LayerMask wallLayer;

    // WALL JUMP VARIABLES
    private float wallJumpingDirection;

    [SerializeField]
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _playerController = GetComponent<PlayerController>();

        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll =  GetComponent<CapsuleCollider2D>();
    }

    private bool IsWalled()
    {
        if(Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer) != null)
        {
            return Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer);
        }
        else if(Physics2D.OverlapCircle(transform.position - new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer) != null)
        {
            return Physics2D.OverlapCircle(transform.position - new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer);
        }
        else
        {
            return false;
        }
    }

    public void WallSlide()
    {
        if(IsWalled() && !_movementController._isGrounded)
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void WallJump()
    {
        if (sprt.flipX == true)
        {
            sprt.flipX = false;
        }
        else if (sprt.flipX == false)
        {
            sprt.flipX = true;
        }

        if (sprt.flipX == false)
        {
            wallJumpingDirection = 1;
        }
        else
        {
            wallJumpingDirection = -1;
        }

        _playerController.playerState = PlayerController.PlayerStates.AIR;
        isWallSliding = false;
        rb2d.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        _movementController.externalForces = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    }
}
