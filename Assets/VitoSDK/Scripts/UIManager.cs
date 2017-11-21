using UnityEngine;
using System.Collections.Generic;

public class UIManager : SingleInstance<UIManager>
{
    public Transform UI2DRoot;
    public Transform UI3DRoot;
    public List<GameObject> PlayerHideObjs = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        if (VitoPlugin.IsNetMode && VitoPlugin.CT == CtrlType.Admin)
        {

        }
        else
        {
            foreach (GameObject item in PlayerHideObjs)
            {
                item.SetActive(false);
            }
        }
    }


    

    public void CreatePlayerFocus(string mes, Vector3 pos)
    {
        /*
        PlayerFocus pr = Instantiate<PlayerFocus>(Resources.Load<PlayerFocus>("PlayerFocus"));
        pr.text.text = mes;
        pr.transform.parent = UI3DRoot;
        pr.transform.localScale = Vector3.one;
        pr.transform.position = pos;
        Quaternion lookRotation = Quaternion.LookRotation(pr.transform.position - Camera.main.transform.position);
        pr.transform.rotation = lookRotation;
        ConnectionClient.instance.owner.head.transform.parent = null;
        ConnectionClient.instance.owner.body.rotation = ConnectionClient.instance.owner.head.transform.rotation;
        ConnectionClient.instance.owner.head.transform.parent = ConnectionClient.instance.owner.body;
        ConnectionClient.instance.owner.body.rotation = lookRotation;
        ConnectionClient.instance.owner.body.eulerAngles = new Vector3(ConnectionClient.instance.owner.body.eulerAngles.x, ConnectionClient.instance.owner.body.eulerAngles.y, 0);
        */
        //ConnectionClient.instance.owner.body.rotation = lookRotation;
        //float angle = Quaternion.Angle(ConnectionClient.instance.owner.body.rotation, ConnectionClient.instance.owner.head.transform.rotation);
        //ConnectionClient.instance.owner.body.rotation *= angle;
    }

    //不需要红点的场景功能，调用UIManager实例将该变量设置为false（UIManager.instance.VRCameraHoldOpened = false;）
    public bool VRCameraHoldOpened = true;
    bool cacheVRCameraHoldOpened = false;
    public void ShowVRCamera(bool show)
    {
        if (VRCameraHoldOpened)
            return;
        OneShowVrCamera(show);
    }
    public void OneShowVrCamera(bool show)
    {
        VitoVRReticle.enableReticle = show;
    }
    // Update is called once per frame
    public bool allHide = false;
    void Update()
    {
        //if (cacheVRCameraHoldOpened != VRCameraHoldOpened)
        //{
        //    cacheVRCameraHoldOpened = VRCameraHoldOpened;
        //    VitoVRReticle.enableReticle = VRCameraHoldOpened;
        //}

        if (Input.GetKeyUp(KeyCode.F1))
        {
            UI2DRoot.gameObject.SetActive(allHide);
            UI3DRoot.gameObject.SetActive(allHide);
            allHide = !allHide;
        }
    }

}
