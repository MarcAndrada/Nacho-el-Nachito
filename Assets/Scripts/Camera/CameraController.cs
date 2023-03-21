using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float verticalOffset;
    [SerializeField]
    private float horizontalOffset;
    [SerializeField]
    private Vector2 leftDownMinPos;
    [SerializeField]
    private Vector2 rightUpMaxPos;


    private Vector2 newCamPos;
    private PlayerController playerController;

    private Camera myCamera;

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        newCamPos = transform.position;
    }


    private void LateUpdate()
    {

        DefineCameraPos();

        LockCameraInBorders();
    }

    private void DefineCameraPos()
    {
        Vector2 centerPos = playerController.transform.position;
        Vector2 lookAtPos = SetLookAtValue();        
        Vector2 rightUpBorder = myCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
        Vector2 leftDownBorder = myCamera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
        newCamPos = transform.position;

        

        if (lookAtPos.x > rightUpBorder.x - horizontalOffset)
        {
            if (true)
            {

            }
        }
        else if (lookAtPos.x < leftDownBorder.x - horizontalOffset)
        {

        }
        else
        {

        }

        if (lookAtPos.y > rightUpBorder.y - verticalOffset)
        {

        }
        else if (lookAtPos.y < leftDownBorder.y - verticalOffset)
        {

        }
        else
        {

        }






    }

    private Vector2 SetLookAtValue()
    {
        Vector2 lookAtValue;
        if (PlayerAimController._instance.controllerType == PlayerAimController.ControllerType.MOUSE)
        {
            lookAtValue = PlayerAimController._instance.transform.position;
        }
        else
        {
            lookAtValue = PlayerAimController._instance.gamepadDir * 100;
        }
        return lookAtValue;
    }

    //private void CalculateCamPos(Vector2 _camPos, )
    //{

    //}


    private void LockCameraInBorders()
    {
        if (newCamPos.y > rightUpMaxPos.y)
        {
            transform.position = new Vector3(transform.position.x, rightUpMaxPos.y, transform.position.z);
        }

        if (transform.position.x > rightUpMaxPos.x)
        {
            transform.position = new Vector3(rightUpMaxPos.x, transform.position.y, transform.position.z);
        }


        if (transform.position.y < leftDownMinPos.y)
        {
            transform.position = new Vector3(transform.position.x, leftDownMinPos.y, transform.position.z);
        }

        if (transform.position.x < leftDownMinPos.x)
        {
            transform.position = new Vector3(leftDownMinPos.x, transform.position.y, transform.position.z);
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (myCamera)
        {
            Gizmos.DrawWireSphere(myCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 20)), 0.5f);
            Gizmos.DrawLine(myCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 20)), new Vector3(transform.position.x, transform.position.y, 20));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(myCamera.ScreenToWorldPoint(new Vector3(0, 0, 20)), 0.5f);
            Gizmos.DrawLine(myCamera.ScreenToWorldPoint(new Vector3(0, 0, 20)), new Vector3(transform.position.x, transform.position.y, 20));

        }


    }
}
