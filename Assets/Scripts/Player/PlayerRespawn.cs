using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeDead;
    private float startValue;

    private Vector2 respawnPos;

    private PlayerController pc;

    [Header("Particles"), SerializeField]
    private GameObject deathParticles;

    [Header("Sound"), SerializeField]
    private AudioClip deadSound;
    [SerializeField]
    private AudioClip respawnSound;
    [SerializeField]
    private AudioClip checkPoint;
    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        startValue = timeDead;
        respawnPos = transform.position;
    }

    public void Respawn()
    {
        timeDead -= Time.deltaTime;

        if (timeDead <= 0)
        {
            transform.position = respawnPos;
            timeDead = startValue;
            pc.rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            pc.ChangeState(PlayerController.PlayerStates.NONE);
        }
    }

    public void Die() 
    {
        // Nos aseguramos que no se repita la animación de muerte al colisionar con un obstaculo mientras está muerto
        if (pc.playerState == PlayerController.PlayerStates.DEAD) return;
        
        switch (pc.playerState)
        {
            case PlayerController.PlayerStates.MOVING:
            case PlayerController.PlayerStates.AIR:
                pc.rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.HOOK:
                pc._hookController.StopHook();
                pc._hookController._hookController.ResetHookPos();
                pc._hookController._hookController.DisableHook();
                pc.rb2d.velocity = Vector2.zero;
                break;
            case PlayerController.PlayerStates.WALL_SLIDE:
                pc._wallJumpController.StopSlide();
                break;
        }

        pc.rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        pc.DeadAnimation();
        deathParticles.SetActive(true);
        AudioManager._instance.Play2dOneShotSound(deadSound, 0.2f, 0.7f, 1.3f);
        Invoke("PlayRespawnSound", startValue - respawnSound.length);
    }

    private void PlayRespawnSound() 
    {
        AudioManager._instance.Play2dOneShotSound(respawnSound, 0.1f, 0.7f, 1.3f);
    }
    public void SetRespawnPos(Transform _newPos)
    {
        respawnPos = _newPos.position;
        AudioManager._instance.Play2dOneShotSound(checkPoint, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstaculo"))
        {
            pc.ChangeState(PlayerController.PlayerStates.DEAD);
        }
        else if(collision.CompareTag("OVNI"))
        {
            pc.ChangeState(PlayerController.PlayerStates.DEAD);
        }
    }
}
