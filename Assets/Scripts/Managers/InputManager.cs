using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Hacemos una variable estatica que sera la que controle los inputs en todo el juego
    //Al ser la variable estatica podremos acceder a ella sin necesidad de tener una referencia directa a este script
    public static InputManager _instance; 

    //Cositas del nuevo input system
    [Header("Ingame Actions")]
    public InputActionReference ingameMovementAction;
    public InputActionReference ingameJumpAction;
    public InputActionReference ingameAimAction;
    public InputActionReference ingameHookAction;

    [Header("Menu Actions")]
    public InputActionReference menuMoveAction;


    private void Awake()
    {
        //Aqui comprobamos si tiene algun valor la variable de _instance
        if (_instance != null)
        {
            // Si hay algun valor revisaremos si es diferente a este script (para asegurarnos que no le haya dado una embolia a unity)
            if (_instance != this)
            {
                
                Destroy(_instance); //Si es diferente a este mismo script borraremos el objeto actual de _instance
                _instance = this; //Y le asignaremos este script como nuevo valor
            }
        }
        else
        {
            _instance = this; //En caso de que no tenga valor le asigno este script como valor
        }
    }
}
