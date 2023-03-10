using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CrosshairController : MonoBehaviour
{

    public static CrosshairController _instance;
    [SerializeField]
    private float cursorSpeed;

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

    }


    public void SMEGMA() 
    {
        if (Gamepad.current == null)
        {
            Vector2 v3 = Input.mousePosition;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            transform.position = v3;
            Debug.Log("ESMEGMA");
        }
        else
        {
            Debug.Log("ESMEGMA MUY DURO");
        }
    }
}
