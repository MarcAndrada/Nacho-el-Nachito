using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField]
    private Transform door;

    private Rigidbody2D rb2d;

    private bool collided = false;
    private bool activated = false;
    private float speed = 10;
    
    [SerializeField]
    private float maxPosY;

    private float startPosY;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = door.GetComponent<Rigidbody2D>();
        startPosY = door.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && collided && activated == false)
        {
            activated = true;
        }
        else if(Input.GetKeyDown(KeyCode.E) && collided && activated == true)
        {
            activated = false;
        }

        if(activated && door.transform.position.y <= maxPosY + startPosY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, speed);
        }
        else if(!activated && door.transform.position.y > startPosY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -speed);
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collided = false;
        }
    }
}
