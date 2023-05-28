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

    private bool levelFinished = false;
    private float timer = 1.1f;

    private void Start()
    {
        startingTransition.SetActive(true);
        AudioManager._instance.Play2dOneShotSound(Resources.Load("FadeOutTransition") as AudioClip);
    }

    private void Update()
    {
        if(levelFinished)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene(NextLevel);
            }
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
            AudioManager._instance.Play2dOneShotSound(Resources.Load("FadeInTransition") as AudioClip);

        }
    }
}
