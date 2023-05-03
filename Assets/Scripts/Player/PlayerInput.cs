using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private float playerMovement; //La variable donde guardamos el input de movimiento que recibamos 
    public float _playerMovement => playerMovement; //Variable que recibira el valor de player Movement y la haremos publica para que asi no pueda editarse desde fuera

    private PlayerController playerController;

    private CinematicManager cinematicManager;
    
    private PauseGameController pauseGameController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        cinematicManager = FindObjectOfType<CinematicManager>();
        pauseGameController = FindObjectOfType<PauseGameController>();
    }

    /*
     *
     * Aqui vinculamos cada input con la accion que realizara     
     */
    private void Start()
    {
        InputManager._instance.ingameJumpAction.action.started += JumpAction; //Asignamos la funcion que se llamara pulsar el boton de salto
        InputManager._instance.ingameJumpAction.action.canceled += StopJump; //Al dejar de apretar el boton de saltar dejara de saltar
        InputManager._instance.ingameMovementAction.action.performed += MoveAction; //Mientras el input de ataque este activo se llamara a la funcion para guardarse el valor
        InputManager._instance.ingameMovementAction.action.canceled += MoveAction;
        InputManager._instance.ingameInteractTextAction.action.started += InteractTextAction;
        InputManager._instance.ingameInteractPauseAction.action.started += InteractPauseAction;
        
        
        InputManager._instance.ingameAimAction.action.started += GamepadHookAction;
        InputManager._instance.ingameHookAction.action.started += MouseHookAction;
        InputManager._instance.ingameDashAction.action.started += DashAction;

        InputManager._instance.ingameInteractAction.action.started += InteractingAction;
        InputManager._instance.ingameGoDownAction.action.started += GoDownAction;
        InputManager._instance.ingameGoDownAction.action.canceled += GoDownAction;
    }

    /*
     * ---------------------------IMPORTANTE!!!!------------------------------------- 
     * Al cambiar de escena, borrar el objeto... 
     * Tendremos que desvincular las funciones de los inputs para que no nos pete el codigo
     *
     */
    private void OnDestroy()
    {
        InputManager._instance.ingameJumpAction.action.started -= JumpAction;
        InputManager._instance.ingameJumpAction.action.canceled -= StopJump;
        InputManager._instance.ingameMovementAction.action.performed -= MoveAction;
        InputManager._instance.ingameMovementAction.action.canceled -= MoveAction;

        InputManager._instance.ingameAimAction.action.started -= GamepadHookAction;
        InputManager._instance.ingameHookAction.action.started -= MouseHookAction;
        InputManager._instance.ingameDashAction.action.started -= DashAction;

        InputManager._instance.ingameInteractAction.action.started -= InteractingAction;
        InputManager._instance.ingameGoDownAction.action.started -= GoDownAction;
        InputManager._instance.ingameGoDownAction.action.canceled -= GoDownAction;

    }



    #region Actions
    private void MoveAction(InputAction.CallbackContext obj)
    {
        playerMovement = InputManager._instance.ingameMovementAction.action.ReadValue<float>(); //Le damos a playerMovement
    }

    private void JumpAction(InputAction.CallbackContext obj)
    {
        switch (playerController.playerState)
        {
            case PlayerController.PlayerStates.WALL_SLIDE:
                playerController._wallJumpController.WallJump();
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
                    playerController._playerDashController.StartDash(InputManager._instance.ingameAirDirectionAction.action.ReadValue<Vector2>());
                    break;
            }
        }
    }

    private void InteractingAction(InputAction.CallbackContext obj)
    {
        playerController._interactionController.Interact();
    }

    private void GoDownAction(InputAction.CallbackContext obj)
    {
        if (InputManager._instance.ingameGoDownAction.action.ReadValue<float>() == 1)
        {
            playerController._playerDownController.goingDown = true;
        }
        else
        {
            playerController._playerDownController.goingDown = false;
        }
    }

    private void InteractTextAction(InputAction.CallbackContext obj)
    {
        cinematicManager.InteractText();
    }

    private void InteractPauseAction(InputAction.CallbackContext obj)
    {
        pauseGameController.PauseInteraction();
    }
    
    #endregion
}
