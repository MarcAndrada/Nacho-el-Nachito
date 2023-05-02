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
    [Header("Wall Slide")]
    public bool isWallSliding;
    public float wallSlidingSpeed;

    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private float wallCheckRange;

    private float walledDir;
    [SerializeField]
    private float timeUnstickWall;
    private float timeWaitedUnstickWall = 0;

    // WALL JUMP VARIABLES
    [Header("Wall Jump"), SerializeField]
    private float airWallJumpRange;

    [SerializeField]
    private Vector2 wallJumpingPower;

    [Header("Sound"), SerializeField]
    private AudioClip walljumpSound;
    [SerializeField]
    private AudioClip wallSlide;
    private AudioSource loopAS;


    void Awake()
    {
        _playerController = GetComponent<PlayerController>();

        rb2d = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
        coll =  GetComponent<CapsuleCollider2D>();
    }

    private bool IsWalled()
    {
        float lookDir = _playerController._playerInput._playerMovement;
        bool isGrounded = Physics2D.Raycast(transform.position + new Vector3(0, -coll.size.y / 2), Vector2.down, 0.1f, floorLayer);
        //Si no esta en el muro comprueba en la direccion que se esta moviendo el personaje que no haya una pared, tambien que no este tocando el suelo
        if (!isWallSliding && 
            lookDir != 0 &&
            Physics2D.OverlapCircle(transform.position + (new Vector3(coll.size.x / 2, 0) * lookDir), wallCheckRange, wallLayer) && 
            !isGrounded) 
        {

            walledDir = lookDir;
            return true;
        }
        //O que si esta moviendose en una direccion y esta cerca del muro
        else if (!isWallSliding &&
                lookDir == 0 &&
                rb2d.velocity.x != 0 &&
                Physics2D.OverlapCircle(transform.position + (new Vector3(coll.size.x / 2, 0) * Mathf.Clamp(rb2d.velocity.x, -1f,1f)), wallCheckRange, wallLayer) &&
                !isGrounded)
        {

            walledDir = Mathf.Clamp(rb2d.velocity.x, -1f, 1f);
            return true;
        }
        //O si esta en el muro comrueba que lo siga tocando y que no este tocando el suelo
        else if (isWallSliding &&
                Physics2D.OverlapCircle(transform.position + new Vector3(coll.size.x / 2, 0) * walledDir, wallCheckRange * 2, wallLayer) &&
                !isGrounded)
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
            loopAS = AudioManager._instance.Play2dLoop(wallSlide);
        }
    }


    public void WallSlide()
    {
        if(IsWalled())
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (walledDir == 1)
            {
                sprt.flipX = true;
            }
            else if(walledDir == -1)
            {
                sprt.flipX = false;
            }
        }
        else
        {
            walledDir = 0;
            StopSlide();
        }
    }

    public void CheckIfStopSliding()
    {
         if (_playerController._playerInput._playerMovement == walledDir * -1)
        {
            timeWaitedUnstickWall += Time.deltaTime;
            if (timeWaitedUnstickWall >= timeUnstickWall)
            {
                StopSlide();
            }
        }
        else
        {
            timeWaitedUnstickWall = 0;
        }
    }
    private void StopSlide() 
    {
        isWallSliding = false;
        _playerController.playerState = PlayerController.PlayerStates.AIR;

        if (loopAS)
            AudioManager._instance.StopLoopSound(loopAS);
        loopAS = null;
    }
    public void CheckWallJumpInAir()
    {
        if (Physics2D.Raycast(transform.position + new Vector3(coll.size.x / 2, 0), Vector2.right, airWallJumpRange,  wallLayer) &&
            Physics2D.Raycast(transform.position + new Vector3(coll.size.x / 2, -coll.size.y/2), Vector2.right, airWallJumpRange, wallLayer))
        {
            walledDir = 1;
            sprt.flipX = true;
            WallJump();
        }
        else if(Physics2D.Raycast(transform.position + new Vector3(-coll.size.x / 2, 0), Vector2.left, airWallJumpRange, wallLayer) &&
            Physics2D.Raycast(transform.position + new Vector3(-coll.size.x / 2, -coll.size.y / 2), Vector2.left, airWallJumpRange, wallLayer))
        {
            walledDir = -1;
            sprt.flipX = false;
            WallJump();
        }
    }
    public void WallJump()
    {

        _playerController.playerState = PlayerController.PlayerStates.AIR;
        isWallSliding = false;
        Vector2 jumpDir = new Vector2(-walledDir * wallJumpingPower.x, wallJumpingPower.y);
        rb2d.velocity = jumpDir;
        _playerController._movementController.externalForces = jumpDir;
        _playerController._movementController.SetAirAcceleration(-walledDir);
        walledDir = 0;
        if (loopAS)
            AudioManager._instance.StopLoopSound(loopAS);
        loopAS = null;
        AudioManager._instance.Play2dOneShotSound(walljumpSound, 0.65f, 1.35f, 1.2f);
    }


    private void OnDrawGizmos()
    {
        if(_playerController != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + new Vector3(coll.size.x / 2, 0) * _playerController._playerInput._playerMovement, wallCheckRange);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + new Vector3(coll.size.x / 2, 0) * walledDir, (transform.position + new Vector3(coll.size.x / 2, 0) * walledDir) + Vector3.right * walledDir * airWallJumpRange);
        }
    }
}
