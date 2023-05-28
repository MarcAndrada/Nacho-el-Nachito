using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BouncingWall : MonoBehaviour
{
    [SerializeField]
    private float bounceForce;
    private PlayerController _playerController;
    private Animator animator;

    [Header("Sound"), SerializeField]
    private AudioClip bounceSound;

    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            Vector2 direction = (Vector2)transform.right * bounceForce + Vector2.up * bounceForce / 2;
            _playerController.GetComponent<Rigidbody2D>().velocity = direction;
            _playerController._movementController.externalForces = direction;
            _playerController._playerDashController._canDash = true;
            switch (_playerController.playerState)
            {
                case PlayerController.PlayerStates.NONE:
                case PlayerController.PlayerStates.MOVING:
                case PlayerController.PlayerStates.AIR:
                case PlayerController.PlayerStates.WALL_SLIDE:
                case PlayerController.PlayerStates.DASH:
                    _playerController.ChangeState(PlayerController.PlayerStates.AIR);
                    break;

                default:
                    break;
            }
            AudioManager._instance.Play2dOneShotSound(bounceSound);
            animator.SetTrigger("Bounce");
        }   
    }
}
