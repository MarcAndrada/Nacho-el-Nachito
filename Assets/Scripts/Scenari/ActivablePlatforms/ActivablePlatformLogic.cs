using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivablePlatformLogic : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool startEngine = false;

    [SerializeField]
    private float engineTimer;
    private float engineTimePassed;
    
    public Transform[] platforms;

    [Header("Sound"), SerializeField]
    private AudioClip buttonPressed;
    [SerializeField]
    private AudioClip buttonUnpressed;
    [SerializeField]
    private AudioClip buttonTimer;
    private AudioSource timerAS;
    
    [Header("Sprites"), SerializeField]
    private Sprite spritePressed;
    [SerializeField] 
    private Sprite spriteUnpressed;
    [SerializeField]
    private Sprite spriteHold;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        DisablePlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        WaitToDiasble();
    }

    private void ActivatePlatforms() 
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].gameObject.SetActive(true);
        }
        _spriteRenderer.sprite = spriteHold;
        startEngine = false;
        engineTimePassed = 0;
        AudioManager._instance.Play2dOneShotSound(buttonPressed);
    }
    private void StartCountDown()
    {
        _spriteRenderer.sprite = spritePressed;
        startEngine = true;
        AudioManager._instance.Play2dOneShotSound(buttonUnpressed);
        if (!timerAS)
            timerAS = AudioManager._instance.Play2dLoop(buttonTimer);
    }

    private void WaitToDiasble()
    {
        if (startEngine)
        {
            engineTimePassed += Time.deltaTime;
            if (engineTimer <= engineTimePassed)
            {
               DisablePlatforms();
            }
        }
    }
    private void DisablePlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].gameObject.SetActive(false);

        }
        startEngine = false;
        _spriteRenderer.sprite = spriteUnpressed;
        if (timerAS)
        {
            AudioManager._instance.StopLoopSound(timerAS);
            AudioManager._instance.Play2dOneShotSound(buttonUnpressed);
        }

        timerAS = null;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivatePlatforms();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCountDown();
        }
    }


}
