using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerHookController : MonoBehaviour
{
    [Header("Hook"), SerializeField]
    private GameObject hookObj;
    private HookController hookController;

    private Vector2 hookDir;


    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        hookController = hookObj.GetComponent<HookController>();
    }

    public void HookInputPressed()
    {
        //Comprobar si hay algun 
    }
    #region Throw Hook Functions
    private void ThrowHook()
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
}
