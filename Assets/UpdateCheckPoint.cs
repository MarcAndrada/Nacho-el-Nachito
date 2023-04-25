using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCheckPoint : MonoBehaviour
{
    [SerializeField]
    private Transform RespawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            RespawnPosition.position = transform.position; 
        }
    }
}
