using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    newCameraController newCamController;
    private void Awake()
    {
        newCamController = FindObjectOfType<newCameraController>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CameraChanger"))
        {
            CameraValueChanger values = collision.GetComponent<CameraValueChanger>();
            newCamController.ChangeCameraValues(values.camPos, values.camSize);
        }
    }
}
