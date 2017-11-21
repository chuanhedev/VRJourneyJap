using UnityEngine;
using System.Collections;

public class IVRCameraCtrl : MonoBehaviour {
    public Transform mainEye;
    public Transform otherEye;
    public Transform centerEye;
    private bool needSync = false;

	// Use this for initialization
	void Start () {

	    if(ActionController.instance!=null&&VitoPlugin.CT==CtrlType.Admin)
        {
            needSync = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(needSync)
        {
            centerEye.localRotation= otherEye.localRotation = mainEye.localRotation;
            centerEye.localPosition= otherEye.localPosition = mainEye.localPosition;
            centerEye.localScale= otherEye.localScale = mainEye.localScale;

        }
	}
}
