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
        ResetAllGrabObject();
    }

    public void ResetAllGrabObject()
    {
        if (grabObjectStates == null) return;

        for (int i = 0; i < grabObjectStates.Length; i++)
        {
            // Debug.Log(grabObjectStates[i].name);
            grabObjectStates[i].ResetObject();
        }

    }

    void Find()
    {
        grabObjectStates = FindObjectsOfType<GrabObjectState>();
    }
}
