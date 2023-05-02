using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0) 
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }
}
