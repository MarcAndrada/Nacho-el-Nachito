using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformColision : MonoBehaviour
{
    [SerializeField] private float timeNoColision;

    private float startValue;

    private bool platformDes;

    private BoxCollider2D boxcollider;
    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        startValue = timeNoColision;
    }
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // CAMBIAR POR INPUTSYSTEM
        {
            boxcollider.enabled = false;
            platformDes = true;
        }

        if(platformDes)
        {
            timeNoColision = timeNoColision - 1f * Time.deltaTime;
        }

        if(timeNoColision <= 0 )
        {
            platformDes = false;
            boxcollider.enabled = true;
            timeNoColision = startValue;
        }
    }
   
}
