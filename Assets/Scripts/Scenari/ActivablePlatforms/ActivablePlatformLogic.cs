using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePlatformLogic : MonoBehaviour
{
    private bool startEngine = false;

    [SerializeField]
    private float engineTimer = 100f;

    private float startValue;

    public Transform[] platforms;

    // Start is called before the first frame update
    void Start()
    {
        startValue = engineTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(startEngine)
        {
            for(int i = 0; i < platforms.Length; i++)
            {
                platforms[i].gameObject.SetActive(true);
            }

            engineTimer--;
        }
        else
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].gameObject.SetActive(false);
            }
        }

        if(engineTimer <= 0)
        {
            startEngine = false;
            engineTimer = startValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        startEngine = true;
    }
}
