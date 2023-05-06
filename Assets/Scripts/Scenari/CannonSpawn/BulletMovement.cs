using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [HideInInspector]
    public Vector2 dir;    
    private Rigidbody2D rb2d;
    
    private Sound3dController sound3dController;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sound3dController = GetComponent<Sound3dController>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = _cannon.transform.rotation;
        rb2d.velocity =  dir.normalized *_bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisiona con algo");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Water") ||
            collision.gameObject.layer == LayerMask.NameToLayer("HookObstacle"))
        {
            Debug.Log("MUERE 💀");
            sound3dController.PlaySound();
            Destroy(gameObject);
        }
    }

}
