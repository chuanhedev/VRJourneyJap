using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using LitJson;
public class VitoPluginLoadScene : MonoBehaviour {
    public static VitoPluginLoadScene instance { get; set; }

    public static Action<bool> showLoadingBarAction;
    public static Action<float> updateLoadingBarAction;

    //public UILoadingBar mLoadingBar;
    private string loadingScene;
    private AsyncOperation asyncOperation;
    private Action<bool> mLoadSceneCallback;
    private float curProgress = 0;

    private ActionController mActCtrl { get { return ActionController.instance; } }
    public bool LoadCompleted { get; set; }
    public string mIsloadingSceneName { get; set; }
    public string cacheSceneName { get; set; }

    private bool isLoadingScene = false;
    public  static Action<string> OnLoadSceneListener;

    private string willLoadsceneName = ""; //当前有场景正在加载时，下一个要加载测场景名称

    void Awake()
    {
        instance = this;
        VitoPlugin.RegisterActionEvent("LoadScene", 
            (string type, string content, string deviceid) => {OnResponseChangeScene(content);});
    }
    private void Start()
    {
    }

    public void OnRequestChangeScene(string sceneName)
    {
        VitoPlugin.RequestActionEvent("LoadScene",sceneName,true);
    }

    public void OnResponseChangeScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
            return;
        if(OnLoadSceneListener!=null)
        {
            OnLoadSceneListener(sceneName);
        }        
        if(isLoadingScene)
        {
            willLoadsceneName = sceneName;
        }else
        {
            RealChangeScene(sceneName,ChangeSceneCallback);
        }
    }

    void Update()
    {
        if(isLoadingScene&&asyncOperation!=null)
        {
            if (!asyncOperation.isDone)
            {
                if (curProgress < 0.9f)
                {
                    curProgress = asyncOperation.progress;
                    float showProgress = curProgress;
                    if (!string.IsNullOrEmpty(willLoadsceneName))
                    {
                        showProgress = curProgress / 2;
                    }
                    if(updateLoadingBarAction!=null)
                        updateLoadingBarAction(showProgress);
                    //mLoadingBar.UpdateProgress(showProgress);
                }
                else
                {
                    curProgress += Time.unscaledDeltaTime;
                    float showProgress = curProgress;
                    if (!string.IsNullOrEmpty(willLoadsceneName))
                    {
                       showProgress = curProgress / 2;
                    }
                    if(updateLoadingBarAction!=null)
                        updateLoadingBarAction(showProgress);
                    //mLoadingBar.UpdateProgress(showProgress);
                }
                if (curProgress >= 1)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }
            if (asyncOperation.isDone)
            {
                asyncOperation.allowSceneActivation = true;
                {
                    if(showLoadingBarAction!=null)
                        showLoadingBarAction(false);
                    //mLoadingBar.Hide();
                }
                isLoadingScene = false;
                if (mLoadSceneCallback != null)
                {
                    mLoadSceneCallback(true);
                }
            }
        }        
    }

    public void RealChangeScene(string sceneName,Action<bool> callback=null)
    {
        VitoPlugin.AS = AdminStatus.Wait;
        LoadCompleted = false;
        try
        {
            if (VitoPlugin.CT == CtrlType.Player)
            {
                VitoPlugin.RequestActionEvent("PlayerSceneLoading", VitoPlugin.UserId.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        mIsloadingSceneName = sceneName;
        mLoadSceneCallback = callback;
        loadingScene = sceneName;
        curProgress = 0;
        if (showLoadingBarAction != null)
            showLoadingBarAction(true);
        //mLoadingBar.Show();
        isLoadingScene = true;
        asyncOperation = SceneManager.LoadSceneAsync(loadingScene);
        asyncOperation.allowSceneActivation = false;
        //StartCoroutine(iloadScene=ILoadScene());
        
    }
    private void ChangeSceneCallback(bool isSuccess)
    {
        if (isSuccess)
        {
            DebugHealper.Log("场景加载成功：" + mIsloadingSceneName);
            LoadCompleted = true;
            {
                VitoPlugin.mGameIsPlaying = false;
                VitoPlugin.RequestActionEvent("SceneLoadComplete",VitoPlugin.UserId.ToString());
            }
        }
        else
        {
            DebugHealper.Log("加载场景失败：" + mIsloadingSceneName);
        }
        mIsloadingSceneName = "";
        if(!string.IsNullOrEmpty(willLoadsceneName))
        {
            string sceneName = willLoadsceneName;
            willLoadsceneName = "";
            RealChangeScene(sceneName,ChangeSceneCallback);
        }
    }

}
