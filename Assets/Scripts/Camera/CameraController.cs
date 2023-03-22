using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    [Header("Player Borders"), SerializeField]
    private float playerVerticalOffset;
    [SerializeField]
    private float playerHorizontalOffset;

    [Header("Mouse Borders"), SerializeField]
    private float verticalOffset;
    [SerializeField]
    private float horizontalOffset;

    [Header("Map Borders"), SerializeField]
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
        PlayerAimController._instance.MoveCrosshair();
        CameraBehaviour();
        PlayerAimController._instance.MoveCrosshair();

    }

    private void CameraBehaviour()
    {
        Vector2 playerPos = playerController.transform.position;
        Vector2 lookAtPos = SetLookAtValue();
        Vector2 rightUpBorder = myCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
        Vector2 leftDownBorder = myCamera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
        //Primero comprobamos si el raton se sale por algun lado
        if (CheckIfLookAtPosIsAtBorderOffset(lookAtPos, leftDownBorder, rightUpBorder))
        {
            //Si el raton se sale moveremos la newCameraPos a la posicion hacia la que este el raton
            Vector2 dir = lookAtPos - (Vector2)transform.position;
            ApplyInputDir(dir.normalized, cameraSpeed);

            //Comprobamos si esta muy lejos del player
            //Y en caso de que la nueva posicion haya llegado al maximo de distancia permitido se bloqueara el movimiento
            SetMaxDistanceToCameraFormPlayer(playerPos, leftDownBorder, rightUpBorder);
        }
        else
        {
            //Si no esta tocando los bordes con el raton comprobar que el player este dentro de la pantalla
            SetMaxDistanceToCameraFormPlayer(playerPos, leftDownBorder, rightUpBorder);
            //Si no no hara nada
        }

        //Si choca con los bordes del mapa se bloqueara el movimiento hasta ese punto
        LockCameraInBorders();


        transform.position = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);


    }

    private Vector2 SetLookAtValue()
    {
        Vector2 lookAtValue;

        //Comprobamos el tipo de controlador
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
    #region Follow Input Functions
    private bool CheckIfLookAtPosIsAtBorderOffset(Vector2 _lookAtPos, Vector2 _leftDownBorder, Vector2 _rightUpBorder)
    {
        if (_lookAtPos.x > _rightUpBorder.x - horizontalOffset || _lookAtPos.y > _rightUpBorder.y - verticalOffset ||
            _lookAtPos.x < _leftDownBorder.x + horizontalOffset || _lookAtPos.y < _leftDownBorder.y + verticalOffset)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    private void ApplyInputDir(Vector2 _dir, float _speed)
    {
        float minDirValue = 0.3f;
        if (_dir.x < minDirValue && _dir.x > -minDirValue)
            _dir.x = 0;

        if (_dir.y < minDirValue && _dir.y > -minDirValue)
            _dir.y = 0;


        newCamPos += _dir * _speed * Time.deltaTime;
    }

    private void SetMaxDistanceToCameraFormPlayer(Vector2 _playerPos, Vector2 _leftDownBorder, Vector2 _rightUpBorder)
    {
        //En caso de que la nueva posicion haya llegado al maximo de distancia permitido se bloqueara el movimiento
        Vector2 camWordlPosSize = _rightUpBorder - _leftDownBorder;
        Vector2 updatedRightUpBorder = new Vector2(newCamPos.x + camWordlPosSize.x / 2, newCamPos.y + camWordlPosSize.y / 2);
        Vector2 updatedLeftDownBorder = new Vector2(newCamPos.x - camWordlPosSize.x / 2, newCamPos.y - camWordlPosSize.y / 2); ;


        //Comprobamos la X
        if (_playerPos.x > updatedRightUpBorder.x - playerHorizontalOffset)
        {
            newCamPos.x = (_playerPos.x + playerHorizontalOffset) - (camWordlPosSize.x / 2); //PORQUE ESTO NO ESTA BIEN??????
        }
        else if (_playerPos.x < updatedLeftDownBorder.x + playerHorizontalOffset)
        {
            newCamPos.x = (_playerPos.x - playerHorizontalOffset) + (camWordlPosSize.x / 2);
        }

        //Comprobamos la Y
        if (_playerPos.y > updatedRightUpBorder.y - playerVerticalOffset)
        {
            newCamPos.y = (_playerPos.y + playerVerticalOffset) - (camWordlPosSize.y / 2);
        }
        else if (_playerPos.y < updatedLeftDownBorder.y + playerVerticalOffset)
        {
            newCamPos.y = (_playerPos.y - playerVerticalOffset) + (camWordlPosSize.y / 2);
        }

    }
    #endregion


    private void LockCameraInBorders()
    {
        //Comprobamos la X
        if (newCamPos.x > rightUpMaxPos.x)
        {
            newCamPos.x = rightUpMaxPos.x;
        }
        else if (newCamPos.x < leftDownMinPos.x)
        {
            newCamPos.x = leftDownMinPos.x;
        }

        //Comprobamos la Y
        if (newCamPos.y > rightUpMaxPos.y)
        {
            newCamPos.y = rightUpMaxPos.y;
        }
        else if (newCamPos.y < leftDownMinPos.y)
        {
            newCamPos.y = leftDownMinPos.y;
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
