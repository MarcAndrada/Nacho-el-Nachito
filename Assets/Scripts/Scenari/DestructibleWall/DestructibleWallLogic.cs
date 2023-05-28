using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWallLogic : MonoBehaviour
{
    private PlayerController _playerController;    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerController = collision.GetComponent<PlayerController>();

        if (collision.tag.Equals("Player") && _playerController.playerState == PlayerController.PlayerStates.HOOK)
        {
            Destroy(gameObject);
        }
    }
}
