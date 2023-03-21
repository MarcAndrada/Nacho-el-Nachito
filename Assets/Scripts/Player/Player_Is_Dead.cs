using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Is_Dead : MonoBehaviour
{
    [SerializeField] private float timeSprite;

    private bool isDead;
    private float tiempo;
    private SpriteRenderer dibujo;
    private Transform posicion; 

    // Start is called before the first frame update
    void Start()
    {
        dibujo = GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tiempo = timeSprite;

        if (collision.CompareTag("Obstaculo"))
        {
            dibujo.enabled = false;
            while (timeSprite > 0)
            {
                timeSprite -= timeSprite - 1f * Time.deltaTime;
            }
        }

        if(timeSprite == 0)
        {
            dibujo.enabled = true;
            timeSprite = tiempo; 
        }
    }
}
