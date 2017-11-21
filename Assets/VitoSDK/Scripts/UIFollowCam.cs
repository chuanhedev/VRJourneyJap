using UnityEngine;
using System.Collections;

public class UIFollowCam : MonoBehaviour {
    public Transform  targetCamera;
    public float cacheTargetCameraDistance=-1;
    private Transform mTransform;
    private void Awake()
    {
        mTransform = transform;
    }
    // Use this for initialization
    void Start () {
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform;
            if (targetCamera == null && VRSwitchCameraRig.instance != null)
            {
                targetCamera = VRSwitchCameraRig.instance.mHead;
            }
        }
        if (cacheTargetCameraDistance<=0)
        {
            cacheTargetCameraDistance = Vector3.Distance(mTransform.position, targetCamera.position);
        }
        
    }
    	
	// Update is called once per frame
	void LateUpdate () {

        if(targetCamera==null)
        {
            targetCamera = Camera.main.transform;
            if (targetCamera == null && VRSwitchCameraRig.instance != null)
            {
                targetCamera = VRSwitchCameraRig.instance.mHead;
            }
        }
        
        mTransform.position = targetCamera.position + targetCamera.forward * cacheTargetCameraDistance;
        Quaternion lookRotation = Quaternion.LookRotation(mTransform.position - targetCamera.position);
        mTransform.rotation = lookRotation;
    }
}
