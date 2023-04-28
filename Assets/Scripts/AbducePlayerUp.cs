using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbducePlayerUp : MonoBehaviour
{

    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform bossPos;

    [SerializeField] private float speedUp; 

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("ASD");
        if (collision.CompareTag("Player"))
        {
            m_Player.position = new Vector2(m_Player.position.x, m_Player.position.y+speedUp);
            
        }
    }
}
