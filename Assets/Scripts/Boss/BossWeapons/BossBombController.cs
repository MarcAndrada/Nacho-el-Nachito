using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBombController : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;


    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float bulletHeight;

    private float bulletTime;

    private Vector2 starterPos;
    [HideInInspector]
    public Vector2 endPos;
    [SerializeField]
    private LayerMask floorMask;

    void Start()
    {
        starterPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(endPos, Vector2.down, Mathf.Infinity, floorMask);

        endPos = hit.point;

    }

    // Update is called once per frame
    void Update()
    {
        bulletTime += Time.deltaTime * bulletSpeed;
        transform.position = Parabola(starterPos, endPos, bulletHeight, bulletTime);
        if (bulletTime >= 1)
        {
            Explosion();
        }
    }
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    private void Explosion() 
    {
        //Hacer aparecer particulas
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().ChangeState(PlayerController.PlayerStates.DEAD);
            Explosion();


        }
    }
}
