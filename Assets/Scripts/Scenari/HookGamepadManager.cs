using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGamepadManager : MonoBehaviour
{
    public static HookGamepadManager _instance;
    public GameObject[] allHooks;
    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance.gameObject);
                _instance = this;
            }
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        allHooks = GameObject.FindGameObjectsWithTag("HookPoint");
    }
}
