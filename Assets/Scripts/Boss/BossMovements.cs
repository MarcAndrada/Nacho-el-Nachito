using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovements : MonoBehaviour
{
    [Header("Movement"), SerializeField]
    private Transform playerPos;
    [SerializeField] private float speed;

    Rigidbody2D rb;

    [Header("Position"), SerializeField]
    private float bossPosY;

    [SerializeField]
    private GameObject CinematicManager;

    private CinematicManager _cm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 target = new Vector2(playerPos.position.x, bossPosY);
        
        _cm = CinematicManager.GetComponent<CinematicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_cm.isCinematicMode)
        {
            Vector2 target = new Vector2(playerPos.position.x, bossPosY);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);

            rb.position = newPos;
        }  
    }
}
