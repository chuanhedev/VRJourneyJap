using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// 注册的网络事件
/// </summary>
/// <param name="actionName">注册的事件的名称 </param>
/// <param name="parameter">传递的参数,以字符串的形式传输,需要用户自己解析,可以为json,xml或者单纯的字符串</param>
/// <param name="deviceId">调用时间的设备ID,可选</param>
public delegate void ActionEvent( string actionName, string parameter,string deviceId);

/// <summary>
/// VitoSDK 
/// </summary>
public static class VitoPlugin{

    private static Dictionary<string, ActionEvent> actionEvents = new Dictionary<string, ActionEvent>();

    /// <summary>
    /// 当前的设备ID，用作用户的唯一识别码，所以不允许一台设备跑多个此应用
    /// </summary>
    public static string DeviceID { get { return ConnectionClientConfig.DeviceID; } }

    public static int UserId { get { return ConnectionClientConfig.UserId; }
        set { ConnectionClientConfig.UserId = value; }
    }
    public static CtrlType CT {
        get
        {
            return ConnectionClientConfig.CT;
        }
        set
        {
            ConnectionClientConfig.CT = value;
        }
    }
    public static CtrlMode CM
    {
        get
        {
            return ConnectionClientConfig.CM;
        }
        set
        {
            ConnectionClientConfig.CM = value;
        }
    }
    public static AdminStatus AS = AdminStatus.Wait;
    public static bool isMaster {
        get
        {
            return ConnectionClientConfig.isMaster;
        }
        set
        {
            ConnectionClientConfig.isMaster = value;
        }
    }


    /// <summary>
    /// 是否网络模式，可以通过这个判断是否可调用网络功能相关接口
    /// </summary>
    public static bool IsNetMode
    {
        get
        {
            return ActionController.instance != null;
        }
    }


    public static bool  ClientHasPermission{
        get{
            return !(IsNetMode && ((!VitoPlugin.mGameIsPlaying) || (CT == CtrlType.Player && CM == CtrlMode.ControleMode) || (CT == CtrlType.Admin && ((CM == CtrlMode.FreeMode && ActionController.instance.CurLookedUnitInfo != null)))));
        }
    }

    public static Action<bool> GameIsPlayingListener;
    private static bool _gameIsPlaying = false;
    public static bool mGameIsPlaying
    {
        get
        {
            return _gameIsPlaying;
        }
        set
        {
            if (_gameIsPlaying != value)
            {
                _gameIsPlaying = value;
                if(GameIsPlayingListener!=null)
                {
                    GameIsPlayingListener(_gameIsPlaying);
                }
                if (_gameIsPlaying)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }
            }
        }
    }
   


    public static void RegisterActionEvent(string actionName, ActionEvent actionEvent)
    {
        if (actionEvents.ContainsKey(actionName))
        {                                                       
            actionEvents[actionName] = actionEvent;
        }
        else
        {
            actionEvents.Add(actionName, actionEvent);
        }
    }
    public static void UnRegisterActionEvent(string actionName)
    {
        if(actionEvents.ContainsKey(actionName))
        {
            actionEvents.Remove(actionName);
        }
    }

    /// <summary>
    /// 向服务器发送请求事件
    /// </summary>
    /// <param name="actionName"> 事件名称</param>
    /// <param name="parameter">参数</param>
    /// <param name="isCache">是否缓存，下一次进入系统时会自动执行最后一次缓存的事件</param>
    public static void RequestActionEvent(string actionName,string parameter="",bool isCache=false)
    {
        LitJson.JsonData jd = new LitJson.JsonData();
        jd["type"] = actionName;
        jd["content"] = parameter;
        jd["isCache"] = isCache;
        jd["deviceid"] = VitoPlugin.DeviceID;
        string json = LitJson.JsonMapper.ToJson(jd);
        LogicClient.instance.ExecuteAction(new ActionBrodcastMessageRequest(json), (err, response) => {
            if (response == null)
            {
                Debug.Log("resonse is null with error:" + err);
            }
            else if (!string.IsNullOrEmpty(response.error))
            {
                Debug.Log(response.error);
            }
        });
    }

    /// <summary>
    /// 接收到并执行注册的事件
    /// </summary>
    /// <param name="actionName">事件名称 </param>
    /// <param name="parameter">参数 </param>
    /// <param name="deviceId">设备唯一ID </param>
    /// <returns> 事件不存在返回 false </returns>
    public static bool ReceiveActionEvent(string actionName,string parameter,string deviceId)
    {
        if(actionEvents.ContainsKey(actionName))
        {
            actionEvents[actionName](actionName,parameter,deviceId);
            return true;
        }else
        {
            return false;
        }
    }
}

//公共约定 全局变量 
public class Global
{
    //平台打包宏定义
#if GEARVR
    public static EVrType eVrType = EVrType.EVT_GearVR;
    
#elif HTCVIVE
    //Do Somthing
    public static EVrType eVrType = EVrType.EVT_HTC_Vive;
#elif OCULUSRIFT
    //Do Somthing
    public static EVrType eVrType = EVrType.EVT_Oculus_Rift;
#else
    public static EVrType eVrType = EVrType.EVT_NONE;
#endif
}
