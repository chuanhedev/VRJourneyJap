using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackSetting : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            GameObject cameras = GameObject.Find("player_VRTypeNew");
            Transform head = cameras.GetComponentInChildren<Camera>().transform;
            AudioSource audioSource = cameras.transform.Find("AudioSource").GetComponent<AudioSource>();

            //设置回放的相机和audio
            PlayBackController.instance.InitData(head, audioSource);

            HostUIManager.instance.LeftBtnListEnable(true);
            CameraController cameraController = head.GetComponent<CameraController>();
            if (cameraController) cameraController.enabled = false;
        }
        else
        {
            if (UILoadingBar.instance)
                UILoadingBar.instance.ShowClientPlayBack();
        }
    }

    void OnDestroy()
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            HostUIManager.instance.LeftBtnListEnable(false);
            PlayBackController.instance.StopPlayBack();
        }
        else
        {
            if (UILoadingBar.instance)
                UILoadingBar.instance.HideClientPlayBack();
        }
    }
}
