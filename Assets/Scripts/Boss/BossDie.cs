using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    private ManagerScene managerScene; 
    // Start is called before the first frame update
    void Start()
    {
        managerScene = GetComand<ManagerScene>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
