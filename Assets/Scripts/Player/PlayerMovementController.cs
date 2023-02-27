using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Var")]
    [SerializeField, Tooltip("Velocidad en la que se mueve el player")]
    private float moveSpeed; 
    [SerializeField, Tooltip("Acceleracion del player en el suelo")]
    private float floorAccelSpeed;
    [SerializeField, Tooltip("Friccion del player en el suelo")]
    private float floorDragSpeed;

    private Vector2 moveDir;
    private float acceleration;
    private bool accelerating;
    private float lastDir; //Aqui guardaremos la direccion donde nos indica el ultimo input al que le hemos dado para que cuando este a 0 tener la direccion

    [Header("Grounded Var")]
    [SerializeField, Tooltip("Nos dice si estamos o no en el suelo")]
    private bool isGrounded;
    public bool _isGrounded => isGrounded;
    [SerializeField, Tooltip("El Rango de los rayos que se usan para comprobar el suelo")]
    private float checkFloorRange;
    [SerializeField, Tooltip("Layer del suelo donde chocaran los rayos del movimiento")]
    private LayerMask floorLayer;
    [SerializeField, Tooltip("Nos dice si podemos hacer el coyote time o no")]
    private bool canCoyote;
    [SerializeField, Tooltip("Tiempo de margen que tendremos para usar el coyote time")]
    private float coyoteTime;
    private float coyoteWaited;

    [Header("Air Movement Var"), SerializeField]
    private float airAccelSpeed;
    [SerializeField]
    private float airDragSpeed;
    private float minAirSpeed = 0.05f;


    [Header("Jump Var"), SerializeField, Tooltip("Velocidad del salto")]
    private float jumpSpeed;
    private bool canJump = true;
    [SerializeField, Tooltip("Tiempo maximo de duracion del salto (cuanto mas alto mas durara el salto y mas alto saltara)")]
    private float timeJumping;
    private float timeWaitedJumping = 0;
    private bool jumpInputPerformed = false;
    
    [Header("Slope Var"), SerializeField, Tooltip("Esto comprueba si encontramos un escalon que el personaje pueda llegar a pasar por encima sin que tenga que saltar")]
    private float slopeOffset;
    [SerializeField, Tooltip("La cantidad de divisiones que hara de la colision para sacar los puntos donde comprueba posicion de los rayos para el Slope")]
    private float slopeCapsuleDiv;

    private Vector2 movementForces;



    private PlayerController playerController;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetForces() 
    {
        movementForces = Vector2.zero;
    }
    public void ApplyForces() 
    {
        
        rb2d.velocity = movementForces;
    }

    #region Floor Movement Functions
    public void FloorMovement() 
    {
        CheckOnRamp();
        CheckAcceleration();
        ApplyFloorAcceleration();
        FlipCharacter();
        movementForces = moveDir * moveSpeed * acceleration;

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


        //Aqui para hacer el movimiento mas fluido en el caso de que no le demos a ningun input que frene
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


    }
    private void ApplyFloorAcceleration()
    {

        if (accelerating)
        {
            //En caso de estar acelerando iremos sumando a la aceleracion la accelSpeed para que acelere poco a poco
            acceleration += Time.deltaTime * floorAccelSpeed;
        }
        else
        {
            //En caso de no estar dandole a ningun input de movimiento le restaremos a variable de dragSpeed para frenar poco a poco el player
            acceleration -= Time.deltaTime * floorDragSpeed;
        }
        //Hacemos un clamp para que no pase de 0 o 1 
        acceleration = Mathf.Clamp01(acceleration);

    }

    public void CheckGrounded()
    {
        bool _actuallyGrounded = false;
        //Variable para poner el punto de aparicion de los raycast, asi queda mas limpio el codigo
        Vector3 spawnPosRay;
        //Creamos una variable para el offset hacia arriba debido a que sin el hay problemas con la deteccion del suelo cuando esta muy cerca
        Vector3 posOffset = Vector3.up * checkFloorRange / 2;

        //Hacemos un raycast a los pies del player desde el punto central
        spawnPosRay = transform.position - new Vector3(0, capsuleCollider.size.y / 2) + posOffset;
        RaycastHit2D hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);
        if (hit)
        {
            _actuallyGrounded = true;
        }
        else
        {
            //Si no choca lo lanzamos de uno de los lados (en este caso el de la izquierda)
            spawnPosRay = transform.position - new Vector3(-capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
            hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);
            if (hit)
            {
                _actuallyGrounded = true;
            }
            else
            {
                //Y para acabar si no ha detectado suelo en la izquierda lo lanzaremos a la derecha
                spawnPosRay = transform.position - new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
                hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);

                if (hit)
                {
                    _actuallyGrounded = true;
                }

            }
        }

        isGrounded = _actuallyGrounded;

        if (_actuallyGrounded)
        {
            canCoyote = true;
            canJump = true;
        }
        else if(canCoyote)
        {
            WaitCoyoteTime();
        }
    }

    private void FlipCharacter()
    {
        if (playerController._playerInput._playerMovement > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerController._playerInput._playerMovement < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void WaitCoyoteTime()
    {
        coyoteWaited += Time.deltaTime;

        if (coyoteWaited >= coyoteTime)
        {
            coyoteWaited = 0;
            canCoyote = false;
            canJump = false;
        }
    }

    #endregion


    #region Air Movement Functions

    public void AirMovement()
    {
        CheckAcceleration();
        ApplyAirAcceleration();
        FlipCharacter();
        movementForces = moveDir * moveSpeed * acceleration;

    }

    private void ApplyAirAcceleration()
    {
        //Decimos que la direccion a la que va a moverse sea la direccion en la que estamos moviendonos
        if (playerController._playerInput._playerMovement != 0)
        {
            moveDir = Vector2.right * playerController._playerInput._playerMovement;
            lastDir = playerController._playerInput._playerMovement;

        }
        else
        {
            moveDir = Vector2.right * lastDir;
        }

        if (accelerating)
        {
            //En caso de estar acelerando iremos sumando a la aceleracion la accelSpeed para que acelere poco a poco
            acceleration += airAccelSpeed * Time.deltaTime;

        }
        else
        {
            acceleration -= airDragSpeed * Time.deltaTime;
        }
        acceleration = Mathf.Clamp01(acceleration);

    }

    public void JumpInputPressed() 
    {
        jumpInputPerformed = true;
        canCoyote = false;
    }
    public void CheckJumping() 
    {
        if (jumpInputPerformed && canJump)
        {
            timeWaitedJumping += Time.deltaTime;
            movementForces.y = jumpSpeed;
            if (timeJumping <= timeWaitedJumping)
            {
                StopJump();
            }
        }
        else
        {
            movementForces.y = rb2d.velocity.y;
        }
    }
    public void StopJump()
    {
        timeWaitedJumping = 0;
        coyoteWaited = 0;
        canJump = false;
        jumpInputPerformed = false;

        if (rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 3);
        }

    }


    #endregion

    public void CheckSlope()
    {
        //En esta funcion comprobamos si esta atascado en medio del aire
        Vector3 posOffset = Vector2.right * checkFloorRange / 2 * playerController._playerInput._playerMovement;
        Vector3 posRay = transform.position + new Vector3(capsuleCollider.size.x / 2 * playerController._playerInput._playerMovement, (-capsuleCollider.size.y / slopeCapsuleDiv) * 2) - posOffset;
        Vector2 rayDir = Vector2.right * playerController._playerInput._playerMovement;
        RaycastHit2D hit = DoRaycast(posRay, rayDir, checkFloorRange, floorLayer);

        if (hit)
        {
            posRay = transform.position + new Vector3(capsuleCollider.size.x / 2 * playerController._playerInput._playerMovement, -capsuleCollider.size.y / slopeCapsuleDiv) - posOffset;
            hit = DoRaycast(posRay, rayDir, checkFloorRange, floorLayer);
            if (!hit)
            {
                //En caso de que la parte inferior de la capsula toque una pared y un poco mas arriba no choque con nada lo subiremos un poco para que no se atasque
                rb2d.position += new Vector2(0, slopeOffset);
            }
        }

    }

    private RaycastHit2D DoRaycast(Vector2 _pos, Vector2 _dir, float _distance, LayerMask _layer)
    {
        // Esta funcion es para simplificar el hacer un raycast que da un palo que flipas loquete ;)
        RaycastHit2D[] _hit = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(_layer);

        Physics2D.Raycast(_pos, _dir, filter, _hit, _distance);

        return _hit[0];
    }

    private void OnDrawGizmos()
    {
        //Dibujamos los rayos con unos gizmos para comprobar cual es la variable mas optima para que quede un buen resultado
        if (capsuleCollider != null)
        {
            Gizmos.color = Color.magenta;
            Vector2 spawnPos;
            Vector3 posOffset = Vector3.up * checkFloorRange / 2;
            spawnPos = transform.position - new Vector3(0, capsuleCollider.size.y / 2) + posOffset;
            Gizmos.DrawLine(spawnPos, spawnPos + Vector2.down * checkFloorRange);
            spawnPos = transform.position - new Vector3(-capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
            Gizmos.DrawLine(spawnPos, spawnPos + Vector2.down * checkFloorRange);
            spawnPos = transform.position - new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
            Gizmos.DrawLine(spawnPos, spawnPos + Vector2.down * checkFloorRange);


            Gizmos.color = Color.blue;
            Vector2 endDir = Vector2.right * playerController._playerInput._playerMovement;
            posOffset = Vector2.right * checkFloorRange / 2 * playerController._playerInput._playerMovement;
            spawnPos = transform.position + new Vector3(capsuleCollider.size.x / 2 * playerController._playerInput._playerMovement, (-capsuleCollider.size.y / slopeCapsuleDiv) * 2) - posOffset;
            Gizmos.DrawLine(spawnPos, spawnPos + endDir * checkFloorRange);
            spawnPos = transform.position + new Vector3(capsuleCollider.size.x / 2 * playerController._playerInput._playerMovement, -capsuleCollider.size.y / slopeCapsuleDiv) - posOffset;
            Gizmos.DrawLine(spawnPos, spawnPos + endDir * checkFloorRange);

        }
        
    }

}
