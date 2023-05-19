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

    [Header("Particles"), SerializeField]
    private ParticleSystem cheeseParticles;
    [SerializeField]
    private GameObject cheeseHitParticles;

    [Header("Sound"), SerializeField]
    private AudioClip hookHit;
    [SerializeField]
    private AudioClip hookMissSound;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerHookController = starterPos.GetComponentInParent<PlayerHookController>();

    }

    private void Start()
    {
        cheeseParticles.Stop();
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
        cheeseParticles.Play();
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
            cheeseParticles.transform.position = transform.position;
        }
    }
    private void CheckDistance()
    {
        if (Vector2.Distance(transform.position, playerHookController.transform.position) > playerHookController.hookRange / 2 && !hooked)
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
                Instantiate(cheeseHitParticles, transform.position, Quaternion.identity);
                AudioManager._instance.Play2dOneShotSound(hookMissSound, 0.4f);

            }
            else
            {
                hooked = true;
                AudioManager._instance.Play2dOneShotSound(hookHit);
                //Spawnear particulas de gancho dado
                Instantiate(cheeseHitParticles, transform.position, Quaternion.identity);
                playerHookController.playerController._playerDashController._canDash = true;
            }
        }
    }

    public void DisableHook() 
    {
        playerHookController.canHook = true;
        cheeseParticles.Stop();
        gameObject.SetActive(false);
    }

}
