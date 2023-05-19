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
    }

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
                _spriteRenderer.sprite = spriteUnpressed;
                AudioManager._instance.StopLoopSound(timerAS);
                timerAS = null;
                AudioManager._instance.Play2dOneShotSound(buttonUnpressed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _spriteRenderer.sprite = spritePressed;
            startEngine = true;
            AudioManager._instance.Play2dOneShotSound(buttonUnpressed);
            if(!timerAS)
                timerAS = AudioManager._instance.Play2dLoop(buttonTimer);
        }
    }


}
