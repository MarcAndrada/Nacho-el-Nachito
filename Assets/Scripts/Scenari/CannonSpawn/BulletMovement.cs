using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;

    [SerializeField] private GameObject _cannon;
    
    private Rigidbody2D rb2d;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = _cannon.transform.rotation;
        transform.Translate(_cannon.transform.right *_bulletSpeed * Time.deltaTime);
        if (transform.position.x < -14.5)
        {
            Destroy(this.gameObject);
        }
    }
}
