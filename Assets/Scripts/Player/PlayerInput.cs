using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private float playerMovement; //La variable donde guardamos el input de movimiento que recibamos 
    public float _playerMovement => playerMovement; //Variable que recibira el valor de player Movement y la haremos publica para que asi no pueda editarse desde fuera

    private PlayerController playerController;

    private PlayerWallJumpController wallJumpController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        wallJumpController = GetComponent<PlayerWallJumpController>();
    }

    private void Start()
    {
        InputManager._instance.ingameJumpAction.action.started += JumpAction; //Asignamos la funcion que se llamara pulsar el boton de salto
        InputManager._instance.ingameJumpAction.action.canceled += StopJump; //Al dejar de apretar el boton de saltar dejara de saltar
        InputManager._instance.ingameMovementAction.action.performed += MoveAction; //Mientras el input de ataque este activo se llamara a la funcion para guardarse el valor
        InputManager._instance.ingameMovementAction.action.canceled += MoveAction;

        InputManager._instance.ingameGoDownAction.action.started += GoDownAction;
        InputManager._instance.ingameAimAction.action.started += GamepadHookAction;
        InputManager._instance.ingameHookAction.action.started += MouseHookAction;
        InputManager._instance.ingameDashAction.action.started += DashAction;

        InputManager._instance.ingameInteractAction.action.started += InteractingAction;
    }

  

    private void MoveAction(InputAction.CallbackContext obj)
    {
        playerMovement = InputManager._instance.ingameMovementAction.action.ReadValue<float>(); //Le damos a playerMovement
    }

    private void GoDownAction(InputAction.CallbackContext obj)
    {
        playerController._playerDownController.Interact();
    }


    private void JumpAction(InputAction.CallbackContext obj)
    {
        switch (playerController.playerState)
        {
            case PlayerController.PlayerStates.WALL_SLIDE:
                wallJumpController.WallJump();
                break;

            case PlayerController.PlayerStates.NONE:
            case PlayerController.PlayerStates.MOVING:
                playerController._movementController.JumpInputPressed();
                break;
            case PlayerController.PlayerStates.AIR:
                playerController._wallJumpController.CheckWallJumpInAir();
                break;
        }
    }

    private void StopJump(InputAction.CallbackContext obj)
    {
        playerController._movementController.JumpInputUnPressed();
    }


    private void GamepadHookAction(InputAction.CallbackContext obj)
    {
        PlayerAimController._instance.gamepadDir = InputManager._instance.ingameAimAction.action.ReadValue<Vector2>();
        playerController._hookController.HookInputPressed();
    }

    private void MouseHookAction(InputAction.CallbackContext obj)
    {
        playerController._hookController.HookInputPressed();
    }

   
    private void DashAction(InputAction.CallbackContext obj)
    {
        if (playerController._playerDashController._canDash)
        {
            switch (playerController.playerState)
            {
                case PlayerController.PlayerStates.NONE:
                case PlayerController.PlayerStates.MOVING:
                case PlayerController.PlayerStates.AIR:
                    playerController._playerDashController._dashDirection = InputManager._instance.ingameAirDirectionAction.action.ReadValue<Vector2>();
                    playerController._wallJumpController.isWallSliding = false;
                    playerController.playerState = PlayerController.PlayerStates.DASH;
                    break;
            }
        }
    }

    private void InteractingAction(InputAction.CallbackContext obj)
    {
        playerController._interactionController.Interact();
    }
}
