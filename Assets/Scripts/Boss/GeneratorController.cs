using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public bool broke { get; private set; }
    [SerializeField]
    private GameObject particles;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void BreakGenerator()
    {
        spriteRenderer.color = new Color(0.4627451f, 0.3686275f, 0.2588235f);
        broke = true;
        particles.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OVNI"))
        {
            collision.GetComponent<BossMovements>().GetDamage();
            Destroy(gameObject);
        }
    }
}
