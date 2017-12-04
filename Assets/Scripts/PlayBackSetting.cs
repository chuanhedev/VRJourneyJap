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
            MyController myController = head.GetComponent<MyController>();
            if (cameraController) cameraController.enabled = false;
            if (myController) myController.enabled = false;
        }
        else
        {
            if (UILoadingBar.instance)
                UILoadingBar.instance.ShowClientPlayBack();

            GameObject pano_Box = GameObject.Find("Pano_Box");
            pano_Box.SetActive(false);

            MicController.instance.enabled = false;
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

            MicController.instance.enabled = true;
        }
    }
}
