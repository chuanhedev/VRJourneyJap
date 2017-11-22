using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    HandGrabController handGrabController;
    RaycastHit raycastHit;
    FixedJoint fixedJoint;
    GrabObjectState grabObjectState;
    LayerMask grabObjectLayer;

    void Awake()
    {
        Find();

        Init();
    }

    void Update()
    {
        if (handGrabController.Thumb == null) return;

        Vector3 organPos = handGrabController.Thumb.TipPosition.ToVector3() - handGrabController.Thumb.Direction.ToVector3() * 1;

        Ray ray = new Ray(organPos, handGrabController.Thumb.Direction.ToVector3());

        bool isGrabObject = Physics.Raycast(ray, out raycastHit, Vector3.Distance(handGrabController.Thumb.TipPosition.ToVector3(), organPos), grabObjectLayer);
        //Debug.Log(isGrabObject);

        if (isGrabObject)
        {
            grabObjectState = raycastHit.transform.GetComponent<GrabObjectState>();

            if (grabObjectState)
            {
                //if (grabObjectState.objectGripState != ObjectGripState.None) return;

                // Debug.Log(grabObjectState);
                DoGrab();
            }
        }

        //Debug.Log(handGrabController.IsGrab);

        if (!handGrabController.IsGrab)
        {
            if (!fixedJoint.connectedBody) return;

            //Debug.Log("释放-----");
            GameObject connectedGo = fixedJoint.connectedBody.gameObject;
            connectedGo.GetComponent<GrabObjectState>().objectGripState = ObjectGripState.None;
            fixedJoint.connectedBody = null;
        }

    }

    void DoGrab()
    {
        if (grabObjectState && handGrabController.IsGrab)
        {
            if (handGrabController.Hand.IsRight)
            {
                grabObjectState.objectGripState = ObjectGripState.Right;
            }
            else
            {
                grabObjectState.objectGripState = ObjectGripState.Left;
            }

            // Debug.Log("DoGrab");
            if (!fixedJoint.connectedBody)
                fixedJoint.connectedBody = grabObjectState.GetComponent<Rigidbody>();
        }
    }

    void Find()
    {
        handGrabController = GetComponent<HandGrabController>();
        fixedJoint = GetComponent<FixedJoint>();
    }

    void Init()
    {
        grabObjectLayer = 1 << LayerMask.NameToLayer("GrabObject");
    }
}
