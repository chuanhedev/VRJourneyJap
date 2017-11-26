using Pvr_UnitySDKAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterHomeSetting : MonoBehaviour
{
    GradeController gradeController;
    float timer;

    void Awake()
    {
        Find();
    }

    void Start()
    {
        MicController.instance.enabled = false;
        //Debug.Log("Start-----");
        FacadeManager._instance.SwitchPicoGrabMode(true);
    }

    void OnDestroy()
    {
        MicController.instance.enabled = true;
        // Debug.Log("OnDestroy-----");
    }

    void Update()
    {

        //MyPicoLog.SetLog1(Enum.GetName(typeof(CtrlMode), VitoPlugin.CM));

        if (PicoGrab._instance == null) return;

        if (PicoGrab._instance.IsGrab) return;

        if (Controller.UPvr_GetKeyDown(Pvr_KeyCode.TOUCHPAD) || Input.GetKeyDown(KeyCode.A))
        {
            timer = 0;
        }

        if (Controller.UPvr_GetKey(Pvr_KeyCode.TOUCHPAD) || Input.GetKey(KeyCode.A))
        {
            if (gradeController.IsShowing) return;

            timer += Time.deltaTime;

            if (timer >= 2)
            {
                gradeController.ShowGrade();
                timer = 0;
            }
        }
    }

    void Find()
    {
        gradeController = GetComponent<GradeController>();
    }
}
