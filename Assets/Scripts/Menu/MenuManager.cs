using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string GameScene;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(GameScene);
        Time.timeScale = 1.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
