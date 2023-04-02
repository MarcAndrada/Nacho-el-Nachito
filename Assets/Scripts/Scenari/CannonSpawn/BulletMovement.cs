using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [HideInInspector]
    public Vector2 dir;    
    private Rigidbody2D rb2d;
    
    private LayerMask collisionMask;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = _cannon.transform.rotation;
        rb2d.velocity =  dir.normalized *_bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Water")) 
        {
            Destroy(gameObject);
        }


    }

}
