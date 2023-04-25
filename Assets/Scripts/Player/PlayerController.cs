using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {NONE, MOVING, AIR, HOOK,  WALL_SLIDE, CINEMATIC, INTERACTING, DASH, DEAD};
    public PlayerStates playerState;
    


    //Aqui crearemos las variables de todos los scritps del player
    private PlayerInput playerInput;
    private PlayerMovementController movementController;
    private PlayerHookController hookController;
    private PlayerWallJumpController wallJumpController;
    private PlayerRespawn playerRespawn;
    private PlayerDashController dashController;

    //Variable para acceder a los demas scripts
    public PlayerInput _playerInput => playerInput;
    public PlayerMovementController _movementController => movementController;
    public PlayerHookController _hookController => hookController;

    public PlayerWallJumpController _wallJumpController => wallJumpController;

    public PlayerRespawn _playerRespawn => playerRespawn;

    private Animator anim;

    public PlayerDashController _playerDashController => dashController;

    // Start is called before the first frame update
    void Awake()
    {
        AllGetComponents();

    }

    private void AllGetComponents() 
    {
        playerInput = GetComponent<PlayerInput>();
        movementController = GetComponent<PlayerMovementController>();
        hookController = GetComponent<PlayerHookController>();
        wallJumpController = GetComponent<PlayerWallJumpController>();
        playerRespawn = GetComponent<PlayerRespawn>();
        anim = GetComponent<Animator>();
        dashController = GetComponent<PlayerDashController>();
    }

    // Update is called once per frame
    void Update()
    {
        StatesFunctions();
        AnimateCharacter();
        PlayerAimController._instance.UpdateAimMethod();

    }

    private void StatesFunctions() 
    {
        //Aqui segun el estado que este el player haremos una cosa u otra
        switch (playerState)
        {
            case PlayerStates.NONE:
            case PlayerStates.MOVING:
                movementController.CheckGrounded();
                CheckMovementStates();
                movementController.FloorMovement();
                movementController.CheckJumping();
                movementController.CheckSlope();
                movementController.ApplyForces();
                hookController.CheckHookPointNearToCursor();
                break;
            case PlayerStates.AIR:
                movementController.CheckGrounded();
                CheckMovementStates();
                movementController.AirMovement();
                movementController.CheckJumping();
                movementController.CheckSlope();
                movementController.ApplyForces();
                hookController.CheckHookPointNearToCursor();
                wallJumpController.CheckIfWallSliding();
                break;
            case PlayerStates.HOOK:
                hookController.MoveHookedPlayer();
                hookController.CheckPlayerNotStucked();
                hookController.CheckHookPointNearToCursor();

                break;
            case PlayerStates.WALL_SLIDE:
                //Bajar la Y
                //Comporbar el salto
                wallJumpController.WallSlide();
                wallJumpController.CheckIfStopSliding();
                hookController.CheckHookPointNearToCursor();
                break;
            case PlayerStates.DASH:
                dashController.Dash();
                dashController.DashTimer();
                dashController.DashCheckWall();
                break;
            case PlayerStates.CINEMATIC:
                // NADA
                break;
            case PlayerStates.INTERACTING:
                //playerState = PlayerStates.NONE;
                break;
            case PlayerStates.DEAD:
                playerRespawn.Respawn();
                break;
            default:
                break;
        }
    }


    private void CheckMovementStates()
    {
        if (movementController._isGrounded)
        {
            //Si esta en el suelo
            if (playerInput._playerMovement != 0)
            {
                playerState = PlayerStates.MOVING;
            }
            else
            {
                playerState = PlayerStates.NONE;
            }
            
        }
        else
        {
            //Si esta en el aire
            playerState = PlayerStates.AIR;
        }
    }

    private void AnimateCharacter()
    {
        if (playerState == PlayerStates.MOVING)
        {
            anim.SetBool("Moving", true);
            anim.SetBool("OnAir", false);
        }
        if(playerState == PlayerStates.AIR)
        {
            anim.SetBool("OnAir", true);
        }
        if(playerState == PlayerStates.NONE || playerState == PlayerStates.CINEMATIC)
        {
            anim.SetBool("Moving", false);
            anim.SetBool("OnAir", false);
        }
    }
}
