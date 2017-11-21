using UnityEngine;
using System.Collections;

public class SycnHtcViveBody : MonoBehaviour {
    
    public Transform target;
    public Vector3 cachePos;
    
    // Use this for initialization
    void Start () {
        cachePos = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 temp = target.position;
        //temp.z = cachePos.z;
        transform.position = temp;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.eulerAngles.y, transform.eulerAngles.z);
    }
}
