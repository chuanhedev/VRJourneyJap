using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILoadingBar : MonoBehaviour {
    public GameObject barObject;
    public Text mProgress;
    public Image mProgressBar;
    public Slider mSlider;
    public GameObject ClientObj;
    public static UILoadingBar instance;
    void Awake()
    {
        VitoPluginLoadScene.showLoadingBarAction += ShowBar;
        VitoPluginLoadScene.updateLoadingBarAction += UpdateProgress;
    }
    void ShowBar(bool show)
    {
        if(show)
        {
            Show(0);
        }else
        {
            Hide();
        }
    }
    public void Hide()
    {
        if(barObject.activeSelf)
        {
            barObject.SetActive(false);
            List<Camera> cameras = new List<Camera>();
            if (VRSwitchCameraRig.instance != null)
            {
                if (VRSwitchCameraRig.instance.mHead != null && VRSwitchCameraRig.instance.mHead.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mHead.GetComponent<Camera>());
                }
                if (VRSwitchCameraRig.instance.mLeftEye != null && VRSwitchCameraRig.instance.mLeftEye.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mLeftEye.GetComponent<Camera>());
                }
                if (VRSwitchCameraRig.instance.mRightEye != null && VRSwitchCameraRig.instance.mRightEye.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mRightEye.GetComponent<Camera>());
                }
            }
            for (int i = 0; i < cameras.Count; i++)
            {
                cameras[i].cullingMask = System.Int32.MaxValue;//   ~LayerMask.GetMask("3DMobileUI");
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "hongxibaodemaoxian")
                {
                    cameras[i].clearFlags = CameraClearFlags.SolidColor;
                    cameras[i].backgroundColor = new Color(255.0f / 255, 72.0f / 255, 41.0f / 255, 1);
                }
                else
                {
                    cameras[i].clearFlags = CameraClearFlags.Skybox;
                    cameras[i].backgroundColor = Color.black;
                }
            }
        }        
    }

    public void Show(float initValue=0)
    {
        if(!barObject.activeSelf)
        {
            if (HostUIManager.instance != null && HostUIManager.instance.uiBG != null)
            {
                HostUIManager.instance.uiBG.SetActive(false);
            }
            List<Camera> cameras = new List<Camera>();
            if (VRSwitchCameraRig.instance != null)
            {
                if (VRSwitchCameraRig.instance.mHead != null && VRSwitchCameraRig.instance.mHead.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mHead.GetComponent<Camera>());
                }
                if (VRSwitchCameraRig.instance.mLeftEye != null && VRSwitchCameraRig.instance.mLeftEye.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mLeftEye.GetComponent<Camera>());
                }
                if (VRSwitchCameraRig.instance.mRightEye != null && VRSwitchCameraRig.instance.mRightEye.GetComponent<Camera>())
                {
                    cameras.Add(VRSwitchCameraRig.instance.mRightEye.GetComponent<Camera>());
                }
            }
            for (int i = 0; i < cameras.Count; i++)
            {
                cameras[i].cullingMask = LayerMask.GetMask("3DMobileUI");
                cameras[i].clearFlags = CameraClearFlags.SolidColor;
                cameras[i].backgroundColor = Color.black;
            }
            if (ClientObj != null)
            {
                ClientObj.SetActive(false);
            }
            if (UI3DFollowCamera.instance != null)
            {
                UI3DFollowCamera.instance.ResetPos();
            }
            barObject.SetActive(true);
            UpdateProgress(initValue);
        }
    }
    public void UpdateProgress(float value)
    {
        value = Mathf.Clamp01(value);
        mProgress.text = Mathf.FloorToInt(value * 100).ToString()+"%";
        mProgressBar.fillAmount = value;
        mSlider.value = value;
    }


}
