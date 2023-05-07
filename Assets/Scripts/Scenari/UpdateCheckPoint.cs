using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _respawnPosition;
    
    [SerializeField] private Animator _animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _animator.SetBool("isActivated", true);
            collision.GetComponent<PlayerRespawn>().SetRespawnPos(transform); 
        }
    }
} 
