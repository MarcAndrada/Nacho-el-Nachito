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

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        pc = GetComponent<PlayerController>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstaculo"))
        {
            sprite.enabled = false;
            pc.playerState = PlayerController.PlayerStates.DEAD;
        }

        else if(collision.CompareTag("Abduce"))
        { 
            sprite.enabled = false;
            pc.playerState = PlayerController.PlayerStates.DEAD;
        }
    }
}
