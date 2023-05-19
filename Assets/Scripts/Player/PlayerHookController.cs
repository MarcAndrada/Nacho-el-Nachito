using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHookController : MonoBehaviour
{
    [Header("Hook"), SerializeField]
    private GameObject hookObj;
    [SerializeField]
    private GameObject hookPointIcon;
    private HookController hookController;
    
    public HookController _hookController => hookController;
    [SerializeField]
    private Transform hookStarterPos;
    [SerializeField]
    private GameObject rangeObj;
    [SerializeField]
    public float hookRange;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeedAtRelease;
    public bool canHook = true;
    [SerializeField]
    private float minDistanceFromPoint;
    [SerializeField]
    private float hookCD;
    private Vector2 posToReach;

    private GameObject hookPointSelected;


    public PlayerController playerController { get; private set; }
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;

    [SerializeField]
    private LayerMask hookLayer;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField] 
    private GameObject _hookTarget;
    
    [Header("Sound"), SerializeField]
    private AudioClip hookThrow;

    
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        hookController = hookObj.GetComponent<HookController>();
        rb2d = GetComponent<Rigidbody2D>(); 
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        canHook = true;
    }


    #region Input Functions
    public void HookInputPressed()
    {
        if (canHook && playerController.playerState != PlayerController.PlayerStates.DEAD)
        {
            canHook = false;

            switch (PlayerAimController._instance.controllerType)
            {
                case PlayerAimController.ControllerType.MOUSE:
                    CheckHookWay();
                    break;
                case PlayerAimController.ControllerType.GAMEPAD:
                    GamepadHook();
                    break;
                default:
                    break;
            }

            AudioManager._instance.Play2dOneShotSound(hookThrow);

        }
    }

    private void CheckHookWay()
    {
        if (hookPointSelected != null)
        {
            RaycastHit2D hit = RaycastCheckFloor(hookStarterPos.position, (hookPointSelected.transform.position - hookStarterPos.position).normalized, Mathf.Infinity);
            //Si lo hay comprobar que no haya ninguna pared en medio 
            if (hit.collider == null || Vector2.Distance(hookPointSelected.transform.position, hookStarterPos.position) <= Vector2.Distance(hit.point, hookStarterPos.position))
            {
                //Si no hay nada lanzar el gancho al que este mas cerca y bloquear el movimiento
                ThrowHook(hookPointSelected.transform.position, true);
            }
            else
            {
                //Si hay algo lanzar el gancho hacia la pared
                ThrowHook(hit.point, false);
                Debug.DrawLine(hookStarterPos.position, hit.point, Color.red);
            }
        }
        else
        {
            //Si no simplemente lanzarlo para que choque contra la pared sin bloquear el movimiento ni nada
            Vector2 dir = (PlayerAimController._instance.controllerType == PlayerAimController.ControllerType.MOUSE) ? (PlayerAimController._instance.transform.position - hookStarterPos.position).normalized : PlayerAimController._instance.gamepadDir;

            //Para ello comprobamos cual es la pared que estamos apuntando con la mira
            RaycastHit2D hit2 = RaycastCheckFloor(hookStarterPos.position, dir, Mathf.Infinity);
            ThrowHook(hit2.point, false);
        }
    }

    private void GamepadHook()
    {
        CheckHookGamepadDir();
        CheckHookWay();
    }

    #endregion

    #region Mouse Hook Functions
    public void CheckHookPointNearToCursor() 
    {
        if (PlayerAimController._instance.controllerType == PlayerAimController.ControllerType.MOUSE)
        {
            //Comprobar si hay algun punto de enganche alrededor del cursor
            RaycastHit2D nearToCursorHit = CheckHookPointAround();
            if (nearToCursorHit.collider != null)
            {
                hookPointIcon.SetActive(true);
                hookPointIcon.transform.position = nearToCursorHit.transform.position;

                hookPointSelected = nearToCursorHit.transform.gameObject;

            }
            else
            {
                hookPointIcon.SetActive(false);
                hookPointSelected = null;
            }
        }

    }

    #endregion

    #region Gamepad Hook Functions

    public void CheckHookGamepadDir()
    {
        //Aqui comprovar segun la direccion del joystick cual es el que esta mas cerca del otro
        float hookPointDot = 0.4f;
        float hookPointDistance = 50;
        float dotOffset = 0.15f;
        float distanceOffset = 10;
        GameObject lookingObject = null;
        foreach (GameObject item in HookGamepadManager._instance.allHooks)
        {
            float provisionalDot = Vector2.Dot(PlayerAimController._instance.gamepadDir.normalized, (item.transform.position - transform.position).normalized);
            float provisionalDist = Vector2.Distance(item.transform.position, transform.position);
            if (provisionalDot >= hookPointDot - dotOffset && provisionalDist <= hookRange / 2)
            {
                if (provisionalDot - hookPointDot > 0 && provisionalDist - hookPointDistance < distanceOffset)
                {
                    RaycastHit2D hit = RaycastCheckFloor(hookStarterPos.position, (item.transform.position - transform.position).normalized, provisionalDist);

                    if (lookingObject != null && hit.collider != null)
                    {
                        continue;
                    }
                    hookPointDot = provisionalDot;
                    lookingObject = item;
                    hookPointDistance = provisionalDist;
                }
                
            }
        }
        hookPointSelected = lookingObject;

        if (hookPointSelected != null)
        {
            hookPointIcon.SetActive(true);
            hookPointIcon.transform.position = hookPointSelected.transform.position;
        }
        else
        {
            hookPointIcon.SetActive(false);
        }

    }

    #endregion

    #region Throw Hook Functions
    private void ThrowHook(Vector2 _target, bool _stickPoint)
    {
        
        if (_stickPoint)
        {
            playerController.ChangeState(PlayerController.PlayerStates.HOOK);
            posToReach = _target;

        }

        hookObj.SetActive(true);
        hookController.ThrowHook(_target, _stickPoint);

    }
    #endregion

    #region Hook Movement Functions
    public void MoveHookedPlayer()
    {
        if (hookController.hooked)
        {
            //Ponerle la velociad al player hacia el punto que tiene que llegar
            rb2d.velocity = (posToReach - (Vector2)transform.position).normalized * speed;
        }
        

        CheckIfPositionReached();
    }

    private void CheckIfPositionReached()
    {
        if (Vector2.Distance(transform.position, posToReach) <= minDistanceFromPoint)
        {
            StopHook();
        }

    }

    public void StopHook()
    {        
        playerController._movementController.externalForces = rb2d.velocity;
        float xSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeedAtRelease, maxSpeedAtRelease);
        float ySpeed = Mathf.Clamp(rb2d.velocity.y, -maxSpeedAtRelease, maxSpeedAtRelease);
        rb2d.velocity = new Vector2(xSpeed, ySpeed);
        playerController.ChangeState(PlayerController.PlayerStates.AIR);
        hookController.DisableHook();
    }

    public void CheckPlayerNotStucked() 
    {
        float rayRange = 1;
        Vector2 upperPos = transform.position + transform.up * capsuleCollider.size.y / 3;
        Vector2 upperDir = posToReach - upperPos;
        RaycastHit2D upperRay = RaycastCheckFloor(upperPos, upperDir, rayRange);
        Vector2 lowerPos = transform.position - transform.up * capsuleCollider.size.y / 3;
        Vector2 lowerDir = posToReach - lowerPos;
        RaycastHit2D lowerRay = RaycastCheckFloor(lowerPos, lowerDir, rayRange);

        float offsetSpeed = 3;

        if (upperRay.collider != null && lowerRay.collider != null)
        {
            //Romper el gancho
            StopHook();
        }
        else if (upperRay.collider != null)
        {
            //Hay que bajar
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - offsetSpeed);
            
        }
        else if (lowerRay.collider != null)
        {
            //Hay que subir
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y + offsetSpeed);


        }


    }
    #endregion


    RaycastHit2D CheckHookPointAround()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(PlayerAimController._instance.transform.position, checkRadius, Vector2.zero, 0, hookLayer);

        if (hits.Length < 2)
        {
            if (hits.Length != 0)
            {
                return hits[0];
            }
            else
            {
                return new RaycastHit2D();
            }
        }
        else
        {
            RaycastHit2D closestHit = new RaycastHit2D();
            foreach (var item in hits)
            {
                if (!closestHit || Vector2.Distance(item.transform.position, PlayerAimController._instance.transform.position) < Vector2.Distance(closestHit.transform.position, PlayerAimController._instance.transform.position))
                {
                    closestHit = item;
                }
            }
            return closestHit;
        }
    }

    RaycastHit2D RaycastCheckFloor(Vector2 _startPos, Vector2 _dir, float _range)
    {
        RaycastHit2D hit = new RaycastHit2D();

        hit = Physics2D.Raycast(_startPos, _dir, _range, floorLayer);

        return hit;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (PlayerAimController._instance)
        {
            Gizmos.DrawWireSphere(PlayerAimController._instance.transform.position, checkRadius);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hookRange / 2);
    }

    public void CheckActivated()
    {
        _hookTarget.SetActive(playerController._canHook);
        rangeObj.SetActive(playerController._canHook);
        rangeObj.transform.localScale = new Vector2(hookRange, hookRange);
    }
}
