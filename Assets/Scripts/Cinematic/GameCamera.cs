using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform gameManager;
    private float speed = 0f;

    Camera cameraC;

    bool cinematicMode;

    Vector3 savedGameplayPosition;
    float savedGameplaySize;

    // Start is called before the first frame update
    void Start()
    {
        cameraC = GetComponent<Camera>();

        cinematicMode = false;
    }

    public void EnterCinematicMode()
    {
        if (!cinematicMode)
        {
            //savedGameplayPosition = transform.position;
            //savedGameplaySize = cameraC.orthographicSize;
            cinematicMode = true;
        }
    }

    public void ExitCinematicMode()
    {
        if (cinematicMode)
        {
            transform.position = savedGameplayPosition;
            cameraC.orthographicSize = savedGameplaySize;

            cinematicMode = false;
        }
    }

    public void SetSize(float size)
    {
        cameraC.orthographicSize = size;
    }

    public void setObjectActive(int objectIndex, bool active)
    {
        gameObject.SetActive(active);
    }
}
