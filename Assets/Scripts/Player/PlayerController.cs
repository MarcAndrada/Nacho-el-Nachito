using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {NONE, MOVING, AIR, HOOK, DEAD };
    public PlayerStates playerState;


    //Aqui crearemos las variables de todos los scritps del player
    private PlayerInput playerInput;

    

    // Start is called before the first frame update
    void Awake()
    {
        AllGetComponents();
    }

    private void AllGetComponents() 
    {
        playerInput = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        StatesFunctions();
    }

    private void StatesFunctions() 
    {
        switch (playerState)
        {
            case PlayerStates.NONE:
                break;
            case PlayerStates.MOVING:
                break;
            case PlayerStates.AIR:
                break;
            case PlayerStates.HOOK:
                break;
            case PlayerStates.DEAD:
                break;
            default:
                break;
        }

        
    }


}
