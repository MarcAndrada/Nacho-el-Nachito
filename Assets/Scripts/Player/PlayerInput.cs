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

        InputManager._instance.ingameAimAction.action.started += GamepadHookAction;
        InputManager._instance.ingameHookAction.action.started += MouseHookAction;
        InputManager._instance.ingameDashAction.action.started += DashAction;

    }

  

    private void MoveAction(InputAction.CallbackContext obj)
    {
        playerMovement = InputManager._instance.ingameMovementAction.action.ReadValue<float>(); //Le damos a playerMovement
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
        switch (playerController.playerState)
        {
            case PlayerController.PlayerStates.WALL_SLIDE:
                // Dash oposite wall
                break;

            case PlayerController.PlayerStates.NONE:
                // Dash front player
                break;
                
            case PlayerController.PlayerStates.MOVING:
                playerController._movementController.JumpInputPressed();
                break;
        }
        Debug.Log("Dash");
    }

}
