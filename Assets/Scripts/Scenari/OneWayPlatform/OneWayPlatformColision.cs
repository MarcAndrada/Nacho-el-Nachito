using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformColision : MonoBehaviour
{
    [SerializeField] private float timeNoColision;

    private BoxCollider2D boxcollider; 

    public void Desactivarcolision()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }
    //// Start is called before the first frame update
    //void Start()
    //{
    //   private bool colision = true; 
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if ()
    //}
}
