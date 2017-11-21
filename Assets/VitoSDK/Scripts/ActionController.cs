using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.VR;

/// <summary>
/// 管理者当前的状态
/// </summary>
public enum AdminStatus
{
    /// <summary>
    /// 控制台在初始场景或者正在加载场景时为此状态
    /// </summary>
    Wait = 0,
    Beging
}
[RequireComponent( typeof(VitoPluginLoadScene))]
[RequireComponent(typeof(UserInfoManager))]
public class ActionController : MonoBehaviour
{
    public static ActionController instance { get;  private set; }

    public JsonData ActionData;
    public bool virtualBodyVisiable {
        get
        {
            return ConnectionClientConfig.virtualBodyVisiable;
        }
        set
        {
            ConnectionClientConfig.virtualBodyVisiable = value;
        }
    }
    [HideInInspector]
    public UnitInfo cacheAdminUnitInfo;
    public HostActionController mConsoleManager;
    public VitoPluginLoadScene mSceneChangeAction;
    public UserInfoManager mUserInfoManager;
    void Awake()
    {
        ConnectionClientConfig.IsNetMode = true;
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        mConsoleManager.RegisterBroadcastMsg(this);
        mUserInfoManager.RegisterBroadcastMsg(this);

        VitoPlugin.RegisterActionEvent("HideNodePlayer", (string type, string content, string deviceid) => {
            //if (VitoPlugin.CT == CtrlType.Admin)
            if(bool.Parse(content))
            {
                virtualBodyVisiable = true;
            }else
            {
                virtualBodyVisiable = false;
            }
                //virtualBodyVisiable = !virtualBodyVisiable;
        });

        VitoPlugin.RegisterActionEvent("Stop", (actionName, parameter, deviceId) =>
        {
            VitoPlugin.mGameIsPlaying = false;
            VitoPluginPlayVideo.instance.SetStatus(false);
        });
        VitoPlugin.RegisterActionEvent("Continue", (actionName, parameter, deviceId) =>
        {
            VitoPlugin.mGameIsPlaying = true;
            VitoPlugin.AS = AdminStatus.Beging;
            ActionController.instance.SendMsg_PlayerRunning();
            VitoPluginPlayVideo.instance.SetStatus(true);
        });

        VitoPlugin.RegisterActionEvent("AskQuestion", (actionName, parameter, deviceId) =>
        {
            if (VitoPlugin.CT == CtrlType.Player)
            {
                JsonData questionjd = JsonMapper.ToObject(parameter);
                VitoPluginQuestionManager.instance.AddQuestions(questionjd);
            }
        });
        VitoPlugin.RegisterActionEvent("SceneLoadAllComplete", (actionName, parameter, deviceId) =>
        {
            VitoPlugin.AS = AdminStatus.Beging;
            VitoPlugin.mGameIsPlaying = true;
            ActionController.instance.SendMsg_PlayerRunning();
            ActionController.instance.mConsoleManager.OnInitSceneModeAction();
        });
        VitoPlugin.RegisterActionEvent("SceneLoadComplete", (actionName, parameter, deviceId) =>
        {
            if (VitoPlugin.CT == CtrlType.Admin)
            {
                UserInfoManager.instance.ChangeUserStatus(parameter, UserPosStatus.SceneLoadedOver);
                if (UserInfoManager.instance.IsAllCompleted || VitoPlugin.AS == AdminStatus.Beging)
                {
                    VitoPlugin.RequestActionEvent("SceneLoadAllComplete", "");
                }
            }
        });
        LogicClient.instance.LoginToLogicServer();
    }
    public void SendMsg_PlayerRunning()
    {
        VitoPlugin.RequestActionEvent("PlayerRuning",VitoPlugin.UserId.ToString());
    }

    /// <summary>
    /// 首次启动时获取服务器广播的最后一条消息
    /// </summary>
    public void RequestLastBrocastMsg()
    {
        LogicClient.instance.ExecuteAction(new GetLastActionMsgRequest(),
        delegate (string err, Response response)
        {
            if (!string.IsNullOrEmpty(response.error))
            {
                DebugHealper.Log(response.error);                
            }
            else
            {
                ExecuteAction(response.data);
            }
        });
    }
    
    /// <summary>
    /// 当前观察的用户对象
    /// </summary>
    [HideInInspector]
    public UnitInfo CurLookedUnitInfo {
        get {
            return ConnectionClientConfig.CurLookedUnitInfo;
        }
        set
        {
            ConnectionClientConfig.CurLookedUnitInfo = value;
            if(HostUIManager.instance!=null)
            {
                HostUIManager.instance.SetLookedInfo(ConnectionClientConfig.CurLookedUnitInfo);
            }
        }
    }

    /// <summary>
    /// 设置同步终端的信息到控制台
    /// </summary>
    /// <param name="targetUnitInfo"></param>
    /// <param name="syncUnitInfo"></param>
    public void SetSyncUnitInfo(UnitInfo targetUnitInfo,UnitInfo syncUnitInfo)
    {
        targetUnitInfo.SyncUnitInfo = syncUnitInfo;
        targetUnitInfo.m_OpenHead = false;
        
    }

    public void ClearLookedUnitInfo()
    {
        if (CurLookedUnitInfo != null)
        {
            CurLookedUnitInfo.m_OpenHead = true;
            if(CurLookedUnitInfo.curPlayerRay!=null)
            {
                CurLookedUnitInfo.curPlayerRay.HideTouxiang(false);
            }            
            CurLookedUnitInfo.SyncUnitInfo = null;
            CurLookedUnitInfo = null;
        }
    }

    void ExecuteAction(string data)
    {
        if (string.IsNullOrEmpty(data))
            return;
        JsonData jd = JsonMapper.ToObject(data);
        string type = jd["type"].ToString();
        string content = jd["content"].ToString();
        string deviceid = jd["deviceid"].ToString();

        if (!VitoPlugin.ReceiveActionEvent(type, content, deviceid))
        {
            Debug.LogError("received unregisted action: " + type);
        }
        return;
    }

    void OnGetBrodcastMsg(string err, Response response)
    {
        if (response == null || string.IsNullOrEmpty(response.data))
            return;
#if !UNITY_ANDROID
        DebugHealper.Log("getBroadCastMsg:"+response.data);
#endif
        ExecuteAction(response.data);
    }

    public void JoinBrodcastMesg()
    {
        LogicClient.instance.ExecuteAction(new JoinLogicServiceRequest(), delegate (string err, Response response)
        {
            JoinComplete();
        });
        if (LogicClient.instance != null && LogicClient.instance.WSClient != null)
            LogicClient.instance.WSClient.RegisterHandler((int)Client.ESystemRequest.GetBrodcastMesg, OnGetBrodcastMsg);
    }

    void JoinComplete()
    {
        mUserInfoManager.HMDMounted();
    }
    
    void OnDestroy()
    {
        if (LogicClient.instance != null && LogicClient.instance.WSClient != null)
            LogicClient.instance.WSClient.UnRegisterHandler((int)Client.ESystemRequest.GetBrodcastMesg, OnGetBrodcastMsg);
    }
}
