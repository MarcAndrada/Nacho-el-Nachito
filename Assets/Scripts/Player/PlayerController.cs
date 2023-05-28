using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public enum PlayerStates {NONE, MOVING, AIR, HOOK,  WALL_SLIDE, CINEMATIC, DASH, DEAD};
    public PlayerStates playerState;
    


    //Aqui crearemos las variables de todos los scritps del player
    private PlayerInput playerInput;
    private PlayerMovementController movementController;
    private PlayerDownController playerDownController;
    private PlayerHookController hookController;
    private PlayerWallJumpController wallJumpController;
    private PlayerRespawn playerRespawn;
    private PlayerDashController dashController;
    private PlayerInteractionController interactionController;
    
    //Variable para acceder a los demas scripts
    public PlayerInput _playerInput => playerInput;
    public PlayerMovementController _movementController => movementController;
    public PlayerDownController _playerDownController => playerDownController;
    public PlayerHookController _hookController => hookController;
    public PlayerWallJumpController _wallJumpController => wallJumpController;
    public PlayerInteractionController _interactionController => interactionController;
    public PlayerDashController _playerDashController => dashController;
    public PlayerRespawn _playerRespawn => playerRespawn;

    private Animator anim;

    public Rigidbody2D rb2d { get; private set; } 
    [SerializeField] public bool _canDash { get; private set; }
    [SerializeField] public bool _canHook { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        _canHook = false;
        _canDash = false;
        AllGetComponents();
    }

    private void AllGetComponents() 
    {
        playerInput = GetComponent<PlayerInput>();
        movementController = GetComponent<PlayerMovementController>();
        hookController = GetComponent<PlayerHookController>();
        wallJumpController = GetComponent<PlayerWallJumpController>();
        playerRespawn = GetComponent<PlayerRespawn>();
        anim = GetComponent<Animator>();
        dashController = GetComponent<PlayerDashController>();
        playerDownController = GetComponent<PlayerDownController>();
        interactionController = GetComponent<PlayerInteractionController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StatesFunctions();
        AnimateCharacter();
        PlayerAimController._instance.UpdateAimMethod();
        Cheats();
        SetPowerUps();
        hookController.CheckActivated();
    }

    private void StatesFunctions() 
    {
        //Aqui segun el estado que este el player haremos una cosa u otra
        switch (playerState)
        {
            case PlayerStates.NONE:
            case PlayerStates.MOVING:
                movementController.CheckGrounded();
                CheckMovementStates();
                movementController.FloorMovement();
                movementController.CheckJumping();
                movementController.CheckSlope();
                movementController.ApplyForces();
                hookController.CheckHookPointNearToCursor();
                playerDownController.CheckIfCanGoOneWayPlat();
                break;
            case PlayerStates.AIR:
                movementController.CheckGrounded();
                CheckMovementStates();
                movementController.AirMovement();
                movementController.CheckJumping();
                movementController.CheckSlope();
                movementController.ApplyForces();
                hookController.CheckHookPointNearToCursor();
                wallJumpController.CheckIfWallSliding();
                playerDownController.CheckIfCanGoOneWayPlat();
                break;
            case PlayerStates.HOOK:
                hookController.MoveHookedPlayer();
                hookController.CheckPlayerNotStucked();
                hookController.CheckHookPointNearToCursor();
                break;
            case PlayerStates.WALL_SLIDE:
                //Bajar la Y
                //Comporbar el salto
                wallJumpController.WallSlide();
                wallJumpController.CheckIfStopSliding();
                hookController.CheckHookPointNearToCursor();
                playerDownController.CheckIfCanGoOneWayPlat();
                break;
            case PlayerStates.DASH:
                dashController.Dash();
                dashController.DashTimer();
                _movementController.CheckGrounded();
                dashController.DashCheckWall();
                wallJumpController.CheckIfWallSliding();
                break;
            case PlayerStates.CINEMATIC:
                // NADA
                break;
            case PlayerStates.DEAD:
                playerRespawn.Respawn();
                break;
            default:
                break;
        }
    }


    private void CheckMovementStates()
    {
        if (movementController._isGrounded)
        {
            //Si esta en el suelo
            if (playerInput._playerMovement != 0)
            {
                ChangeState(PlayerStates.MOVING);
            }
            else
            {
                ChangeState(PlayerStates.NONE);
            }
            
        }
        else
        {
            //Si esta en el aire
            ChangeState(PlayerStates.AIR);
        }
    }

    private void AnimateCharacter()
    {
        if (playerState == PlayerStates.MOVING)
        {
            anim.SetBool("Moving", true);
            anim.SetBool("OnAir", false);
            
        }

        if (playerState == PlayerStates.AIR)
        {
            anim.SetBool("OnAir", true);
        }

        if (playerState == PlayerStates.NONE || playerState == PlayerStates.CINEMATIC)
        {
            anim.SetBool("Moving", false);
            anim.SetBool("OnAir", false);
        }
    }

    public void DeadAnimation()
    {
        anim.SetTrigger("Death");
    }

    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Level 1.1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level 1.2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level 2.1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Level 2.2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Level 3.1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Level 3.2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SceneManager.LoadScene("Level 4.1");
        }
    }

    private void SetPowerUps()
    {
        if (SceneManager.GetActiveScene().name == "Level 1.1" || 
            SceneManager.GetActiveScene().name == "Level 1.2")
        {
            _canHook = false;
            _canDash = false;
        }
        else if (SceneManager.GetActiveScene().name == "Level 2.1" || 
                 SceneManager.GetActiveScene().name == "Level 2.2" ||
                 SceneManager.GetActiveScene().name == "Level 3.1" || 
                 SceneManager.GetActiveScene().name == "Level 3.2")
        {
            _canHook = false;
            _canDash = true;
        }
        else
        {
            _canHook = true;
            _canDash = true;
        }
    }

    public void ChangeState(PlayerStates _nextState)
    {
        switch (_nextState)
        {
            case PlayerStates.NONE:
                break;
            case PlayerStates.MOVING:
                break;
            case PlayerStates.AIR:
                break;
            case PlayerStates.HOOK:
                break;
            case PlayerStates.WALL_SLIDE:
                movementController.externalForces = Vector2.zero;
                break;
            case PlayerStates.CINEMATIC:
                break;
            case PlayerStates.DASH:
                break;
            case PlayerStates.DEAD:
                playerRespawn.Die();
                break;
            default:
                break;
        }
        playerState = _nextState;
    }

}
