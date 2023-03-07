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
    private float checkRadius;

    private Vector2 hookDir;


    private PlayerController playerController;

    [SerializeField]
    LayerMask hookLayer;
  
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        hookController = hookObj.GetComponent<HookController>();
    }

    public void HookInputPressed()
    {
        //Comprobar si hay algun punto de enganche alrededor del cursor
        RaycastHit2D hit = CheckHookPointAround();
        if (hit.collider != null)
        {
            //Si lo hay lanzar el gancho al que este mas cerca y bloquear el movimiento
            Debug.Log("Engancha en el " + hit.collider.transform.position);
            
        }
        else
        {
            //Si no simplemente lanzarlo para que choque contra la pared sin bloquear el movimiento ni nada
            Debug.Log("No engancha");
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


    #region Throw Hook Functions
    private void ThrowHook(Vector2 _target)
    {

    }
    #endregion
    public void MovePlayer()
    {
        playerController.playerState = PlayerController.PlayerStates.HOOK;
    }

    private void CheckIfStopHooking() 
    {
        
    }


    public void SetHookBeforeThrow()
    {

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
