using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePlatformLogic : MonoBehaviour
{
    private bool startEngine = false;

    [SerializeField]
    private float engineTimer;
    private float engineTimePassed;
    
    public Transform[] platforms;

    // Update is called once per frame
    void Update()
    {
        if (startEngine)
        {
            engineTimePassed += Time.deltaTime;
            if (engineTimer <= engineTimePassed)
            {
                for (int i = 0; i < platforms.Length; i++)
                {
                    platforms[i].gameObject.SetActive(false);
                }
                startEngine = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startEngine = true;
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].gameObject.SetActive(true);
            }

            engineTimePassed = 0;
        }
    }


}
