using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeField] 
    float timeDelay = 2.0f;
    [SerializeField] 
    float timeDelayRespawn = 5.0f;
    private float timePassed = 0f;
    bool touched = false;
    Collider2D coll;
    SpriteRenderer spriteRenderer;

    [Header("Sound"), SerializeField]
    private AudioClip fallingLoop;
    [SerializeField]
    private AudioClip fallSound;
    private AudioSource fallAS; 


    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            other.GetContacts(contacts);
            if (contacts[0].normal == Vector2.down)
            {
                touched = true;
                if (!fallAS)
                    fallAS = AudioManager._instance.Play2dLoop(fallingLoop);
            }
        }
    }
    private void FixedUpdate()
    {
       if (touched)
        {
            timePassed += Time.fixedDeltaTime;
            if (timePassed >= timeDelay)
            {
                coll.enabled = false;
                spriteRenderer.enabled = false;
                timePassed = 0f;
                touched = false;
                AudioManager._instance.StopLoopSound(fallAS);
                fallAS = null;
                AudioManager._instance.Play2dOneShotSound(fallSound);
            }
        }
       else
        {
            if (!coll.enabled)
            {
                timePassed += Time.fixedDeltaTime;
                if (timePassed >= timeDelayRespawn)
                {
                    coll.enabled = true;
                    spriteRenderer.enabled = true;
                    timePassed = 0f;
                }
            }
        }

    }
}
