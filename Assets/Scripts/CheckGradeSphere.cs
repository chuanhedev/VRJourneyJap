using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGradeSphere : MonoBehaviour
{
    [SerializeField] Transform rightTransform;
    [SerializeField] int num;
    [SerializeField] float maxPutDis;

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
                GrabObjectState grabObjectState = other.transform.GetComponent<GrabObjectState>();
                Debug.Log(grabObjectState);

                if (grabObjectState)
                {
                    float putDis = Vector3.Distance(grabObjectState.transform.position, rightTransform.position);
                    float prop;
                    if (putDis > maxPutDis)
                    {
                        prop = 0;
                    }
                    else
                    {
                        if (putDis < maxPutDis / 5)
                        {
                            prop = 1;
                        }
                        else
                        {
                            prop = 1 - putDis / maxPutDis;
                        }
                    }
                    //Debug.Log((int)(num * prop));
                    tablewareManager.AddGrade((int)(num * prop));
                }
            }
        }
    }
}
