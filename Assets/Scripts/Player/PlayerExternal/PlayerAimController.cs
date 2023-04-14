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
    [SerializeField]
    private float crosshairSpeed;

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

        spriteRenderer = GetComponent<SpriteRenderer>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        CheckSpriteRendEnabled();
        InputSystem.onDeviceChange += onDeviceChanged;
    }

    private void onDeviceChanged(InputDevice arg1, InputDeviceChange arg2)
    {
        CheckSpriteRendEnabled();
    }

    public void UpdateAimMethod() 
    {
        //Comprobamos con cual input estamos controlando ahora el juego
        if (Gamepad.current != null)
        {
            UseControllerInput();
        }
        else
        {
            MoveCrosshair();
        }
    }


    public void MoveCrosshair()
    {
        Vector2 v3 = Input.mousePosition;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        //transform.position = Vector3.MoveTowards(transform.position, v3, Time.deltaTime * crosshairSpeed);
        transform.position = v3;
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }       

    }


    private void UseControllerInput() 
    {
        Cursor.visible = false;
    }

    private void CheckSpriteRendEnabled()
    {

        if (Gamepad.current == null)
        {
            if (spriteRenderer)
            {
                spriteRenderer.enabled = true;
            }
            controllerType = ControllerType.MOUSE;
        }
        else
        {
            if (spriteRenderer)
            {
                spriteRenderer.enabled = false;
            }
            controllerType = ControllerType.GAMEPAD;

        }
    }
    
}
