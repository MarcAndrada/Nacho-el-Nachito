using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TriggerCinematic : MonoBehaviour
{
    public int index;
    public Transform cinematicManager;

    bool fired;

    CinematicManager cinematicManagerC;

    // Start is called before the first frame update
    void Start()
    {
        cinematicManagerC = cinematicManager.GetComponent<CinematicManager>();

        fired = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!fired)
        {
            UnityEngine.Debug.Log("Funciona");
            cinematicManagerC.OnTriggerCinematic(index);
            fired = true;
        }
    }
}
