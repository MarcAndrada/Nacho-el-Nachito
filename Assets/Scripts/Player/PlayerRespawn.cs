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
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();  
        pc = GetComponent<PlayerController>();
        startValue = timeDead;
    }

    public void Respawn()
    {
        timeDead = timeDead -1f * Time.deltaTime;

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
    }
}
