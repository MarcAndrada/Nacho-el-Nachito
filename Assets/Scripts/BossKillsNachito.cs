using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillsNachito : MonoBehaviour
{
    private PlayerController pc;
    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("kill");
        if (collision.CompareTag("Player"))
        {
            pc.playerState = PlayerController.PlayerStates.DEAD;
        }
    }
}
