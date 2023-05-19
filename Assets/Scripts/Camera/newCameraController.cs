using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class newCameraController : MonoBehaviour
{
    private PixelPerfectCamera pixelCam;

    private bool isMoving;
    [SerializeField]
    private float camSpeed;
    private float moveProcess = 0;

    private Vector3 starterPos;
    private Vector3 targetPos;
    private Vector2 starterSize;
    private Vector2 targetSize;



    private void Awake()
    {
        pixelCam = GetComponent<PixelPerfectCamera>();
        ChangeCameraValues(transform.position, new Vector2(pixelCam.refResolutionX, pixelCam.refResolutionY));
    }

    private void Update()
    {
        if (isMoving)
        {
            moveProcess += Time.deltaTime * camSpeed;
            transform.position = Vector2.Lerp(starterPos, targetPos, moveProcess);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);   
            Vector2 camProcess = Vector2.Lerp(starterSize, targetSize, moveProcess);
            if ((int)camProcess.x % 2 != 0 )
            {
                camProcess.x++;
            }
            if ((int)camProcess.y % 2 != 0 )
            {
                camProcess.y++;
            }
            pixelCam.refResolutionX = (int)camProcess.x;
            pixelCam.refResolutionY = (int)camProcess.y;
            if (moveProcess >= 1)
            {
                isMoving = false;
            }
        }        
    }

    public void ChangeCameraValues(Vector2 _nextPos, Vector2 _nextSize) 
    {
        starterPos = transform.position;
        starterPos.z = -10;
        starterSize.x = pixelCam.refResolutionX;
        starterSize.y = pixelCam.refResolutionY;

        targetPos = _nextPos;
        targetPos.z = -10;
        targetSize = _nextSize;
        moveProcess = 0;
        isMoving = true;
        
    }
}
