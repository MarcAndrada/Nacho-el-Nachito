using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneController : MonoBehaviour
{

    [SerializeField]
    private BossMovements.BossStates nextBossState;


    [Space, SerializeField]
    private bool constantY;
    [SerializeField]
    private float yPos;

    [Space, SerializeField]
    private float xSpeed;
    [SerializeField]
    private float ySpeed;

    [Space, SerializeField]
    private bool changeSpawnPos;
    [SerializeField]
    private Vector2 spawnPos;

    [Space, SerializeField]
    private bool fightZone;
    [SerializeField]
    private LeverManager enterDoor;
    [SerializeField]
    private LeverManager exitDoor;

    private BossMovements bossController;
    private void Awake()
    {
        bossController = FindObjectOfType<BossMovements>();

    }


    private void ApplyValuesToBoss()
    {
        bossController.ChangeBossState(nextBossState);
        bossController.constantY = constantY;
        bossController.yPos = yPos;
        bossController.xSpeed = xSpeed;
        bossController.ySpeed = ySpeed;
        bossController.currentZone = this;
        if(changeSpawnPos)
            bossController.starterPos = spawnPos;
    }

    public void OpenExitDoor()
    {
        if (fightZone)
            exitDoor.ActivateLever();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyValuesToBoss();
            if (fightZone)
                enterDoor.ActivateLever();
        }
    } 
}
