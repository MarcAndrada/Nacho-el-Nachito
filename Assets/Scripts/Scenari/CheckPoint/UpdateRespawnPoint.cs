using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRespawnPoint : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform respawnPosition; 

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            respawnPosition.position = transform.position; 
        }
    }
}
