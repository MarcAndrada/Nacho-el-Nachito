using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActive : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private ParticleSystem fartParticle ;
    [SerializeField] private ParticleSystem normalParticle; 


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
      
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerController.playerState)
        {
            case PlayerController.PlayerStates.MOVING:
            case PlayerController.PlayerStates.AIR:
            case PlayerController.PlayerStates.WALL_SLIDE:
                normalParticle.Play();
                break;
            case PlayerController.PlayerStates.DASH:
                fartParticle.Play();
                break;

            default:
                break;
        }
    }
}
