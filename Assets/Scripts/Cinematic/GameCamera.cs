using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform gameManager;
    private float speed = 0f;

    Camera cameraC;

    bool cinematicMode;
    bool shaking;
    float shakeTime = 100;

    Vector3 savedGameplayPosition;
    float savedGameplaySize;

    // Start is called before the first frame update
    void Start()
    {
        cameraC = GetComponent<Camera>();

        cinematicMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinematicMode)
        {
            // Nada
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    public void EnterCinematicMode()
    {
        if (!cinematicMode)
        {
            savedGameplayPosition = transform.position;
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

    public void cameraShake(float duracion, float amplitud)
    {
        if (cinematicMode)
        {
            shaking = true;

            if (shaking)
            {
                shakeTime--;

                if (shakeTime <= 0)
                {
                    shakeTime = 100;

                    while (duracion >= 0)
                    {
                        cameraC.transform.position = new Vector3(UnityEngine.Random.Range(1, amplitud), UnityEngine.Random.Range(1, amplitud), 1);

                        duracion--;
                        Debug.Log("Funciona");
                    }
                }
            }
        }
    }

    public void setObjectActive(int objectIndex, bool active)
    {
        gameObject.SetActive(active);
    }
}
