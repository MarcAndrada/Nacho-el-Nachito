using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerAimController : MonoBehaviour
{
    public enum ControllerType { MOUSE, GAMEPAD };
    public ControllerType controllerType;

    public static PlayerAimController _instance;
    [HideInInspector]
    public Vector2 gamepadDir;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance.gameObject);
                _instance = this;
            }
        }
        else
        {
            _instance = this;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        spriteRenderer = GetComponent<SpriteRenderer>();
        InputSystem.onDeviceChange += onDeviceChanged;
    }

    private void onDeviceChanged(InputDevice arg1, InputDeviceChange arg2)
    {
        CheckSpriteRendEnabled();
    }

    public void UpdateAimMethod() 
    {
        //Comprobamos con cual input estamos controlando ahora el juego
        if (Gamepad.current == null)
        {
            UseMouseInput();
        }
        else
        {
            UseControllerInput();
        }
    }


    private void UseMouseInput()
    {
        Vector2 v3 = Input.mousePosition;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        transform.position = v3;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("ESMEGMA");

    }


    private void UseControllerInput() 
    {
        Debug.Log("ESMEGMA MUY DURO");

        Cursor.visible = false;
    }

    private void CheckSpriteRendEnabled()
    {
        if (Gamepad.current == null)
        {
            spriteRenderer.enabled = true;
            controllerType = ControllerType.MOUSE;
        }
        else
        {
            spriteRenderer.enabled = false;

        }
    }
    
}
