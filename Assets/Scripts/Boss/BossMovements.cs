using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMovements : MonoBehaviour
{
    public enum BossStates {NONE, LIGHTNING, ABDUCING, THROWING, DEAD};
    public BossStates bossState;

    [Header("Movement"), SerializeField]
    private Transform playerPos;

    private int bossHP = 3;

    [HideInInspector]
    public Vector3 starterPos;
    [HideInInspector]
    public bool constantY;
    [HideInInspector]
    public float yPos;
    [HideInInspector]
    public float xSpeed;
    [HideInInspector]
    public float ySpeed;


    [Header("Lightning"), SerializeField]
    private GameObject lightningObj;
    [Header("Abduce"), SerializeField]
    private GameObject abduceObj;

    [Header("Throw"), SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private float bombCD;
    private float bombTimeWaited;

    private BossStates lastState;
    
    [Header("Components"), SerializeField]
    private CinematicManager _cm;
    private Rigidbody2D rb;
    private AbducePlayerUp abduceCont;
    private PlayerController playerController;
    [HideInInspector]
    public BossZoneController currentZone;
    private Animator animator;

    [Space, Header("UI"), SerializeField]
    private GameObject[] bossHearts;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        abduceCont = abduceObj.GetComponentInChildren<AbducePlayerUp>();
        playerController = playerPos.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        starterPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_cm.isCinematicMode)
        {
            switch (bossState)
            {
                case BossStates.LIGHTNING:
                    ChasePlayer();
                    break;
                case BossStates.ABDUCING:
                    ChasePlayer();
                    break;
                case BossStates.THROWING:
                    ChasePlayer();
                    WaitToThrowBomb();
                    break;
                case BossStates.DEAD:
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GetDamage();
        }
    }

    private void FixedUpdate()
    {
        if (playerController.playerState == PlayerController.PlayerStates.DEAD)
        {
            ResetBoss();
        }
    }

    private void ChasePlayer()
    {
        Vector2 target;
        if (!constantY)
        {
            target = new Vector2(playerPos.position.x, playerPos.position.y + yPos);
            if (abduceCont.abducingPlayer)
            {
                target.y = transform.position.y;
            }
        }
        else
        {
            target = new Vector2(playerPos.position.x, yPos);
        }
        Vector2 newPos;
        newPos.x = Vector2.MoveTowards(rb.position, target, xSpeed * Time.deltaTime).x;
        newPos.y = Vector2.MoveTowards(rb.position, target, ySpeed * Time.deltaTime).y;

        rb.position = newPos;
    }

    public void ChangeBossState(BossStates _bossState)
    {
        bossState = _bossState;
        switch (_bossState)
        {
            case BossStates.NONE:
                abduceObj.SetActive(false);
                lightningObj.SetActive(false);
                break;
            case BossStates.LIGHTNING:
                abduceObj.SetActive(false);
                lightningObj.SetActive(true);

                break;
            case BossStates.ABDUCING:
                abduceObj.SetActive(true);
                lightningObj.SetActive(false);
                Invoke("ChangeToThrowBomb", 6);
                break;
            case BossStates.THROWING:
                abduceObj.SetActive(false);
                lightningObj.SetActive(false);
                bombTimeWaited = 0;
                Invoke("ChangeToAbduce", 8);
                break;
            case BossStates.DEAD:
                abduceObj.SetActive(false);
                lightningObj.SetActive(false);

                break;
            default:
                break;
        }
    }
    private void ChangeToThrowBomb()
    {
        if (bossState == BossStates.ABDUCING)
            ChangeBossState(BossStates.THROWING);
    } 
    private void ChangeToAbduce()
    {
        if (bossState == BossStates.THROWING)
            ChangeBossState(BossStates.ABDUCING);
    }
    private void ChangeLastState()
    {
        if (bossState == BossStates.NONE)
            ChangeBossState(lastState);
    }

    private void WaitToThrowBomb()
    {
        bombTimeWaited += Time.deltaTime;
        if (bombTimeWaited >= bombCD)
        {
            bombTimeWaited = 0;
            //Crear Bala
            BossBombController currentBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity).GetComponent<BossBombController>();
            currentBomb.endPos = playerPos.position;
        }
    }

    public void ResetBoss()
    {
        transform.position = starterPos;
    }

    public void GetDamage()
    {
        lastState = bossState;
        ChangeBossState(BossStates.NONE);
        bossHP--;
        UpdateBossLife();
        if (bossHP <= 0)
        {
            Die();
        }
        else
        {
            currentZone.OpenExitDoor();
            animator.SetTrigger("Damaged");
            Invoke("ChangeLastState", 2.5f);
        }
    }

    private void UpdateBossLife() 
    {
        bossHearts[bossHP].SetActive(false);
    }
    
    private void Die()
    {
        ChangeBossState(BossStates.DEAD);
        animator.SetTrigger("Dead");
        Invoke("GoMainMenu", 7.5f);
    }

    private void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
