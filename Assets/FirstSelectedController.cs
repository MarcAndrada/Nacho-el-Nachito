using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectedController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuFirstSelected;

    [SerializeField]
    private GameObject optionsMenuFirstSelected;

    [SerializeField]
    private GameObject soundMenuFirstSelected;

    [SerializeField]
    private GameObject graphicsMenuFirstSelected;

    [SerializeField]
    private GameObject controlsMenuFirstSelected;

    [SerializeField]
    private GameObject creditsMenuFirstSelected;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void GoToOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    public void GoBackToMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void GoToSoundMenu()
    {
        EventSystem.current.SetSelectedGameObject(soundMenuFirstSelected);
    }

    public void GoToGraphicsMenu()
    {
        EventSystem.current.SetSelectedGameObject(graphicsMenuFirstSelected);
    }

    public void GoToControlsMenu()
    {
        EventSystem.current.SetSelectedGameObject(controlsMenuFirstSelected);
    }

    public void GoBackToOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    public void GoToCreditsMenu()
    {
        EventSystem.current.SetSelectedGameObject(creditsMenuFirstSelected);
    }
}
