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
