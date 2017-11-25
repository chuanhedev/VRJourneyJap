using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Text;
public class UserInfoManager : MonoBehaviour {
    public static UserInfoManager instance { get; set; }

    private static Dictionary<string, UserInfoData> HistoryUserinfoMap = new Dictionary<string, UserInfoData>();

    public System.Action<int,int> OnUserCountChange;

    public void RegisterBroadcastMsg( ActionController actCtrl)
    {
        {
            VitoPlugin.RegisterActionEvent("PlayerSceneLoading", (string type, string content, string deviceid) => {
                ChangeUserStatus(content, UserPosStatus.SceneIsLoading);
            });
            VitoPlugin.RegisterActionEvent("PlayerRuning", (string type, string content, string deviceid) => {
                ChangeUserStatus(content, UserPosStatus.InRuningScene);
            });
            VitoPlugin.RegisterActionEvent("PlayerHeadLeave", (string type, string content, string deviceid) => {
                UserInfoManager.instance.ChangeHMDStatus(content, UserHMDStatus.PutOff);
            });
            VitoPlugin.RegisterActionEvent("PlayerHeadPutOn", (string type, string content, string deviceid) => {
                UserInfoManager.instance.ChangeHMDStatus(content, UserHMDStatus.PutOn);
            });
        }
        
    }

    private void Awake()
    {
        instance = this;
        if(VitoPlugin.IsNetMode&&VitoPlugin.CT==CtrlType.Admin)
        {
            LoadHistoryUserData();
        }
    }

    #region 客户端逻辑

    private void LoadHistoryUserData()
    {
        string historyFileName = "UserInfoData.txt";
        string path = Application.streamingAssetsPath + "/" + historyFileName;
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            byte[] byData = new byte[file.Length];
            file.Read(byData, 0, (int)file.Length);

            string content = Encoding.UTF8.GetString(byData);
            //Debug.Log(content);
            Dictionary<string ,UserInfoData> historyUserInfoData = JsonConvert.DeserializeObject<Dictionary<string, UserInfoData>>(content);
            if(historyUserInfoData!=null)
            {
                HistoryUserinfoMap = historyUserInfoData;
            }
        }
    }
    private void WriteHistoryUserData()
    {
        string historyFileName = "UserInfoData.txt";
        string path = Application.streamingAssetsPath + "/" + historyFileName;
       
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        {
            FileStream file = new FileStream(path,FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            string content=JsonConvert.SerializeObject(HistoryUserinfoMap);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
    }
    public UserInfoData getHistoryUserData(string deviceId)
    {
        if(HistoryUserinfoMap.ContainsKey(deviceId))
        {
            return HistoryUserinfoMap[deviceId];
        }else
        {
            return null;
        }
    }

    public void RefreshUserInfoData(UserInfoData data)
    {
        if(HistoryUserinfoMap.ContainsKey(data.deviceid))
        {
            HistoryUserinfoMap[data.deviceid] = data;
        }else
        {
            HistoryUserinfoMap.Add(data.deviceid,data);
        }
        WriteHistoryUserData();
    }

    public void HMDMounted()
    {
        VitoPlugin.RequestActionEvent("PlayerHeadPutOn",VitoPlugin.UserId.ToString());
    }

    public void HMDUnmounted()
    {
        VitoPlugin.RequestActionEvent("PlayerHeadLeave", VitoPlugin.UserId.ToString());
    }
    #endregion

    #region 控制台逻辑

    private List<UserInfoData> UserInfoList = new List<UserInfoData>();
    /// <summary>
    /// 有新用户进入控制台
    /// </summary>
    public void AddUser(int userId,string deviceId)
    {
        UserInfoData historyData = getHistoryUserData(deviceId);

        if(!UserInfoList.Exists((x)=>(x.deviceid==deviceId)))
        {
            UserInfoData userInfoData = new UserInfoData();
            userInfoData.deviceid = deviceId;
            userInfoData.userid = userId;
            if(historyData!=null)
            {
                userInfoData.name = historyData.name;
                userInfoData.number = historyData.number;
                userInfoData.info = historyData.info;
            }
            UserInfoList.Add(userInfoData);
            HostUIManager.instance.OnUserLogIn(userInfoData);
        }else
        {
            UserInfoData userInfoData = GetUserInfoWithDeviceId(deviceId);
            userInfoData.userid = userId;
            HostUIManager.instance.OnUserRelogin(userInfoData);
        }
        TempRefreshUserStatusCount();

        //同步模式
        VitoModeController._instance.RequestChangeMode(Enum.GetName(typeof(VitoMode), FacadeManager._instance.vitoMode));
    }

    /// <summary>
    /// 刷新当前准备就绪和总用户UI
    /// </summary>
    void TempRefreshUserStatusCount()
    {
        int sumCount = 0;
        int curCount = 0;
        for (int i = 0; i < UserInfoList.Count; i++)
        {
            {
                sumCount++;
                if (UserInfoList[i].mSceneStatus == UserPosStatus.InRuningScene || UserInfoList[i].mSceneStatus == UserPosStatus.SceneLoadedOver)
                {
                    curCount++;
                }
            }
        }
        if (OnUserCountChange != null)
        {
            OnUserCountChange(sumCount, curCount);
        }
    }
    /// <summary>
    /// 用户离开
    /// </summary>
    public void OnUserLeave(int userId)
    {
        UserInfoData data=GetUserInfoWithUserId(userId);
        if(data!=null)
        {
            data.mHMDStatus = UserHMDStatus.PutOff;
            data.mSceneStatus = UserPosStatus.None;
        }
    }

    /// <summary>
    /// 用户信息刷新
    /// </summary>
    public void OnRefreshUserInfo(UserInfoData userinfoData)
    {
        RefreshUserInfoData(userinfoData);
        if(userinfoData.OnRefresh!=null)
        {
            userinfoData.OnRefresh();
        }                
    }

    /// <summary>
    /// 所有用户场景加载完成
    /// </summary>
    public bool IsAllCompleted
    {
        get {
            bool rtn = true;
            for(int i=0;i<UserInfoList.Count;i++)
            {
                 if( UserInfoList[i].mSceneStatus!=UserPosStatus.SceneLoadedOver)
                {
                    rtn = false;
                    break;
                }
            }
            return rtn;
        }
    }

    /// <summary>
    /// 使用用户网络Id或者用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public UserInfoData GetUserInfoWithUserId(int userId)
    {
        UserInfoData userData = UserInfoList.Find((x) => x.userid == userId);
        return userData;
    }
    /// <summary>
    /// 使用用户设备号获取用户信息
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public UserInfoData GetUserInfoWithDeviceId(string deviceId)
    {
        UserInfoData userData=UserInfoList.Find((x) => x.deviceid == deviceId);
        return userData;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceid"> 设备Id</param>
    /// <param name="isStart">true 开始；false：结束 </param>
    /// <param name="levelId">关卡ID开发者自定义 </param>
    /// <param name="levelName">关卡名称，开发者自定义</param>
    public void ChangeUserLevelStatus(string deviceid, bool isStart, int levelId, string levelName)
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            UserInfoData userData = GetUserInfoWithDeviceId(deviceid);
            if (userData != null)
            {
                if (userData.OnLevelStatusChange != null)
                {
                    userData.OnLevelStatusChange(isStart, levelId, levelName);
                }
            }
        }
    }
    /// <summary>
    /// 用户电量信息更新
    /// </summary>
    public void OnUserPowerChange(string deviceid,float power)
    {
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            UserInfoData userData = GetUserInfoWithDeviceId(deviceid);
            if (userData != null)
            {
                if (userData.OnLevelStatusChange != null)
                {
                    userData.onPowerChange(power);
                }
            }
        }
    }

    /// <summary>
    /// 用户状态更新
    /// </summary>
    public void ChangeUserStatus(string userIds,UserPosStatus status)
    {
        int userId = System.Int32.Parse(userIds);
        if(VitoPlugin.CT ==CtrlType.Admin&& VitoPlugin.UserId!= userId)
        {
            
            UserInfoData userData = GetUserInfoWithUserId(userId);
            if(userData!=null)
            {
                userData.mSceneStatus = status;
            }
            else
            {
                DebugHealper.Log("user:"+ userId + " not exist ");
            }
            
        }
        TempRefreshUserStatusCount();
    }
    /// <summary>
    /// 用户头显状态更新
    /// </summary>
    public void ChangeHMDStatus(string userids,UserHMDStatus status)
    {
        //Debug.Log("change hmd status: "+ userids+" status:" +status);
        int userId = System.Int32.Parse(userids);
        if (VitoPlugin.CT == CtrlType.Admin && VitoPlugin.UserId != userId)
        {
            UserInfoData userData = GetUserInfoWithUserId(userId);
            if (userData != null)
            {
                userData.mHMDStatus = status;
            }
            else
            {
                DebugHealper.Log("user:" + userId + " not exist ");
            }
        }
        TempRefreshUserStatusCount();
    }

    public  void OnUserSelected(UserInfoData userInfoData)
    {

    }

    /// <summary>
    /// 观察用户
    /// </summary>
    /// <param name="userInfoData"></param>
    public  void OnLookUser(UserInfoData userInfoData)
    {
        if (VitoPlugin.CT == CtrlType.Admin && userInfoData != null && userInfoData.userid > 0)
        {
            VitoPlugin.RequestActionEvent("LookPlayer", userInfoData.userid.ToString());
        }
    }
    /// <summary>
    /// 放弃观察用户
    /// </summary>
    public void OnGiveUpLookUser()
    {
        UnitInfo curInfo = ActionController.instance.CurLookedUnitInfo;
        if (curInfo != null)
        {
            VitoPlugin.RequestActionEvent("GiveUpLookPlayer", curInfo.UnitID.ToString());
        }
    }
    #endregion

}
