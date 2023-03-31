using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonSpawn : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenBullets = 2f;
    
    [SerializeField] private GameObject _bullet;

    private float _timePassed;
    // Start is called before the first frame update
    void Start()
    {
        _timePassed = _timeBetweenBullets;
    }

    // Update is called once per frame
    void Update()
    {
        _timePassed -= Time.deltaTime;
        if (_timePassed <= 0)
        {
            Instantiate(_bullet, transform.position, transform.rotation).GetComponent<BulletMovement>().dir = transform.right;
            _timePassed = _timeBetweenBullets;
        }
    }
}
