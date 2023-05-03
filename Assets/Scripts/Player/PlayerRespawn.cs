using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeDead;
    private float startValue;
    
    private SpriteRenderer sprite;

    [SerializeField]
    private Transform posit;

    private PlayerController pc;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        pc = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        startValue = timeDead;
    }

    public void Respawn()
    {
        timeDead -= Time.deltaTime;

        if (timeDead <= 0)
        {
            transform.position = posit.position;
            sprite.enabled = true;
            timeDead = startValue;
            pc.playerState = PlayerController.PlayerStates.NONE;
        }
    }

    public void Die() 
    {
        switch (pc.playerState)
        {
            case PlayerController.PlayerStates.MOVING:
            case PlayerController.PlayerStates.AIR:
                rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.HOOK:
                pc._hookController.StopHook();
                rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.WALL_SLIDE:
                pc._wallJumpController.StopSlide();
                break;
            case PlayerController.PlayerStates.CINEMATIC:
                break;
            case PlayerController.PlayerStates.DASH:
                break;
            default:
                break;
        }

        sprite.enabled = false;
        pc.playerState = PlayerController.PlayerStates.DEAD;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstaculo"))
        {
            Die();
        }
    }
}
