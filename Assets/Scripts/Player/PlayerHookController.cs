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
    private SpriteRenderer hookPointIcon;
    private HookController hookController;
    [SerializeField]
    private Transform hookStarterPos;
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


    private PlayerController playerController;
    private Rigidbody2D rb2d;

    [SerializeField]
    private LayerMask hookLayer;
    [SerializeField]
    private LayerMask floorLayer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        hookController = hookObj.GetComponent<HookController>();
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        canHook = true;
    }


    #region Input Functions
    public void HookInputPressed()
    {
        if (canHook)
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

        }
    }

    private void CheckHookWay()
    {
        if (hookPointSelected != null)
        {
            RaycastHit2D hit = RaycastCheckFloor(hookStarterPos.position, (hookPointSelected.transform.position - hookStarterPos.position).normalized);
            //Si lo hay comprobar que no haya ninguna pared en medio 
            if (Vector2.Distance(hookPointSelected.transform.position, hookStarterPos.position) <= Vector2.Distance(hit.point, hookStarterPos.position))
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
            //Para ello comprobamos cual es la pared que estamos apuntando con la mira
            RaycastHit2D hit2 = RaycastCheckFloor(hookStarterPos.position, (PlayerAimController._instance.transform.position - hookStarterPos.position).normalized);
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
                hookPointIcon.enabled = true;
                hookPointIcon.transform.position = nearToCursorHit.transform.position;

                hookPointSelected = nearToCursorHit.transform.gameObject;

            }
            else
            {
                hookPointIcon.enabled = false;
                hookPointSelected = null;
            }
        }

    }

    #endregion

    #region Gamepad Hook Functions

    public void CheckHookGamepadDir()
    {
        //Aqui comprovar segun la direccion del joystick cual es el que esta mas cerca del otro
        float hookPointDot = 0.3f;
        float hookPointDistance = 50;
        float dotOffset = 0.2f;
        float distanceOffset = 10;
        GameObject lookingObject = null;
        foreach (GameObject item in HookGamepadManager._instance.allHooks)
        {
            float provisionalDot = Vector2.Dot(PlayerAimController._instance.gamepadDir.normalized, (item.transform.position - transform.position).normalized);
            float provisionalDist = Vector2.Distance(item.transform.position, transform.position);
            if (provisionalDot >= hookPointDot - dotOffset)
            {
                if (provisionalDot - hookPointDot > -dotOffset && provisionalDist - hookPointDistance < distanceOffset )
                {
                    hookPointDot = provisionalDot;
                    lookingObject = item;
                    hookPointDistance = provisionalDist;
                }
                
            }
        }
        hookPointSelected = lookingObject;

        if (hookPointSelected != null)
        {
            hookPointIcon.enabled = true;
            hookPointIcon.transform.position = hookPointSelected.transform.position;
        }
        else
        {
            hookPointIcon.enabled = false;
        }
        
    }

    #endregion

    #region Throw Hook Functions
    private void ThrowHook(Vector2 _target, bool _stickPoint)
    {
        if (_stickPoint)
        {
            playerController.playerState = PlayerController.PlayerStates.HOOK;
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

    private void StopHook()
    {        
        playerController._movementController.externalForces = rb2d.velocity;
        float xSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeedAtRelease, maxSpeedAtRelease);
        float ySpeed = Mathf.Clamp(rb2d.velocity.y, -maxSpeedAtRelease, maxSpeedAtRelease);
        rb2d.velocity = new Vector2(xSpeed, ySpeed);
        playerController.playerState = PlayerController.PlayerStates.AIR;
        hookController.DisableHook();
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

    RaycastHit2D RaycastCheckFloor(Vector2 _startPos, Vector2 _dir)
    {
        RaycastHit2D hit = new RaycastHit2D();

        hit = Physics2D.Raycast(_startPos, _dir, Mathf.Infinity, floorLayer);

        return hit;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (PlayerAimController._instance)
        {
            Gizmos.DrawWireSphere(PlayerAimController._instance.transform.position, checkRadius);
        }
    }


}
