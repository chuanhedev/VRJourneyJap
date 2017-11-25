using Pvr_UnitySDKAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicoGrab : MonoBehaviour
{
    public static PicoGrab _instance;
    RaycastHit raycastHit;
    RaycastHit groundRaycastHit;
    Transform controller;
    Transform dot;
    Transform direction;
    LayerMask grabObjectLayer;
    Rigidbody grabRigid;
    public bool IsGrab { get; set; }
    float grabDis;
    LayerMask groundLayer;

    void Awake()
    {
        _instance = this;
        Find();
        Init();
    }

    void Update()
    {
        //   if (dot) dot.position = controller.position + direction.forward * 3;
        FacadeManager facadeManager = FacadeManager._instance;
        //if (facadeManager.vitoMode == VitoMode.Ctrl) return;
        if (facadeManager.mode == Mode.Leap) return;

        Ray ray = new Ray(controller.position, (dot.position - controller.position).normalized);

        bool isGrabObject = Physics.Raycast(ray, out raycastHit, 10, grabObjectLayer);
        Debug.Log(isGrabObject);

        if (isGrabObject)
        {
            if (Controller.UPvr_GetKeyDown(Pvr_KeyCode.TOUCHPAD))
            {
                if (IsGrab) return;

                grabRigid = raycastHit.transform.GetComponent<Rigidbody>();
                grabRigid.transform.LookAt(grabRigid.position + Vector3.up);
                grabRigid.transform.GetComponent<GrabObjectState>().SetTrigger(true);
                grabRigid.isKinematic = true;
                //grabDefRotate = raycastHit.transform.rotation;
                grabDis = Vector3.Distance(controller.position, raycastHit.transform.position);
                //  if (!grabObjectList.Contains(raycastHit.transform.gameObject)) grabObjectList.Add(raycastHit.transform.gameObject);
                IsGrab = true;

            }

            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    if (isGrab) return;

            //    grabRigid = raycastHit.transform.GetComponent<Rigidbody>();
            //    grabRigid.transform.LookAt(grabRigid.position + Vector3.up);
            //    raycastHit.transform.GetComponent<GrabObjectState>().SetTrigger(true);
            //    grabRigid.isKinematic = true;
            //    //grabDefRotate = raycastHit.transform.rotation;
            //    grabDis = Vector3.Distance(controller.position, raycastHit.transform.position);
            //    //  if (!grabObjectList.Contains(raycastHit.transform.gameObject)) grabObjectList.Add(raycastHit.transform.gameObject);
            //    isGrab = true;

            //    Debug.Log("GetKeyDown");
            //}
        }

        if (Controller.UPvr_GetKeyUp(Pvr_KeyCode.TOUCHPAD))
        {
            grabRigid.transform.GetComponent<GrabObjectState>().SetTrigger(false);
            grabRigid.isKinematic = false;
            IsGrab = false;
        }

        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    grabRigid.transform.GetComponent<GrabObjectState>().SetTrigger(false);
        //    grabRigid.isKinematic = false;
        //    isGrab = false;
        //    //  Debug.Log("释放");
        //}

        if (IsGrab)
        {

            bool isGround = Physics.Raycast(ray, out groundRaycastHit, 20, groundLayer);

            if (isGround)
            {
                grabRigid.position = groundRaycastHit.point + groundRaycastHit.normal * 0.06f;
            }
            else
            {
                grabRigid.position = controller.position + (dot.position - controller.position).normalized * grabDis;
            }
            // Debug.Log("抓取");
        }

    }

    void Find()
    {
        controller = transform.Find("controller/ppcontroller");
        dot = transform.Find("dot");
        direction = transform.Find("direction");
    }

    void Init()
    {
        grabObjectLayer = 1 << LayerMask.NameToLayer("GrabObject");
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }
}
