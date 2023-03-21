using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {NONE, MOVING, AIR, HOOK,  WALL_SLIDE, DEAD };
    public PlayerStates playerState;

    


    //Aqui crearemos las variables de todos los scritps del player
    private PlayerInput playerInput;
    private PlayerMovementController movementController;
    private PlayerHookController hookController;
    private PlayerWallJumpController wallJumpController;
    private PlayerDashController dashController;

    //Variable para acceder a los demas scripts
    public PlayerInput _playerInput => playerInput;
    public PlayerMovementController _movementController => movementController;
    public PlayerHookController _hookController => hookController;

    public PlayerWallJumpController _wallJumpController => wallJumpController;

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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAimController._instance.UpdateAimMethod();
        StatesFunctions();
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
                dashController.Dash();
                break;
            case PlayerStates.AIR:
                movementController.CheckGrounded();
                wallJumpController.WallSlide();
                CheckMovementStates();
                movementController.AirMovement();
                movementController.CheckJumping();
                movementController.CheckSlope();
                movementController.ApplyForces();
                hookController.CheckHookPointNearToCursor();
                dashController.Dash();
                break;
            case PlayerStates.HOOK:
                hookController.MoveHookedPlayer();
                hookController.CheckHookPointNearToCursor();
                break;
            case PlayerStates.WALL_SLIDE:
                //Bajar la Y
                //Comporbar el salto
                wallJumpController.WallSlide();
                movementController.CheckGrounded();
                CheckMovementStates();

                hookController.CheckHookPointNearToCursor();
                dashController.Dash();
                break;
            case PlayerStates.DEAD:
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
            if(wallJumpController.isWallSliding)
            {
                playerState = PlayerStates.WALL_SLIDE;
            }
            else
            {
                //Si esta en el aire
                playerState = PlayerStates.AIR;
            }
        }
    }
    
}
