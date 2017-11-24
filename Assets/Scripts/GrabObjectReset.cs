using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectReset : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Transform grabObjectTrans = other.transform;
        GrabObjectState grabObjectState = grabObjectTrans.GetComponent<GrabObjectState>();
        if (grabObjectTrans.gameObject.layer != LayerMask.NameToLayer("GrabObject")) return;

        // Debug.Log("OnTriggerEnter");
      
        StartCoroutine(DoReset(grabObjectState));
    }

    IEnumerator DoReset(GrabObjectState grabObjectState)
    {
        yield return new WaitForSeconds(2);

        grabObjectState.ResetObject();
    }
}
