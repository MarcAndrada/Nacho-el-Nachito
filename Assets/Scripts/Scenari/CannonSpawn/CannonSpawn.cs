using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonSpawn : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenBullets = 2f;
    
    [SerializeField] private GameObject _bullet;

    [SerializeField]
    private float _timePassed;

    Sound3dController sound3dController;
    private void Awake()
    {
        sound3dController = GetComponent<Sound3dController>();
    }

    // Update is called once per frame
    void Update()
    {
        _timePassed -= Time.deltaTime;
        if (_timePassed <= 0)
        {
            Instantiate(_bullet, transform.position, transform.rotation).GetComponent<BulletMovement>().dir = transform.right;
            _timePassed = _timeBetweenBullets;
            sound3dController.PlaySound();
        }
    }
}
