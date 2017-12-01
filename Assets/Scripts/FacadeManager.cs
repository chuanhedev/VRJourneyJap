using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Mode
{
    Controller,
    Leap
}

public enum VitoMode
{
    Free,
    Ctrl
}

public class FacadeManager : MonoBehaviour
{
    public static FacadeManager _instance;
    [HideInInspector] public Mode mode = Mode.Controller;//手柄和leap模式
    [HideInInspector] public VitoMode vitoMode = VitoMode.Free;//自由和管理模式
    //[SerializeField] GameObject menu;
    public Material[] panoMats;
    RefreshPanoManager refreshPanoManager;
    MyUIManager myUIManager;
    UpdatePanoTextures updatePanoTextures;
    UpdatePanoBlack updatePanoBlack;
    UpdatePanoLabel updatePanoLabel;
    Action<string> LoadCallback;
    public static Action<string, bool> Act_UpdatePano;
    [SerializeField] GameObject panoBlack;
    GameObject panoBlackGo;
    [SerializeField] GameObject pano_Label;
    GameObject pano_LabelGo;
    GameObject leap;
    GameObject japEatery;

    void Awake()
    {
        _instance = this;
        InitManagers();
        LoadCallback += OnLoadCallback;

        RegisterUpdatePano();

        //UserInfoData userInfoData = UserInfoManager.instance.getHistoryUserData("2c4552f8145bc7909196bdb433ad0ac7");

        //Debug.Log(userInfoData);
    }

    void OnEnable()
    {
        Act_UpdatePano += UpdatePano;
    }
    void OnDisable()
    {
        Act_UpdatePano -= UpdatePano;
    }

    //void Update()
    //{
    //   // Debug.Log(vitoMode);
    //    //Debug.Log(Enum.GetName(typeof(CtrlMode), VitoPlugin.CM));
    //    //if (Input.GetKeyDown(KeyCode.A))
    //    //{

    //    //    SwitchPicoHome(false);
    //    //}

    //    //if (Input.GetKeyDown(KeyCode.S))
    //    //{
    //    //    RequestUpdatePano("Osaka/Loft");
    //    //}
    //}

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
    ///切换抓取模式
    /// </summary>
    /// <param name="isInitMode">是否重置到手柄模式</param>
    public void SwitchPicoGrabMode(bool isInitMode)
    {
        //myUIManager.CheckPicoMenu(menu);

        if (VitoSDKConfig.instance.ctrlType == CtrlType.Admin) return;

        if (SceneManager.GetActiveScene().name == "JapEatery")
        {
            //切换leap
            if (!leap)
            {
                GameObject mainCameraGo = GameObject.FindGameObjectWithTag("MainCamera");
                if (mainCameraGo)
                {
                    leap = mainCameraGo.transform.Find("LMHeadMountedRig").gameObject;
                }
                else
                {
                    leap = GameObject.FindGameObjectWithTag("LeapMotion");
                }
            }

            if (!japEatery)
                japEatery = GameObject.FindGameObjectWithTag("JapEatery");

            if (leap.activeSelf || isInitMode)//手柄模式
            {
                mode = Mode.Controller;
                leap.SetActive(false);
                japEatery.transform.position = new Vector3(0.2f, 1.65f, -3.11f);
            }
            else//leap模式
            {
                mode = Mode.Leap;
                leap.SetActive(true);
                japEatery.transform.position = new Vector3(0.2f, 2.26f, -4f);
            }
            GrabObjectManager._instance.ResetAllPosAndRos();
        }
    }

    public void LoadPano(string panoPath)
    {
        updatePanoTextures.UpdatePano(panoPath);
    }

    public void UpdateLabel(string panoPath)
    {
        if (!pano_LabelGo)
        {
            pano_LabelGo = Instantiate(pano_Label, Vector3.zero, Quaternion.identity);
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
    public void UpdatePano(string panoPath, bool isAwake)
    {
        if (isAwake)
        {
            updatePanoBlack.UpdatePano(panoPath);
        }
        else
        {
            if (panoBlackGo) return;
            panoBlackGo = Instantiate(panoBlack);
            LivePano_SceneTransition livePano_SceneTransition = panoBlackGo.GetComponent<LivePano_SceneTransition>();
            updatePanoBlack.UpdatePano(livePano_SceneTransition, panoPath);
        }
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
            Act_UpdatePano(msg, false);
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
