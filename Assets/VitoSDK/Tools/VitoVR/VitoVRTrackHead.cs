using UnityEngine;
using System.Collections;

public class VitoVRTrackHead : MonoBehaviour {

    public static VitoVRTrackHead instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    [HideInInspector]
    public Vector3 mHeadPos;
    [HideInInspector]
    public Quaternion mHeadRotate;

    [HideInInspector]
    public Transform mTransform;
    // Use this for initialization

    public bool isVRmode;
    public Transform mHead;
    void Start()
    {
        isVRmode = UnityEngine.VR.VRDevice.isPresent;
        if (mHead == null)
        {
            mHead = Camera.main.transform;
        }

        mTransform = transform;
        //Vector3 centerEyes=UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
        if (isVRmode)
        {
            mTransform.localPosition = UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
            mTransform.localRotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
        }
        else
        {
            mTransform.localPosition = mHead.localPosition;// UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
            mTransform.localRotation = mHead.localRotation;// UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
        }

    }


    void LateUpdate()
    {
        if (isVRmode)
        {
            mTransform.localPosition = UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
            mTransform.localRotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
        }
        else
        {
            mTransform.localPosition = mHead.localPosition;// UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
            mTransform.localRotation = mHead.localRotation;// UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
        }
    }
}
