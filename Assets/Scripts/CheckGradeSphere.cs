using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGradeSphere : MonoBehaviour
{
    [SerializeField] int num;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter" + other.transform.name);

        if (other.gameObject.layer != LayerMask.NameToLayer("GrabObject")) return;

        if (other.transform.name == transform.name)
        {
            TablewareManager tablewareManager = TablewareManager._instance;
            lock (tablewareManager.addedGrabObjectList)
            {
                if (tablewareManager.addedGrabObjectList.Contains(other.gameObject)) return;

                tablewareManager.addedGrabObjectList.Add(other.gameObject);

                tablewareManager.AddGrade(num);
            }
        }
    }
}
