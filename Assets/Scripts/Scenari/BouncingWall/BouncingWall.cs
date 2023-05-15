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

    [Header("Sound"), SerializeField]
    private AudioClip bounceSound;

    private void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            Vector2 direction = (Vector2)transform.right * bounceForce + Vector2.up * bounceForce / 2;
            _playerController.ChangeState(PlayerController.PlayerStates.AIR);
            _playerController.GetComponent<Rigidbody2D>().velocity = direction;
            _playerController._movementController.externalForces = direction;
            AudioManager._instance.Play2dOneShotSound(bounceSound);
        }   
    }
}
