using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectReset : MonoBehaviour
{
    bool isResetting;

    private void OnTriggerEnter(Collider other)
    {
        Transform grabObjectTrans = other.transform;
        GrabObjectState grabObjectState = grabObjectTrans.GetComponent<GrabObjectState>();
        if (grabObjectTrans.gameObject.layer != LayerMask.NameToLayer("GrabObject")) return;

        // Debug.Log("OnTriggerEnter");

        if (isResetting) return;

        isResetting = true;
        StopAllCoroutines();
        StartCoroutine(DoReset(grabObjectState));
    }

    IEnumerator DoReset(GrabObjectState grabObjectState)
    {
        yield return new WaitForSeconds(2);

        grabObjectState.transform.position = grabObjectState.DefPos;
        grabObjectState.transform.rotation = grabObjectState.DefRotate;
        isResetting = false;
    }
}
