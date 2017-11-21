using UnityEngine;
using System.Collections;

public class SynLook : MonoBehaviour {
    public Transform target;
    public bool syncTrans = true;
    public bool syncRotate = true;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.rotation = target.rotation;
        if (target != null)
        {
            if(syncTrans)
                transform.position = target.position;
            if(syncRotate)
                transform.eulerAngles = target.eulerAngles;
        }
            
    }
}
