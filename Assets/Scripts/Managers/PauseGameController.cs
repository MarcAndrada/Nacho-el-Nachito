using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public GameObject pauseOptions;

    public void PauseInteraction()
    {
        if (Time.timeScale != 0.0f)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        } 
        else if (Time.timeScale == 0.0f && pauseOptions.activeSelf)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);

        }
    }
}
