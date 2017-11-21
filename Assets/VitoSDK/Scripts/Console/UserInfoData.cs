using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
/// <summary>
/// 用户位置状态
/// </summary>
public enum UserPosStatus
{
    None = 0, //未录入状态
    DefaultScene, //位于默认场景
    InRuningScene, //正常运行
    SceneIsLoading, //正在加载
    SceneLoadedOver, //加载完成
}

/// <summary>
/// 用户头盔状态
/// </summary>
public enum UserHMDStatus
{
    PutOff,
    PutOn,
    
}

[System.Serializable]
public class UserInfoData{

    [JsonIgnore]
    public int userid;
    public string name="";
    public string deviceid="";
    public string number="";
    public string info="";

    [JsonIgnore]
    private UserPosStatus _SceneStatus=UserPosStatus.None;
    [JsonIgnore]
    private UserHMDStatus _HMDStatus=UserHMDStatus.PutOff ;
    [JsonIgnore]
    public System.Action<UserHMDStatus> OnHMDStatusChange;
    [JsonIgnore]
    public System.Action<UserPosStatus> OnPosStatusChange;
    [JsonIgnore]
    public System.Action<bool, int, string> OnLevelStatusChange;
    [JsonIgnore]
    public System.Action<float> onPowerChange;
    [JsonIgnore]
    public System.Action OnRefresh;
    [JsonIgnore]
    public UserHMDStatus mHMDStatus
    {
        get
        {
            return _HMDStatus;
        }
        set
        {
            if(_HMDStatus!=value)
            {
                _HMDStatus = value;
                if(OnHMDStatusChange!=null)
                {
                    OnHMDStatusChange(_HMDStatus);
                }
            }
        }
    }
    [JsonIgnore]
    public UserPosStatus mSceneStatus
    {
        get
        {
            return _SceneStatus;
        }
        set
        {
            if(_SceneStatus!=value)
            {
                _SceneStatus = value;
                if(OnPosStatusChange!=null)
                {
                    OnPosStatusChange(_SceneStatus);
                }
            }
        }
    }


}
