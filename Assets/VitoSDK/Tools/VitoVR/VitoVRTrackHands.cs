using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR = UnityEngine.VR;
public class VitoVRTrackHands : MonoBehaviour {

    public Transform mLeftHand;
    public Transform mRightHand;

    private bool isVRMode = false;
	// Use this for initialization
	void Start () {
		if( VR.VRDevice.isPresent)
        {
            isVRMode = true;
        }
        if(isVRMode)
        {
            mLeftHand.gameObject.SetActive(true);
            mRightHand.gameObject.SetActive(true);
            //mLeftHand.localPosition = VR.InputTracking.GetLocalPosition(VR.VRNode.LeftHand);
            //mRightHand.localPosition = VR.InputTracking.GetLocalPosition(VR.VRNode.RightHand);
            //mLeftHand.localRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.LeftHand);
            //mRightHand.localRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.RightHand);
        }
        else
        {
            mRightHand.gameObject.SetActive(false);
            mLeftHand.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (VR.VRDevice.isPresent!=isVRMode)
        {
            isVRMode = VR.VRDevice.isPresent;
            if(isVRMode)
            {
                mLeftHand.gameObject.SetActive(true);
                mRightHand.gameObject.SetActive(true);
            }else
            {
                mLeftHand.gameObject.SetActive(false);
                mRightHand.gameObject.SetActive(false);
            }
        }

        //if(isVRMode)
        //{
        //    mLeftHand.localPosition = VR.InputTracking.GetLocalPosition(VR.VRNode.LeftHand);
        //    mRightHand.localPosition = VR.InputTracking.GetLocalPosition(VR.VRNode.RightHand);
        //    mLeftHand.localRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.LeftHand);
        //    mRightHand.localRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.RightHand);
        //}
	}
}
