using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public enum ThrowHookType { INSTANT, BUTTON};
    public ThrowHookType ThrowHook;
    private float playerMovement; //La variable donde guardamos el input de movimiento que recibamos 
    public float _playerMovement => playerMovement; //Variable que recibira el valor de player Movement y la haremos publica para que asi no pueda editarse desde fuera

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

    }

    private void Start()
    {
        InputManager._instance.ingameJumpAction.action.started += JumpAction; //Asignamos la funcion que se llamara pulsar el boton de salto
        InputManager._instance.ingameJumpAction.action.canceled += StopJump; //Al dejar de apretar el boton de saltar dejara de saltar
        InputManager._instance.ingameMovementAction.action.performed += MoveAction; //Mientras el input de ataque este activo se llamara a la funcion para guardarse el valor
        InputManager._instance.ingameMovementAction.action.canceled += MoveAction;


        switch (ThrowHook)
        {
            case ThrowHookType.INSTANT:
                InputManager._instance.ingameAimAction.action.started += InstantHookAction; ;
                break;
            case ThrowHookType.BUTTON:
                InputManager._instance.ingameAimAction.action.started += AimAction;
                InputManager._instance.ingameAimAction.action.performed += AimAction;
                InputManager._instance.ingameAimAction.action.canceled += AimAction;
                InputManager._instance.ingameHookAction.action.started += HookAction;

                break;
            default:
                break;
        }


    }

  

    private void MoveAction(InputAction.CallbackContext obj)
    {
        playerMovement = InputManager._instance.ingameMovementAction.action.ReadValue<float>(); //Le damos a playerMovement
    }



    private void JumpAction(InputAction.CallbackContext obj)
    {
        playerController._movementController.JumpInputPressed();
    }

    private void StopJump(InputAction.CallbackContext obj)
    {
        playerController._movementController.JumpInputUnPressed();
    }


    private void InstantHookAction(InputAction.CallbackContext obj)
    {
        PlayerAimController._instance.gamepadDir = InputManager._instance.ingameAimAction.action.ReadValue<Vector2>();
        playerController._hookController.HookInputPressed();
    }

    private void AimAction(InputAction.CallbackContext obj)
    {
       PlayerAimController._instance.gamepadDir = InputManager._instance.ingameAimAction.action.ReadValue<Vector2>();
    }

    private void HookAction(InputAction.CallbackContext obj)
    {
        playerController._hookController.HookInputPressed();
    }

}
