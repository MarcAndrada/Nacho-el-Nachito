using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;

    private LayerMask floorLayer;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        floorLayer = LayerMask.GetMask("Floor");
    }


    public void MovePlayer() 
    {
        CheckOnRamp();
        rb2d.velocity = new Vector2();
    }

    private void CheckOnRamp() 
    {

        //Hacemos un Raycast para pillar el raycasthit que nos da informacion del suelo
        RaycastHit2D[] hit = new RaycastHit2D[3];
        ContactFilter2D filter = new ContactFilter2D();
        //Lanzamos el rayo hacia abajo desde la parte inferior de la colision
        Physics2D.Raycast(transform.position - new Vector3(0, capsuleCollider.size.y / 2), Vector2.down, filter, hit, 0.5f);
        //Calculamos el angulo del suelo segun 
        float angleFloor = Vector2.Angle(hit[0].normal, Vector2.up);

        if (angleFloor < 10)
        {
            //Eso es que esta en un suelo plano
        }

    }

}
