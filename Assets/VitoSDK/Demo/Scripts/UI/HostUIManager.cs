using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System.Text;
using com.vito.plugin.demo;
public class HostUIManager : MonoBehaviour {

    public GameObject mHostUIRoot;

    public GameObject mHostUISceneItem;
    public Transform mHostUISceneItemParent;
    public HostUIUserListPanel userListPanel;
    public HostUISceneOptions sceneOptions;

    public Text txtTitle;
    public Text txtVoiceToggle;
    public Text txtVisiableToggle;

    public HostToggleWithContent mSceneListToggle;
    public HostToggleWithContent mVideoListToggle;
    public HostToggle mPauseToggle;
    public HostToggle mStartToggle;
    public Transform mHostUISceneItemVideoParent;
    public GameObject uiRoot;
    public GameObject uiBG;
    public Text txtLookedInfo;
    private Dictionary<int, DataSceneConfig> mSceneConfigMap = new Dictionary<int, DataSceneConfig>();
    private string SceneConfigFile = "DataSceneConfig.txt";
    void OnEnable()
    {
        VitoPlugin.GameIsPlayingListener += OnResponsePause;
        VitoPluginLoadScene.OnLoadSceneListener += OnResponseLoadScene;
    }
    void OnDisable()
    {
        VitoPlugin.GameIsPlayingListener -= OnResponsePause;
        VitoPluginLoadScene.OnLoadSceneListener -= OnResponseLoadScene;
    }

    void OnResponseLoadScene(string sceneName)
    {
        VitoSceneConfigData sceneConfig = getSceneConfigDataWithName(sceneName);
        if (sceneConfig != null)
        {
            SetTitle(sceneConfig.title);
        }
        else
        {
            SetTitle("360视频");
        }
        OnResponseChangeScene();
    }
    void ShowSceneList()
    {
        if (VitoSDKConfig.instance.sceneConfigList != null)
        {
            for (int i = 0; i < VitoSDKConfig.instance.sceneConfigList.Count; i++)
            {
                VitoSceneConfigData data = VitoSDKConfig.instance.sceneConfigList[i];
                GameObject go = Instantiate<GameObject>(mHostUISceneItem);
                go.transform.SetParent(mHostUISceneItemParent);
                Vector3 pos = go.transform.localPosition;
                pos.z = 0;
                go.transform.localPosition = pos;
                go.transform.localScale = Vector3.one;
                go.GetComponent<HostUISceneItem>().Init(data);

                NetItem ni = new NetItem();
                ni.url = data.iconPath;
                if (ni.url.StartsWith("http"))
                {

                }
                else
                {
                    ni.url = "file:///" + ni.url;
                }
                ni.backGo = gameObject;
                ni.targetObj = go.gameObject;
                ni.backFun = "ShowTextureCallBack";
                NetManager.instance.EnqueDownloadItem(ni);
            }
        }
    }


    public VitoSceneConfigData getSceneConfigDataWithName(string name)
    {
        foreach (VitoSceneConfigData var in VitoSDKConfig.instance.sceneConfigList)
        {
            if (var.sceneName == name)
            {
                return var;
            }
        }
        return null;
    }


    public void SetLookedInfo(UnitInfo lookedInfo)
    {
        if(lookedInfo==null)
        {
            txtLookedInfo.text = "";
        }else
        {
            UserInfoData data= UserInfoManager.instance.GetUserInfoWithUserId(lookedInfo.UnitID);
            if(data!=null)
            {
                if(!string.IsNullOrEmpty(data.name))
                {
                    txtLookedInfo.text = "正在观察：" + data.name;
                }
                else if(!string.IsNullOrEmpty(data.number))
                {
                    txtLookedInfo.text = "正在观察：" + data.number;
                }
                else
                {
                    txtLookedInfo.text = "正在观察：" + data.userid;
                }
                
            }else
            {
                txtLookedInfo.text = "正在观察：" + lookedInfo.UnitID;
            }
            
        }
    }

    public void CreatePlayerRay(UnitInfo targetUnitInfo, UnitInfo lookUnitInfo)
    {
        
    }
    public void SetTitle(string name)
    {
        txtTitle.text = "当前场景:"+name;
    }

    public static HostUIManager instance;
    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        while(!VitoSDKConfig.instance.IsConfigLoadOver)
        {
            yield return null;
        }

        if(VitoPlugin.CT==CtrlType.Admin)
        {
            mHostUIRoot.SetActive(true);
        }else
        {
            mHostUIRoot.SetActive(false);
        }
        ShowSceneList();
        ShowVideoList();
    }
    void ShowVideoList()
    {
        if (VitoSDKConfig.instance.videoConfigList!=null)
        {
            for (int i = 0; i < VitoSDKConfig.instance.videoConfigList.Count; i++)
            {
                VitoVideoConfigData data = VitoSDKConfig.instance.videoConfigList[i];
                GameObject go = Instantiate<GameObject>(mHostUISceneItem);
                go.transform.SetParent(mHostUISceneItemVideoParent);
                Vector3 pos = go.transform.localPosition;
                pos.z = 0;
                go.transform.localPosition = pos;
                go.transform.localScale = Vector3.one;
                go.GetComponent<HostUISceneItem>().Init(data);
                
                NetItem ni = new NetItem();
                ni.url = data.iconPath;
                if(ni.url.StartsWith("http"))
                {

                }else
                {
                    ni.url = "file:///" + ni.url;
                }
                ni.backGo = gameObject;
                ni.targetObj = go.gameObject;
                ni.backFun = "ShowTextureCallBack";
                NetManager.instance.EnqueDownloadItem(ni);
            }
        }
    }

    void ShowTextureCallBack(MyWWW www)
    {
        HostUISceneItem ssi = www.netItem.targetObj.GetComponent<HostUISceneItem>();
        ssi.imgIcon.texture = www.tex;
    }

    private void InvokeMethod(string name,object[] parameter=null)
    {

        Type type = typeof(HostUIManager);
        MethodInfo method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (method != null)
        {
            {
                method.Invoke(this, parameter);
            }
        }
        else
        {
            #if !UNITY_ANDROID
            DebugHealper.Log("HostUIManager." + name + " isNotExist");
#endif
        }
    }



    public void OnPlayVideo(string videoName)
    {
        VitoPluginPlayVideo.instance.OnRequestPlayVideo(videoName);
    }
    public void OnResponsePlayVideo()
    {
        mVideoListToggle.SetOffToggle();
        mSceneListToggle.SetOffToggle();
    }
    public void OnResponseChangeScene()
    {
        mSceneListToggle.SetOffToggle();
        mVideoListToggle.SetOffToggle();
    }
    public void OnResponsePause(bool isStart)
    {
        if(isStart)
        {
            mPauseToggle.SetToggleValue(false);
            mStartToggle.SetToggleValue(true);
        }else
        {
            mPauseToggle.SetToggleValue(true);
            mPauseToggle.SetToggleValue(false);
        }
    }
    public void OnUserLogIn(UserInfoData userInfoData)
    {
        userListPanel.OnUserLogin(userInfoData);
    }
    public void OnUserRelogin(UserInfoData userInfoData)
    {
        //userListPanel.OnUserRelogin(userInfoData);
    }
    public void OnUserLogOut(UserInfoData userInfoData)
    {
        userListPanel.OnUserLogout(userInfoData);
    }

    public void OnChangeScene(string sceneName)
    {
        if( Application.CanStreamedLevelBeLoaded(sceneName))
        {
            VitoPluginLoadScene.instance.OnRequestChangeScene(sceneName);
            mHostUISceneItemParent.parent.parent.parent.gameObject.SetActive(false);
        }else
        {
            #if !UNITY_ANDROID
            DebugHealper.Log("the scene : " + sceneName + " is not exist ");
#endif
        }
    }


    public void OnToggleValueChange(string toggleName,bool value)
    {
        InvokeMethod(toggleName,new object[] { value});
    }
    
    public void OnBtnClick(string btnName)
    {
        InvokeMethod(btnName);
    }


    private void FreeModeToggle(bool isOn)
    {
        HostActionController.instance.OnRequestFreeMode();
    }
    private void CtrlModeToggle(bool isOn)
    {
        if(isOn)
        {
            HostActionController.instance.OnRequestAdminMode();
        }
    }
    private void PauseModeToggle(bool isOn)
    {
        if(isOn)
        {
            HostActionController.instance.OnRequestStopMsg();
        }
    }

    private void VirtualBodyVisiableToggle(bool isOn)
    {
        
        if(isOn)
        {
            txtVisiableToggle.text = "形象开启";
        }else
        {
            txtVisiableToggle.text = "形象关闭";
        }
        
        if(isOn!=ActionController.instance.virtualBodyVisiable)
        {
            HostActionController.instance.OnRequestVirtualBodyVisiable(isOn);
        }

    }
    private void StartModeToggle(bool isOn)
    {
        if(isOn)
        {
            HostActionController.instance.OnRequestContinueMsg();
        }
    }
    private void SceneOptionToggle(bool isOn)
    {
        if(isOn)
        {
            sceneOptions.ShowOptions();
        }else
        {
            sceneOptions.HideOptions();
        }
    }

    private void OnGiveUpLookPlayer()
    {

    }

    void Update()
    {        
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if(VitoPlugin.IsNetMode&& VitoPlugin.CT==CtrlType.Admin)
            {
                uiRoot.SetActive(!uiRoot.activeSelf);
            }            
        }
    }

    public void OnLookPlayer(UserInfoData userData)
    {
        UserInfoManager.instance.OnLookUser(userData);
    }

    private void ObserveToggle(bool isOn)
    {
        UserInfoManager.instance.OnGiveUpLookUser();
    }

    private void AudioToggle(bool isOn)
    {
        if(isOn)
        {
            txtVoiceToggle.text = "广播开启";
            HostActionController.instance.SendVoiceOpen();
        }else
        {
            txtVoiceToggle.text="广播关闭";
            HostActionController.instance.SendVoiceClose();
        }
    }

    


}
