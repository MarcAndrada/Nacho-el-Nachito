using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class PlayerDashController : MonoBehaviour
{
    private PlayerController _playerController;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D coll;
    
    [SerializeField]
    private LayerMask floorLayer;
    
    // DASH VARIABLES

    public bool _canDash;
    
    private Vector2 vdirection;
    [HideInInspector] public  Vector2 _dashDirection;
    public bool _isDirectional;
    
    [SerializeField] private float _dashSpeed = 28f;
    [SerializeField] private float _dashTime = 0.3f;
    private float _dashTimePassed = 0f;
    public float capsuleOffset = 0.15F;
    public float speedDashController = 7;

    [Header("Souds"), SerializeField]
    private AudioClip[] dashSounds;
    [SerializeField]
    private GameObject dashParticles;

    private bool _dashDirectional;
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
    }


    public void StartDash(Vector2 _dashDir) 
    {
        _dashDirection = _dashDir;
        _playerController.ChangeState(PlayerController.PlayerStates.DASH);

        AudioManager._instance.PlayOneRandomShotSound(dashSounds, 0.55f, 1.45f);
        //Quaternion.Euler(0, 0, 90 * _playerController._movementController._lastDir)

        if (_dashDir == Vector2.zero)
        {
            _dashDir.x = _playerController._movementController._lastDir;
        }
        Vector2 startPos = transform.position;
        if (Physics2D.Raycast(transform.position, _dashDir, 1.5f, floorLayer))
        {
            startPos -= _dashDir * 1.5f;
        }


        GameObject _particulita = Instantiate(dashParticles, startPos, Quaternion.identity);        
        _particulita.transform.rotation = Quaternion.LookRotation(new Vector3(_dashDir.x, _dashDir.y));
    }

    public void Dash()
    {
        _playerController._movementController.externalForces = Vector2.zero;
        if (_dashDirection != Vector2.zero)
        {
            vdirection = _dashDirection * _dashSpeed;
            _dashDirectional = true;
        }
        else
        {
            vdirection = (Vector2)transform.right * _playerController._movementController._lastDir * _dashSpeed;
            _dashDirectional = false;
        }
        
        rb2d.velocity = vdirection;

        _canDash = false;


    }

    public void DashTimer()
    {
        _playerController._movementController.SetAirAcceleration(vdirection.x);
        _dashTimePassed += Time.deltaTime;
        if (_dashTimePassed >= _dashTime)
        {
            StopDash();
        }

    }

    public void DashCheckWall()
    {

        Vector3 posOffset = new Vector3(0, coll.size.y / 2f );

        //esta condicion es para que cuando este en el suelo no se encuentre con una plataforma
        //un poco movida y mueva al player cuando no deberia hacerlo


        RaycastHit2D hitUpRaycastHit2D = Physics2D.Raycast(transform.position + posOffset -  new Vector3(0,capsuleOffset), transform.right * _dashDirection.x, 0.55f, floorLayer);
        RaycastHit2D hitDownRaycastHit2D = Physics2D.Raycast(transform.position - posOffset + new Vector3(0, capsuleOffset), transform.right * _dashDirection.x, 0.55f, floorLayer);
        if (_dashDirectional)
        {
            bool hitUp = hitUpRaycastHit2D.collider != null && !hitUpRaycastHit2D.collider.CompareTag("OneWayPlatform");
            bool hitDown = hitDownRaycastHit2D.collider != null && !hitDownRaycastHit2D.collider.CompareTag("OneWayPlatform");
            if (hitUp && hitDown)
            {
                StopDash();
            }
            else if (hitUp)
            {
                rb2d.velocity -= new Vector2(0, speedDashController);
            }
            else if (hitDown)
            {
                rb2d.velocity += new Vector2(0, speedDashController);
            }
        }
        
        
    }

    private void StopDash()
    {
        float _ySpeed = Mathf.Clamp(vdirection.y * _dashSpeed, -5, 5);
        rb2d.velocity = new Vector2(rb2d.velocity.x, _ySpeed);
        _playerController.ChangeState(PlayerController.PlayerStates.NONE);
        _dashTimePassed = 0;
    }
    
    

    private void OnDrawGizmos()
    {
        if (coll != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + new Vector3(0, coll.size.y/2 + capsuleOffset) , transform.position + new Vector3(0, coll.size.y/2 + capsuleOffset) + transform.right * _dashDirection.x);
            Gizmos.DrawLine(transform.position , transform.position + transform.right * _dashDirection.x);
            Gizmos.DrawLine(transform.position + new Vector3(0, -coll.size.y/2 - capsuleOffset) , transform.position + new Vector3(0, -coll.size.y/2f - capsuleOffset) + transform.right * _dashDirection.x);
        }
    }
}