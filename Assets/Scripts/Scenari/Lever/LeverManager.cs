using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField]
    private Transform door;

    private Rigidbody2D rb2d;

    public bool activated = false;
    [SerializeField]
    private float openSpeed = 10;
    
    [SerializeField]
    private float maxPosY;

    private float startPosY;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite sprite_on;
    [SerializeField] 
    private Sprite sprite_off;

    [Header("Sound"), SerializeField]
    private AudioClip leverSound;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = door.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPosY = door.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated && door.transform.position.y <= maxPosY + startPosY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, openSpeed);
        }
        else if(!activated && door.transform.position.y > startPosY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -openSpeed);
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }
    
    public void ActivateLever()
    {
        if (!activated)
        {
            spriteRenderer.sprite = sprite_on;
        }
        else
        {
            spriteRenderer.sprite = sprite_off;
        }
        activated = !activated;
        AudioManager._instance.Play2dOneShotSound(leverSound);
    }


}
