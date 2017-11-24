using Pvr_UnitySDKAPI;
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
        FacadeManager._instance.SwitchPicoHome(true);
    }

    void OnDestroy()
    {
        MicController.instance.enabled = true;
        // Debug.Log("OnDestroy-----");
    }

    void Update()
    {
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

            if (timer >= 3)
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
