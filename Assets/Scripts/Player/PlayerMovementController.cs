using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Var")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float accelSpeed;
    [SerializeField]
    private float dragSpeed;

    private Vector2 moveDir;
    private float acceleration;
    private bool accelerating;
    private float lastDir; //Aqui guardaremos la direccion donde nos indica el ultimo input al que le hemos dado para que cuando este a 0 tener la direccion

    [Header("Air Movement Var")]
    [SerializeField]
    private bool isGrounded;
    public bool _isGrounded => isGrounded;
    [SerializeField]
    private float checkFloorDistance;
    private LayerMask floorLayer;



    private PlayerController playerController;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        floorLayer = LayerMask.GetMask("Floor");
    }


    public void MovePlayer() 
    {
        CheckOnRamp();
        CheckAcceleration();
        rb2d.velocity = moveDir * moveSpeed * acceleration;
    }

    private void CheckOnRamp() 
    {


        Vector2 _dir = Vector2.right;


        //Hacemos un Raycast para pillar el raycasthit que nos da informacion del suelo
        RaycastHit2D hit = DoRaycast(transform.position - new Vector3(0, capsuleCollider.size.y / 2), Vector2.down, 0.25f, floorLayer);

        //Lanzamos el rayo hacia abajo desde la parte inferior de la colision
        if (hit)
        {
            //Calculamos el angulo del suelo segun 
            float angleFloor = Vector2.Angle(hit.normal, Vector2.up);

            if (angleFloor > 15)
            {
                //Esta en una Rampa
                //Aqui tocaria editar la direccion de movimiento segun la inclinacion de la rampa
                // Editar _dir

            }
        }


        //Aqui para hacer el movimiento mas fluido en el caso de que 
        if (playerController._playerInput._playerMovement != 0)
        {
            moveDir = _dir * playerController._playerInput._playerMovement;
            lastDir = playerController._playerInput._playerMovement;
        }
        else
        {
            moveDir = _dir * lastDir;
        }

    }

    private void CheckAcceleration() 
    {
        //Revisamos si le estamos dando a algun input o no
        if (playerController._playerInput._playerMovement == 0)
        {
            accelerating = false;
        }
        else
        {
            accelerating = true;
        }

        ApplyAcceleration();

    }
    private void ApplyAcceleration()
    {
        if (accelerating)
        {
            //En caso de estar acelerando iremos sumando a la aceleracion la accelSpeed para que acelere poco a poco
            acceleration += Time.deltaTime * accelSpeed;
        }
        else
        {
            //En caso de no estar dandole a ningun input de movimiento le restaremos a variable de dragSpeed para frenar poco a poco el player
            acceleration -= Time.deltaTime * dragSpeed;
        }
        //Hacemos un clamp para que no pase de 0 o 1 
        acceleration = Mathf.Clamp01(acceleration);

    }

    public void CheckGrounded()
    {
        bool _actuallyGrounded = false;
        //Hacemos un raycast a los pies del player desde el punto central
        RaycastHit2D hit = DoRaycast(transform.position - new Vector3(0, capsuleCollider.size.y / 2), Vector2.down, 0.25f, floorLayer); 
        if (hit)
        {
            _actuallyGrounded = true;
        }
        else
        {
            //Si no choca lo lanzamos de uno de los lados (en este caso el de la izquierda)
            hit = DoRaycast(transform.position - new Vector3(-capsuleCollider.size.x / 2, capsuleCollider.size.y / 2), Vector2.down, 0.25f, floorLayer);
            if (hit)
            {
                _actuallyGrounded |= true;
            }
            else
            {
                //Y para acabar si no ha detectado suelo en la izquierda lo lanzaremos a la derecha
                hit = DoRaycast(transform.position - new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2), Vector2.down, 0.25f, floorLayer);

            }
        }




        isGrounded = _actuallyGrounded;
    }


    private RaycastHit2D DoRaycast(Vector2 _pos, Vector2 _dir, float _distance, LayerMask _layer)
    {
        // Esta funcion es para simplificar el hacer un raycast que da un palo que flipas loquete ;)
        RaycastHit2D[] _hit = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = _layer;

        Physics2D.Raycast(_pos, _dir, filter, _hit, _distance);



        return _hit[0];
    }

}
