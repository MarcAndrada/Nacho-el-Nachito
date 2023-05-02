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
    private bool abducingPlayer = false;
    private float starterSpeed;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        rb = playerController.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        if (abducingPlayer)
        {
            starterSpeed += Time.deltaTime * speedUp;
            rb.velocity = new Vector2(rb.velocity.x, starterSpeed);
            starterSpeed = Mathf.Clamp(starterSpeed, float.NegativeInfinity, maxSpeed);
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
                    starterSpeed = 0;
                    abducingPlayer = true;
                    break;
                case PlayerController.PlayerStates.AIR:
                case PlayerController.PlayerStates.WALL_SLIDE:
                case PlayerController.PlayerStates.DASH:
                    starterSpeed = rb.velocity.y;
                    abducingPlayer = true;
                    break;

                default:
                    break;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            abducingPlayer = false;
        }
    }
}
