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
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerHookController = starterPos.GetComponentInParent<PlayerHookController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetLinePositions();
        MoveHookToPos();
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
        //Debug.DrawLine(starterPos.position, posToReach, Color.green, 10);
        //Debug.DrawLine(starterPos.position, starterPos.position + (Vector3)throwDir * Vector3.Distance(starterPos.position, posToReach), Color.red, 4);
        //transform.rotation = Quaternion.LookRotation(throwDir, Vector3.up);

        ResetHookPos();

    }
    private void ResetHookPos()
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

    private void CheckIfStopMoving() 
    {
        if (Vector2.Distance(transform.position, posToReach) < minDistanceToPoint)
        {
            rb2d.simulated = false;
            transform.position = posToReach;
            if (!stickAtPos)
            {
                DisableHook();
            }
            else
            {
                hooked = true;
            }
        }
    }

    public void DisableHook() 
    {
        //StartCoroutine(playerHookController.WaitToHook());
        playerHookController.canHook = true;
        //Spawnear particulas

        gameObject.SetActive(false);
    }


}
