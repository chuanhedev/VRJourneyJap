using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroController : MonoBehaviour
{

    void Start()
    {
        MicController.instance.enabled = false;
        //Debug.Log("Start-----");
        FacadeManager._instance.SwitchPicoHome(true);
    }

    void OnDestroy()
    {
        MicController.instance.enabled = true;
        // Debug.Log("OnDestroy-----");
    }
}
