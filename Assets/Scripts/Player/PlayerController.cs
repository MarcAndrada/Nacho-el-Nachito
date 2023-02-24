using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {NONE, MOVING, AIR, HOOK, DEAD };
    public PlayerStates playerState;


    //Aqui crearemos las variables de todos los scritps del player
    private PlayerInput playerInput;
    private PlayerMovementController movementController;

    //Variable para acceder a los demas scripts
    public PlayerInput _playerInput => playerInput;
    public PlayerMovementController _movementController => movementController;
    

    // Start is called before the first frame update
    void Awake()
    {
        AllGetComponents();
    }

    private void AllGetComponents() 
    {
        playerInput = GetComponent<PlayerInput>();
        movementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
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
                movementController.MovePlayer();
                break;
            case PlayerStates.AIR:
                movementController.CheckGrounded();
                CheckMovementStates();


                break;
            case PlayerStates.HOOK:
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
            //Si esta en el aire
            playerState = PlayerStates.AIR;
        }
    }

}