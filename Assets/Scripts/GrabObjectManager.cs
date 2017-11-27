using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectManager : MonoBehaviour
{
    public static GrabObjectManager _instance;
    GrabObjectState[] grabObjectStates;

    void Awake()
    {
        _instance = this;
        Find();
        ResetAllPosAndRos();
    }

    public void ResetAllPosAndRos()
    {
        if (grabObjectStates == null) return;

        for (int i = 0; i < grabObjectStates.Length; i++)
        {
            grabObjectStates[i].ResetObject();
        }

    }

    public void SetAllTrigger(bool isTrigger)
    {
        if (grabObjectStates == null) return;

        for (int i = 0; i < grabObjectStates.Length; i++)
        {
            grabObjectStates[i].SetTrigger(isTrigger);
        }
    }

    public void SetSync(GrabObjectState grabObjectState)
    {
        if (grabObjectStates == null) return;

        for (int i = 0; i < grabObjectStates.Length; i++)
        {
            if (grabObjectStates[i] == grabObjectState)
            {
                grabObjectStates[i].GetComponent<UnitInfo>().enabled = true;
            }
            else
            {
                grabObjectStates[i].GetComponent<UnitInfo>().enabled = false;
            }
        }
    }

    void Find()
    {
        grabObjectStates = FindObjectsOfType<GrabObjectState>();
    }
}
