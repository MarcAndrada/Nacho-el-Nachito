using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField]
    private Transform starterPos;

    [Header("Movement Hook"), SerializeField]
    private float hookSpeed;
    private Vector2 posToReach;
    private Vector2 throwDir;
    [SerializeField]
    private float minDistanceToPoint;
    private bool stickAtPos;
    [HideInInspector]
    public bool hooked;

    private LineRenderer lineRenderer;
    private Rigidbody2D rb2d;
    private PlayerHookController playerHookController;

    [Header("Sound"), SerializeField]
    private AudioClip hookHit;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerHookController = starterPos.GetComponentInParent<PlayerHookController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        MoveHookToPos();
        SetLinePositions();
    }


    private void SetLinePositions() 
    {
        lineRenderer.SetPosition(0, starterPos.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    
    public void ThrowHook(Vector3 _posToReach, bool _stickAtPos) 
    {
        stickAtPos = _stickAtPos;
        posToReach = _posToReach;
        throwDir = _posToReach - starterPos.position;
        throwDir = throwDir.normalized;

        ResetHookPos();

    }
    public void ResetHookPos()
    {
        hooked = false;
        transform.position = starterPos.position;
        rb2d.simulated = true; 
    }
    private void MoveHookToPos() 
    {
        if (rb2d.simulated)
        {
            throwDir = (posToReach - (Vector2)transform.position).normalized;
            rb2d.velocity = throwDir * hookSpeed;
            CheckIfStopMoving();
        }
    }
    private void CheckDistance()
    {
        if (Vector2.Distance(transform.position, playerHookController.transform.position) > playerHookController.hookRange / 2)
        {
            playerHookController.StopHook();
        }
    }

    private void CheckIfStopMoving() 
    {
        if (Vector2.Distance(transform.position, posToReach) < minDistanceToPoint)
        {
            rb2d.simulated = false;
            transform.position = posToReach;
            if (!stickAtPos)
            {
                DisableHook();
                //Spawnear particulas de gancho fallado

            }
            else
            {
                hooked = true;
                AudioManager._instance.Play2dOneShotSound(hookHit, 0.85f, 1.25f);
                //Spawnear particulas de gancho dado
            }
        }
    }

    public void DisableHook() 
    {
        playerHookController.canHook = true;
        Debug.Log(Vector2.Distance(transform.position, playerHookController.transform.position));
        gameObject.SetActive(false);
    }

}
