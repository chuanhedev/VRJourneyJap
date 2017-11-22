using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacadeManager : MonoBehaviour
{
    public static FacadeManager _instance;
    [SerializeField] GameObject menu;
    public Material[] panoMats;
    RefreshPanoManager refreshPanoManager;
    MyUIManager myUIManager;
    UpdatePanoTextures updatePanoTextures;
    UpdatePanoBlack updatePanoBlack;
    UpdatePanoLabel updatePanoLabel;
    Action<string> LoadCallback;
    public static Action<string> Act_UpdatePano;
    [SerializeField] GameObject panoBlack;
    GameObject panoBlackGo;
    [SerializeField] GameObject pano_Label;
    GameObject pano_LabelGo;

    void Awake()
    {
        _instance = this;
        InitManagers();
        LoadCallback += OnLoadCallback;

        RegisterUpdatePano();

    }

    void OnEnable()
    {
        Act_UpdatePano += UpdatePano;
    }
    void OnDisable()
    {
        Act_UpdatePano -= UpdatePano;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            RequestUpdatePano("Osaka/Park");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            RequestUpdatePano("Osaka/Loft");
        }
    }

    public T AddOrDestoryComponment<T>(GameObject go, bool isAdd) where T : Component
    {
        T t = go.GetComponent<T>();
        if (isAdd)
        {
            if (t == null) t = go.AddComponent<T>();
        }
        else
        {
            if (t != null) Destroy(t);
        }
        return t;
    }

    /// <summary>
    /// 获取刷新数据,暂时无用
    /// </summary>
    /// <returns></returns>
    public void GetRefreshPanoData()
    {
        refreshPanoManager.GetRefreshPanoData(LoadCallback);
    }

    void OnLoadCallback(string result)
    {
        Debug.Log(result);
        MyPicoLog.SetLog1(result);
    }

    /// <summary>
    ///点击pico手柄home键时调用
    /// </summary>
    /// <param name="isShow"></param>
    public void SwitchPicoMenu()
    {
        //myUIManager.CheckPicoMenu(menu);
    }

    public void LoadPano(string panoPath)
    {
        updatePanoTextures.UpdatePano(panoPath);
    }

    public void UpdateLabel(string panoPath)
    {
        if (!pano_LabelGo)
        {
            pano_LabelGo = Instantiate(pano_Label);
        }

        updatePanoLabel.UpdateLabel(pano_LabelGo, panoPath);
    }

    /// <summary>
    /// 执行同步切换pano
    /// </summary>
    /// <param name="panoPath"></param>
    public void RequestUpdatePano(string panoPath)
    {
        Rpc_UpdatePano(panoPath);
    }

    #region 切换pano
    public void UpdatePano(string panoPath)
    {
        if (panoBlackGo) return;
        panoBlackGo = Instantiate(panoBlack);
        LivePano_SceneTransition livePano_SceneTransition = panoBlackGo.GetComponent<LivePano_SceneTransition>();
        updatePanoBlack.UpdatePano(livePano_SceneTransition, panoPath);
    }


    void Rpc_UpdatePano(string msg)
    {
        if (!VitoPlugin.IsNetMode)
        {
            Cmd_UpdatePano(msg);
        }
        else
        {
            VitoPlugin.RequestActionEvent("UpdatePano", msg);
        }
    }

    void Cmd_UpdatePano(string msg)
    {
        if (Act_UpdatePano != null)
        {
            Act_UpdatePano(msg);
        }
    }

    void RegisterUpdatePano()
    {
        VitoPlugin.RegisterActionEvent("UpdatePano", (string actionName, string parameter, string deviceid) =>
        {
            Cmd_UpdatePano(parameter);
        });
    }
    #endregion

    void InitManagers()
    {
        refreshPanoManager = new RefreshPanoManager(this);
        myUIManager = new MyUIManager();
        updatePanoTextures = new UpdatePanoTextures(this);
        updatePanoBlack = new UpdatePanoBlack(this);
        updatePanoLabel = new UpdatePanoLabel();
    }
}
