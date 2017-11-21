using UnityEngine;
using System.Collections;

public class SyncMainCam : MonoBehaviour {
    public bool syncRotate = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Camera.main != null)
        {
            transform.position = Camera.main.transform.position;
            if (syncRotate)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
	}
}
