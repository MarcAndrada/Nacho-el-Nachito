using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbducePlayerUp : MonoBehaviour
{

    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform bossPos;

    [SerializeField] private float speedUp;
    [SerializeField] private float maxSpeed;
    private Rigidbody2D rb;
    private PlayerController playerController;
    public bool abducingPlayer {get; private set;}
    private float starterSpeed;

    private Rigidbody2D generatorRb;

    private bool abducingGenerator;
    private float generatorUpSpeed;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        rb = playerController.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AbducePlayer();
        AbduceGenerator();
    }

    private void AbducePlayer() 
    {
        if (abducingPlayer)
        {
            starterSpeed += Time.deltaTime * speedUp;
            rb.velocity = new Vector2(rb.velocity.x, starterSpeed);
            starterSpeed = Mathf.Clamp(starterSpeed, float.NegativeInfinity, maxSpeed);
        }
    }

    private void AbduceGenerator()
    {
        if (abducingGenerator)
        {
            generatorUpSpeed += Time.deltaTime * speedUp * 2.5f;
            generatorRb.velocity = new Vector2((transform.position.x - generatorRb.transform.position.x) * 5, generatorUpSpeed);
            generatorUpSpeed = Mathf.Clamp(generatorUpSpeed, float.NegativeInfinity, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) 
        {
            switch (playerController.playerState)
            {
                case PlayerController.PlayerStates.NONE:
                case PlayerController.PlayerStates.MOVING:
                    starterSpeed = 1.5f;
                    abducingPlayer = true;
                    break;
                case PlayerController.PlayerStates.AIR:
                case PlayerController.PlayerStates.WALL_SLIDE:
                case PlayerController.PlayerStates.DASH:
                    starterSpeed = rb.velocity.y;
                    starterSpeed = Mathf.Clamp(starterSpeed, -1, maxSpeed);
                    abducingPlayer = true;
                    break;
                default:
                    starterSpeed = 1.5f;
                    break;
            }
            
        }

        if (collision.CompareTag("ElectricGenerator"))
        {
            GeneratorController genCont = collision.GetComponent<GeneratorController>();
            if (genCont.broke)
            {
                generatorRb = collision.GetComponent<Rigidbody2D>();
                abducingGenerator = true;
                generatorUpSpeed = 1.5f;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) 
        {
            switch (playerController.playerState)
            {
                case PlayerController.PlayerStates.NONE:
                case PlayerController.PlayerStates.MOVING:
                case PlayerController.PlayerStates.AIR:
                case PlayerController.PlayerStates.WALL_SLIDE:
                case PlayerController.PlayerStates.DASH:

                    abducingPlayer = true;
                    break;
                default:
                    abducingPlayer = false;
                    break;
            }
            
        }
        if (collision.CompareTag("ElectricGenerator"))
        {
            GeneratorController genCont = collision.GetComponent<GeneratorController>();
            if (genCont.broke)
            {
                abducingGenerator = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            abducingPlayer = false;
        }

        if (collision.CompareTag("ElectricGenerator"))
        {
            abducingGenerator = false;
        }
    }
}
