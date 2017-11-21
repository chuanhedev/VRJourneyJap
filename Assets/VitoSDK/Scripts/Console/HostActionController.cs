using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Text;
using System;
/// <summary>
/// 控制台功能管理
/// </summary>
public class HostActionController : MonoBehaviour {
    private ActionController mActCtrl;
    public static HostActionController instance { get; set; }

    void Awake()
    {
        instance = this;
    }

    
    public void RegisterBroadcastMsg(ActionController actCtrl)
    {
        mActCtrl = actCtrl;
        VitoPlugin.RegisterActionEvent("JoinPlayer", (string type, string content, string device) => {
                OnResponseJoinPlayer(int.Parse(content), device);
        });
        VitoPlugin.RegisterActionEvent("LeavePlayer", (string type, string content, string device) => {
            if (VitoPlugin.CT == CtrlType.Admin)
            {
                OnResponseLeavePlayer(int.Parse(content));
            }
        });
        VitoPlugin.RegisterActionEvent("LookPlayer", (string type, string content, string deviceid) => {
            OnResponseLookPlayerMsg(content);
        });
        VitoPlugin.RegisterActionEvent("GiveUpLookPlayer", (string type, string content, string deviceid) => {
            OnResponseGiveupLookPlayerMsg(content);
        });
        VitoPlugin.RegisterActionEvent("FreeModel", (string type, string content, string deviceid) => {
            OnResponseFreeModeMsg();
        });
        VitoPlugin.RegisterActionEvent("AdminModel", (string type, string content, string deviceid) => {
            OnResponseAdminModeMsg(content);
        });
        VitoPlugin.RegisterActionEvent("Stop", ( string type, string content, string deviceid) => {
            OnResonseStopMsg(content);
        });
        VitoPlugin.RegisterActionEvent("Continue", (string type, string content, string deviceid) => {
            OnResponseContinueMsg(content);
        });
    }

    protected void OnResponseContinueMsg(string parameter)
    {
        VitoPlugin.mGameIsPlaying = true;
        VitoPlugin.AS = AdminStatus.Beging;
        mActCtrl.SendMsg_PlayerRunning();
        VitoPluginPlayVideo.instance.SetStatus(true);
    }

    protected void OnResonseStopMsg(string parameter)
    {
        VitoPlugin.mGameIsPlaying = false;
        VitoPluginPlayVideo.instance.SetStatus(false);
    }

    public void OnRequestFreeMode()
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            VitoPlugin.RequestActionEvent("FreeModel", "");
        }
    }
    public void OnRequestAdminMode()
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            VitoPlugin.RequestActionEvent("AdminModel", VitoPlugin.UserId.ToString());
        }
    }
    public void OnRequestStopMsg()
    {
        if(VitoPlugin.CT==CtrlType.Admin)
        {
            VitoPlugin.RequestActionEvent("Stop","");
        }
    }
    public void OnRequestContinueMsg()
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            VitoPlugin.RequestActionEvent("Continue", "");
        }
    }

    public void SendVoiceOpen()
    {
        VitoPluginPlayVideo.instance.SetStatus(false);
        if (AudioSync.instance != null)
        {
            AudioSync.instance.Record();
        }
    }
    public void SendVoiceClose()
    {
        VitoPluginPlayVideo.instance.SetStatus(true);
        if (AudioSync.instance != null)
        {
            AudioSync.instance.StopRecord();
        }
    }


    public void OnRequestVirtualBodyVisiable(bool isOn)
    {
        VitoPlugin.RequestActionEvent("HideNodePlayer",isOn.ToString());
    }


    /// <summary>
    /// 所有客户端加载完场景后，初始化控制模式
    /// </summary>
    public void OnInitSceneModeAction()
    {
        StartCoroutine(IInitSceneModeAction());
    }

    IEnumerator IInitSceneModeAction()
    {
        float timer = 0;
        float sumTime = 1;
        while(timer<sumTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
               
        //yield return new WaitForSecondsRealtime(1);
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            if (VitoPlugin.CM == CtrlMode.ControleMode)
            {
                OnRequestAdminMode();
            }
            else
            {
                OnRequestFreeMode();
            }
        }
    }


    void OnResponseJoinPlayer(int userid, string device)
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            if (userid == VitoPlugin.UserId)
                return;
            UserInfoManager.instance.AddUser(userid, device);
        }
        
    }


    public void OnRequestGetAllUserId()
    {
        LogicClient.instance.ExecuteAction (new GetAllUserIdRequest(),
        delegate (string err, Response response)
        {
            if (!string.IsNullOrEmpty(response.error))
            {

            }
            else
            {
#if !UNITY_ANDROID
                DebugHealper.Log("get all userid success with data: "+response.data);                
#endif
                JsonData jd = JsonMapper.ToObject(response.data);
                for (int i = 0; i < jd.Count; i++)
                {
                    JsonData tempjd = jd[i];
                    int userid = (int)tempjd["userid"];
                    string deviceid = tempjd["deviceid"].ToString();
                    OnResponseJoinPlayer(userid, deviceid);
                }
            }
        });
    }

    void OnResponseLeavePlayer(int userid)
    {
        UserInfoManager.instance.OnUserLeave(userid);
    }

    public void OnResponseLookPlayerMsg(string content)
    {
        int userid = int.Parse(content);
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            if (userid < 0 || userid == VitoPlugin.UserId || ConnectionClient.instance == null || ConnectionClient.instance.owner == null)
                return;

            if (VitoPlugin.CM == CtrlMode.FreeMode)
            {
                VitoPlugin.isMaster = false;
            }

            mActCtrl.ClearLookedUnitInfo();

            UnitInfo myUnitInfo = ConnectionClient.instance.owner;
            UnitInfo targetUnitInfo = ConnectionClient.instance.GameClient.GetUnit((uint)userid);
            mActCtrl.CurLookedUnitInfo = targetUnitInfo;

            if (myUnitInfo != null && targetUnitInfo != null)
            {
                mActCtrl.SetSyncUnitInfo(targetUnitInfo, myUnitInfo);
                if(targetUnitInfo.curPlayerRay!=null)
                {
                    targetUnitInfo.curPlayerRay.HideTouxiang(true);
                }
                
            }
        }
        else if (VitoPlugin.CT == CtrlType.Player)
        {
            if (userid == VitoPlugin.UserId)
            {
                VitoPlugin.isMaster = true;
            }
            else
            {
                VitoPlugin.isMaster = false;
            }
        }
    }

    void OnResponseGiveupLookPlayerMsg(string content)
    {
        int userid = int.Parse(content);        
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            mActCtrl.ClearLookedUnitInfo();
        }
        else
        {
            VitoPlugin.isMaster = false;
        }
    }

    void OnResponseFreeModeMsg()
    {
        VitoPlugin.CM = CtrlMode.FreeMode;
        VitoPlugin.isMaster = false;
        if (VitoPlugin.CT == CtrlType.Admin)
        {
           // mActCtrl.RequestLookUser();
        }
        else
        {
            if (mActCtrl.cacheAdminUnitInfo != null)
            {
                mActCtrl.cacheAdminUnitInfo.SyncUnitInfoPos = null;
                mActCtrl.cacheAdminUnitInfo = null;
            }
        }
    }
    void OnResponseAdminModeMsg(string content)
    {
        VitoPlugin.CM = CtrlMode.ControleMode;
        if (VitoPlugin.CT == CtrlType.Admin)
            VitoPlugin.isMaster = true;
        else
        {
            VitoPlugin.isMaster = false;
            if (ConnectionClient.instance == null || ConnectionClient.instance.owner == null || ConnectionClient.instance.GameClient == null)
                return;
            int userid = int.Parse(content);
            UnitInfo myUnitInfo = ConnectionClient.instance.owner;
            UnitInfo targetUnitInfo = ConnectionClient.instance.GameClient.GetUnit((uint)userid);
            mActCtrl.cacheAdminUnitInfo = targetUnitInfo;
            if (myUnitInfo != null && targetUnitInfo != null)
            {
                targetUnitInfo.SyncUnitInfoPos = myUnitInfo;
            }
        }
    }


    
}
