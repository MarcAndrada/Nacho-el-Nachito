using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


    public void OnAimUsed(Vector2 _inputValue) 
    {
        //Debug.Log("Input -> " + _inputValue + " Magnitud-> " + _inputValue.magnitude);

        Vector2 newPos = transform.position;
        newPos += _inputValue;
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(newPos);
        
        if (screenPos.y > Screen.height )
        { 
            screenPos.y = Screen.height;
        }
        else if (screenPos.y < 0)
        {
            screenPos.y = 0;
        }

        if (screenPos.x > Screen.width)
        {
            screenPos.x = Screen.width;
        }
        else if (screenPos.x < 0)
        {
            screenPos.x = 0;
        }

        newPos = Camera.main.ScreenToWorldPoint(screenPos);

        transform.position = Vector2.MoveTowards(transform.position, newPos, cursorSpeed);
    }
}
