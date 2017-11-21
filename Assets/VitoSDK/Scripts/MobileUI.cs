using UnityEngine;
using System.Collections;

/// <summary>
/// 移动端的UI控制器.
/// </summary>
public class MobileUI : SingleInstance<MobileUI> {
    public Camera targetCamera;
    public GameObject button_menu;
    public GameObject option;
    public float cacheTargetCameraDistance;
    bool placing = true;
    public float placeSpeed = 10;
    Vector3 targetpos = Vector3.zero;
    // Use this for initialization
    void Start () {
        //option.SetActive(false);
        cacheTargetCameraDistance = Vector3.Distance(transform.position, targetCamera.transform.position);
        targetpos = targetCamera.transform.position + targetCamera.transform.forward * cacheTargetCameraDistance;
    }
    // Update is called once per frame
   

    void Update () {
        targetCamera = Camera.main;
        if (targetCamera == null)
        {
            targetCamera = VRSwitchCameraRig.instance.mHead.GetComponent<Camera>();
        }
        Transform tCameraT = null;
        if (targetCamera!=null)
        {
            tCameraT = targetCamera.transform;
        }
       
        
        if(tCameraT != null)
        {
            if(targetCamera==null)
            {
                placing = false;
            }else
            {
                Vector3 vieportpoint = targetCamera.WorldToViewportPoint(transform.position);

                if (vieportpoint.x < -1.2f || vieportpoint.x > 1.2f || vieportpoint.y < -1.2f || vieportpoint.y > 1.2f)
                {
                    placing = true;
                    targetpos = tCameraT.transform.position + tCameraT.forward * cacheTargetCameraDistance;
                }
            }
            
            if (tCameraT != null)
            {
                //Quaternion lookRotation = Quaternion.LookRotation(targetCamera.transform.forward * cacheTargetCameraDistance - targetCamera.transform.position);

            }
            if (placing)
            {
                float distance = Vector3.Distance(targetpos, transform.position);
                if (distance > 0.1f)
                {
                    transform.position = Vector3.Slerp(transform.position, targetpos, Time.deltaTime * placeSpeed);
                    Quaternion lookRotation = Quaternion.LookRotation(transform.position - tCameraT.position);
                    transform.rotation = lookRotation;
                }
                else
                    placing = false;
            }
        }
         
	}

    public void ShowOption()
    {
        option.SetActive(true);
        //StartCoroutine(IHideOption());
    }

    IEnumerator IHideOption()
    {
        float timer = 0;
        float sumTime = 5;
        while (timer < sumTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        option.SetActive(false);
    }

    
}
