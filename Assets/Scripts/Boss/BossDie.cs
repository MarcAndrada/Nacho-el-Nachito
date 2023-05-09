using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDie : MonoBehaviour
{
    private ManagerScene managerScene; 
    
    [SerializeField] private GameObject finishCollider;

    private LeverManager _leverManager;

    private void Awake()
    {
        _leverManager = GetComponent<LeverManager>();
    }

    private void Update()
    {
        if (_leverManager.activated)
        {
            finishCollider.SetActive(true);
        }
    }
}
