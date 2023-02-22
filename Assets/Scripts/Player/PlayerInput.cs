using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private float playerMovement;
    public float _playerMovement => playerMovement;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

    }

    private void Start()
    {
        InputManager._instance.ingameJumpAction.action.started += JumpAction;
        InputManager._instance.ingameMovementAction.action.performed += MoveAction;
        InputManager._instance.ingameMovementAction.action.canceled += MoveAction;

    }

    private void MoveAction(InputAction.CallbackContext obj)
    {
        playerMovement = InputManager._instance.ingameMovementAction.action.ReadValue<float>();
    }



    private void JumpAction(InputAction.CallbackContext obj)
    {
        
    }
}
