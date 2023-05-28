using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField]
    private Transform door;

    private Rigidbody2D rb2d;

    public bool activated = false;
    private bool moving = false;
    [SerializeField]
    private float openSpeed = 10;
    
    [SerializeField]
    private float maxDistance;

    private Vector2 startPos;

    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Sprite sprite_on;
    [SerializeField] 
    private Sprite sprite_off;

    [Header("Sound"), SerializeField]
    private AudioClip leverSound;

    [Header("Button"), SerializeField]
    private SpriteRenderer controlSR;
    [SerializeField]
    private Sprite keyboardSprite;
    [SerializeField]
    private Sprite controllerSprite;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = door.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPos = door.position;
        controlSR.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {

            if (activated)
            {
                rb2d.velocity = door.up * openSpeed;
                if (Vector2.Distance(startPos, door.position) >= maxDistance)
                {
                    moving = false;
                    door.position = startPos + (Vector2)door.up * maxDistance;
                    rb2d.bodyType = RigidbodyType2D.Static;
                }
            }
            else
            {
                rb2d.velocity = -door.up * openSpeed;
                if (Vector2.Distance(startPos, door.transform.position) <= 0.1f)
                {
                    door.position = startPos;
                    moving = false;
                    rb2d.bodyType = RigidbodyType2D.Static;
                }
            }
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
        moving = true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ShowLeverButton()
    {
        controlSR.gameObject.SetActive(true);
        controlSR.sprite = (PlayerAimController._instance.controllerType == PlayerAimController.ControllerType.GAMEPAD) ? controllerSprite : keyboardSprite;
    }

    public void HideLeverButton() 
    {
        controlSR.gameObject.SetActive(false);
    }
}
