using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovements : MonoBehaviour
{
    public enum BossStates { LIGHTNING, ABDUCING, THROWING, DEAD};
    public BossStates bossState;

    [Header("Movement"), SerializeField]
    private Transform playerPos;
    [SerializeField]
    private float constantYPos;
    [SerializeField]
    private float bossYOffset;

    [Header("Lightning"), SerializeField]
    private GameObject lightningObj;
    [SerializeField]
    private float lightningSpeed;

    [Header("Abduce"), SerializeField]
    private GameObject abduceObj;
    [SerializeField]
    private float abduceSpeed;

    [Header("Throw"), SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private float bombCD;
    private float bombTimeWaited;

    [Header("Components"), SerializeField]
    private CinematicManager _cm;
    private Rigidbody2D rb;
    private AbducePlayerUp abduceCont;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        abduceCont = abduceObj.GetComponentInChildren<AbducePlayerUp>();
        ChangeBossState(bossState);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_cm.isCinematicMode)
        {
            switch (bossState)
            {
                case BossStates.LIGHTNING:
                    ChasePlayer(lightningSpeed, true);
                    break;
                case BossStates.ABDUCING:
                    ChasePlayer(abduceSpeed, false);
                    break;
                case BossStates.THROWING:
                    ChasePlayer(lightningSpeed, true);
                    WaitToThrowBomb();
                    break;
                case BossStates.DEAD:
                    break;
                default:
                    break;
            }

        }  
    }

    private void ChasePlayer(float _chaseSpeed, bool _constantY)
    {
        Vector2 target;
        if (!_constantY)
        {
            target = new Vector2(playerPos.position.x, playerPos.position.y + bossYOffset);
            if (abduceCont.abducingPlayer)
            {
                target.y = transform.position.y;
            }
        }
        else
        {
            target = new Vector2(playerPos.position.x, constantYPos);
        }
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, _chaseSpeed * Time.deltaTime);

        rb.position = newPos;
    }

    public void ChangeBossState(BossStates _bossState)
    {
        bossState = _bossState;
        switch (_bossState)
        {
            case BossStates.LIGHTNING:
                abduceObj.SetActive(false);
                lightningObj.SetActive(true);

                break;
            case BossStates.ABDUCING:
                abduceObj.SetActive(true);
                lightningObj.SetActive(false);
                break;
            case BossStates.THROWING:
                abduceObj.SetActive(false);
                lightningObj.SetActive(false);
                bombTimeWaited = 0;
                break;
            case BossStates.DEAD:
                abduceObj.SetActive(false);
                lightningObj.SetActive(false);
                break;
            default:
                break;
        }
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

    }
}
