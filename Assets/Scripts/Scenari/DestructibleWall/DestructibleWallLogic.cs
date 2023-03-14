using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWallLogic : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player") && _playerController.playerState == PlayerController.PlayerStates.HOOK)
        {
            Destroy(gameObject);
        }
    }
}
