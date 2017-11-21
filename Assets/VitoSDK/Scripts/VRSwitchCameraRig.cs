using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;

#endif

[Serializable]
public class VirtualCameraRig
{
    public GameObject parentObj;
    public Transform head;
}

[Serializable]
public class HTCCameraRig:VirtualCameraRig
{
    public Transform trackingSpace;
    public Transform leftHand;
    public Transform rightHand;
}
[Serializable]
public class PicoCameraRig : VirtualCameraRig
{
    public Transform leftEye;
    public Transform rightEye;
}
[Serializable]
public class GearCameraRig : VirtualCameraRig
{
    public Transform leftEye;
    public Transform rightEye;
}
[Serializable]
public class VDpnCameraRig : VirtualCameraRig
{
    public Transform leftEye;
    public Transform rightEye;
}

[Serializable]
public class IVRCameraRig : VirtualCameraRig
{
    public Transform leftEye;
    public Transform rightEye;
}

public class VRSwitchCameraRig : MonoBehaviour {

    public static VRSwitchCameraRig instance;
#if HTCVIVE
    public static EVrType mVRType = EVrType.EVT_HTC_Vive;    
#elif IVR
     public static EVrType mVRType = EVrType.EVT_IVR;
#elif DPN
    public static EVrType mVRType = EVrType.EVT_DPN;
#else
    public static EVrType mVRType = EVrType.EVT_GearVR;
#endif
#if UNITY_EDITOR
    [ContextMenu("SetExecuteOrder")]
    public void SetScriptOrder()
    {
        short order = short.MinValue+1;
        MonoScript monoScript = MonoScript.FromMonoBehaviour(this);
        MonoImporter.SetExecutionOrder(monoScript, order);
    }
#endif

    public HTCCameraRig mHTCRig;
    public GearCameraRig mGearRig;
    public VDpnCameraRig mDpnRig;
    public IVRCameraRig mIVRRig;
    public PicoCameraRig mPicoRig;

    public Transform mLeftEye
    {
        get
        {
            switch (mVRType)
            {
                case EVrType.EVT_GearVR:
                    return mGearRig.leftEye;
                case EVrType.EVT_DPN:
                    return mDpnRig.leftEye;
                case EVrType.EVT_HTC_Vive:
                    return null;
                case EVrType.EVT_IVR:
                    return mIVRRig.leftEye;
                case EVrType.EVT_NONE:
                    return mGearRig.leftEye;
                case EVrType.EVT_OTHER1:
                    return mPicoRig.leftEye;
            }
            return null;
        }
    }
    public Transform mLeftHand
    {
        get
        {
            switch (mVRType)
            {

                case EVrType.EVT_HTC_Vive:
                    return mHTCRig.leftHand;
            }
            return null;
        }
    }
    public Transform mRightHand
    {
        get
        {
            switch (mVRType)
            {

                case EVrType.EVT_HTC_Vive:
                    return mHTCRig.rightHand;
            }
            return null;
        }
    }

    public Transform mRightEye
    {
        get
        {
            switch (mVRType)
            {
                case EVrType.EVT_GearVR:
                    return mGearRig.rightEye;
                case EVrType.EVT_DPN:
                    return mDpnRig.rightEye;
                case EVrType.EVT_HTC_Vive:
                    return null;
                case EVrType.EVT_IVR:
                    return mIVRRig.rightEye;
                case EVrType.EVT_NONE:
                    return mGearRig.rightEye;
                case EVrType.EVT_OTHER1:
                    return mPicoRig.rightEye;
            }
            return null;
        }
    }


    public Transform mHead
    {
        get
        {
            switch (mVRType)
            {
                case EVrType.EVT_GearVR:
                    return mGearRig.head;
                case EVrType.EVT_DPN:
                    return mDpnRig.head;
                case EVrType.EVT_HTC_Vive:
                    return  mHTCRig.head ;
                case EVrType.EVT_IVR:
                    return mIVRRig.head;
                case EVrType.EVT_NONE:
                    return mGearRig.head;
                case EVrType.EVT_OTHER1:
                    return mPicoRig.head;
            }
            return null;
        }
    }
    void Awake()
    {
        instance = this;
        if(VitoSDKConfig2.instance.type != EVrType.EVT_NONE)
        {
            mVRType = VitoSDKConfig2.instance.type;
        }
        ConnectionClientConfig.evrType = mVRType;
        if(mVRType == EVrType.EVT_HTC_Vive)
        {
            if (mHTCRig.parentObj != null)
            {
                mHTCRig.parentObj.SetActive(true);
            }
        }else
        {
            if (mHTCRig.parentObj != null)
            {
                mHTCRig.parentObj.SetActive(false);
            }
        }

        if(mVRType!=EVrType.EVT_DPN)
        {
            DestroyImmediate(mDpnRig.parentObj);
        }

        if(mVRType!=EVrType.EVT_IVR)
        {
            DestroyImmediate(mIVRRig.parentObj);
        }

        if(mVRType!=EVrType.EVT_GearVR&&mVRType!=EVrType.EVT_NONE)
        {
            DestroyImmediate(mGearRig.parentObj);
        }
        Debug.Log("zzzzzzzzzzz" + mVRType);
        if (mVRType != EVrType.EVT_OTHER1)
        {
            DestroyImmediate(mPicoRig.parentObj);
        }

        Debug.Log("zzzzzzzzzzz" + mHead == null);
        if (mHead.gameObject!=null)
        {
            ConnectionClientConfig.mhead = mHead.gameObject;
        }
        if(mLeftHand!=null)
        {
            ConnectionClientConfig.mLeftHand = mLeftHand.gameObject;
        }
        if(mRightHand!=null)
        {
            ConnectionClientConfig.mRightHand = mRightHand.gameObject;
        }
        
    }

    // Use this for initialization
    void Start () {
	   	
	}
}
