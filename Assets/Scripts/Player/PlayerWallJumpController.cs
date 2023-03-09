using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpController : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerController _playerController;
    private PlayerInput _playerInput;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;

    // WALL SLIDE VARIABLES

    private bool isWallSliding;
    public float wallSlidingSpeed = 0.2f;

    [SerializeField]
    private Transform RightWallcheck;

    [SerializeField]
    private Transform LeftWallCheck;

    [SerializeField]
    private LayerMask wallLayer;

    // WALL JUMP VARIABLES

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _playerController = GetComponent<PlayerController>();
        _playerInput = GetComponent<PlayerInput>();

        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        WallSlide();

        WallJump();
    }

    private bool IsWalled() // NECESITO QUE SI IsWalled != null el input del player se cancele y no se pueda mover mientras esta en ese estado
    {
        if(Physics2D.OverlapCircle(RightWallcheck.position, 0.2f, wallLayer) != null)
        {
            return Physics2D.OverlapCircle(RightWallcheck.position, 0.2f, wallLayer);
        }
        else if(Physics2D.OverlapCircle(LeftWallCheck.position, 0.2f, wallLayer) != null)
        {
            return Physics2D.OverlapCircle(LeftWallCheck.position, 0.2f, wallLayer);
        }
        else
        {
            return false;
        }
    }

    private void WallSlide()
    {
        if(IsWalled() && !_movementController._isGrounded) //&& _playerInput._playerMovement != 0
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;

            if(sprt.flipX == false)
            {
                wallJumpingDirection = 1;
            }
            else
            {
                wallJumpingDirection = -1;
            }

            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb2d.velocity = new Vector2(10000, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(sprt.flipX == true)
            {
                sprt.flipX = false;
            }
            else if (sprt.flipX == false)
            {
                sprt.flipX = true;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
