using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ManagerScene : MonoBehaviour
{
    [SerializeField] private GameObject startingTransition;
    [SerializeField] private GameObject endTransition;

    [SerializeField] private string NextLevel;

    private bool startTimer = true;
    private bool levelFinished = false;
    private float timer = 0;

    private void Start()
    {
        startingTransition.SetActive(true);
    }

    private void Update()
    {
        if(startTimer)
        {
            timer--;
        }

        if (timer <= 0 && !levelFinished)
        {
            startTimer = false;
            timer = 650;
        }

        if (timer <= 0 && levelFinished)
        {
            SceneManager.LoadScene(NextLevel);
        }
    }

    private void DisableStartingTransition()
    {
        startingTransition.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endTransition.SetActive(true);
            levelFinished = true;
            startTimer = true;
        }
    }
}
