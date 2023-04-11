using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpController : MonoBehaviour
{
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprt;
    private CapsuleCollider2D coll;

    // WALL SLIDE VARIABLES

    public bool isWallSliding;
    public float wallSlidingSpeed;

    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private LayerMask floorLayer;
    // WALL JUMP VARIABLES
    private float wallJumpingDirection;

    [SerializeField]
    private Vector2 wallJumpingPower;

    // Start is called before the first frame update
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();

        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll =  GetComponent<CapsuleCollider2D>();
    }

    private bool IsWalled()
    {
        if(Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer) != null ||
            Physics2D.OverlapCircle(transform.position - new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer) != null)
        {
            if (!Physics2D.Raycast(transform.position - new Vector3(0,coll.size.y / 2), Vector2.down, 0.2f, floorLayer))
            {
                return true;
            }
        }

        return false;

    }

    private bool isWalledRight()
    {
        if (Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0), 0.2f, wallLayer) != null )
        {
            return true;
        }
        return false;

    }

    public void CheckIfWallSliding() 
    {
        if (IsWalled())
        {
            _playerController.playerState = PlayerController.PlayerStates.WALL_SLIDE;
            _playerController._playerDashController._canDash = true;
        }
    }


    public void WallSlide()
    {
        if(IsWalled())
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (!isWalledRight())
            {
                sprt.flipX = false;
                wallJumpingDirection = 1;
            }
            else
            {
                sprt.flipX = true;
                wallJumpingDirection = -1;
            }
        }
        else
        {
            isWallSliding = false;
            _playerController.playerState = PlayerController.PlayerStates.AIR;
        }
    }

    public void WallJump()
    {

        _playerController.playerState = PlayerController.PlayerStates.AIR;
        isWallSliding = false;
        Vector2 jumpDir = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        rb2d.velocity = jumpDir;
        _playerController._movementController.externalForces = jumpDir;
    }
}
