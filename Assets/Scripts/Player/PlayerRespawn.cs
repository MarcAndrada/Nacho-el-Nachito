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

    [Header("Particles"), SerializeField]
    private GameObject deathParticles;
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
            timeDead = startValue;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            pc.ChangeState(PlayerController.PlayerStates.NONE);
        }
    }

    public void Die() 
    {
        // Nos aseguramos que no se repita la animación de muerte al colisionar con un obstaculo mientras está muerto
        if (pc.playerState == PlayerController.PlayerStates.DEAD) return;
        
        switch (pc.playerState)
        {
            case PlayerController.PlayerStates.MOVING:
            case PlayerController.PlayerStates.AIR:
                rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.HOOK:
                pc._hookController.StopHook();
                pc._hookController._hookController.ResetHookPos();
                pc._hookController._hookController.DisableHook();
                rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.WALL_SLIDE:
                pc._wallJumpController.StopSlide();
                break;
        }

        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        pc.DeadAnimation();
        deathParticles.SetActive(true);
    }

    public void SetRespawnPos(Transform _newPos)
    {
        posit = _newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstaculo"))
        {
            pc.ChangeState(PlayerController.PlayerStates.DEAD);
        }
        else if(collision.CompareTag("OVNI"))
        {
            pc.ChangeState(PlayerController.PlayerStates.DEAD);
        }
    }
}
