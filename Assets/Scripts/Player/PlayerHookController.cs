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
    private HookController hookController;
    [SerializeField]
    private Transform hookStarterPos;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private float speed;
    public bool canHook = true;
    [SerializeField]
    private float minDistanceFromPoint;

    private Vector2 posToReach;

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

    public void HookInputPressed()
    {
        if (canHook)
        {
            canHook = false;
            //Comprobar si hay algun punto de enganche alrededor del cursor
            RaycastHit2D hit = CheckHookPointAround();
            if (hit.collider != null)
            {

                RaycastHit2D hit2 = RaycastCheckFloor(hookStarterPos.position , (hit.collider.transform.position - hookStarterPos.position).normalized);
                //Si lo hay comprobar que no haya ninguna pared en medio 
                float distanceOffset = 1;
                Debug.Log(Vector2.Distance(hit.point, hookStarterPos.position));
                if (Vector2.Distance(hit.point, hookStarterPos.position) <= Vector2.Distance(hit2.point, hookStarterPos.position))
                {
                    //Si no hay nada lanzar el gancho al que este mas cerca y bloquear el movimiento
                    Debug.Log("Engancha en el " + hit.collider.transform.position);
                    ThrowHook(hit.collider.transform.position, true);
                }
                else
                {
                    //Si hay algo lanzar el gancho hacia la pared
                    ThrowHook(hit2.point, false);
                    Debug.Log("Hay un hueco al que pegarme pero al chocar contra la pared lanzo el gancho a la pos " + hit2.point);
                    Debug.DrawLine(hookStarterPos.position, hit2.point, Color.red);
                }


            }
            else
            {
                //Si no simplemente lanzarlo para que choque contra la pared sin bloquear el movimiento ni nada
                //Para ello comprobamos cual es la pared que estamos apuntando con la mira
                RaycastHit2D hit2 = RaycastCheckFloor(hookStarterPos.position, (CrosshairController._instance.transform.position - hookStarterPos.position).normalized);
                ThrowHook(hit2.point, false);
                Debug.Log("No engancha");
            }
        }
    }

    RaycastHit2D CheckHookPointAround() 
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(CrosshairController._instance.transform.position, checkRadius, Vector2.zero, 0, hookLayer);
        
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
                if (!closestHit || Vector2.Distance(item.transform.position, CrosshairController._instance.transform.position) < Vector2.Distance(closestHit.transform.position, CrosshairController._instance.transform.position))
                {
                    closestHit = item;
                }
            }

            return closestHit;
        }
    }

    RaycastHit2D RaycastCheckFloor(Vector2 _startPos, Vector2  _dir)
    {
        RaycastHit2D hit = new RaycastHit2D();

        hit = Physics2D.Raycast(_startPos, _dir, Mathf.Infinity, floorLayer);

        return hit;
    }

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
    public void MoveHookedPlayer()
    {
        //Ponerle la velociad al player hacia el punto que tiene que llegar
        rb2d.velocity = (posToReach - (Vector2)transform.position).normalized * speed;

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
        playerController.playerState = PlayerController.PlayerStates.AIR;
        hookController.DisableHook();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (CrosshairController._instance)
        {
            Gizmos.DrawWireSphere(CrosshairController._instance.transform.position, checkRadius);
        }
    }


}
