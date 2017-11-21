using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class AndroidInfo
{
    public int power;
}

public class VitoAndroidSDK : MonoBehaviour {
    /// <summary>
    /// 当前设备电量，加%就是百分比
    /// </summary>
    public static int BatteryLevel;
    private float refreshInfoInterval=10;
    private float refreshInfoTimer = 10;

    private const string APPPAUSE = "1";
    public const string APPRESUME = "2";


    void Start()
    {
        VitoPlugin.RegisterActionEvent("s_a_i", (actionname, parameter, deviceid) =>
        {
            if (VitoPlugin.CT == CtrlType.Admin)
            {
                //控制台 收到客户端发来的数据
                ReceivedAndroidInfo(parameter, deviceid);

            }
        });
    }

    private void ReceivedAndroidInfo(string parameter,string deviceid)
    {
        JsonData jd = JsonMapper.ToObject(parameter);
        AndroidInfo data = new AndroidInfo();
        data.power = (int)jd["power"];
        UserInfoManager.instance.OnUserPowerChange(deviceid, data.power / 100.0f);
    }
    public static void SendAndroidInfoData(AndroidInfo data)
    {
        JsonData jd = new JsonData();
        jd["power"] = data.power;
        VitoPlugin.RequestActionEvent("s_a_i", jd.ToJson());
    }


    public static void RefreshBatteryLevel()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.vito.unityvitoplugin.MainActivity"))
            {
                Debug.Log("get bettery level");
                if (jc != null)
                {
                    int newBatteryLevel = jc.CallStatic<int>("getBetteryLevel");
                   
                    if (newBatteryLevel != BatteryLevel)
                    {
                        BatteryLevel = newBatteryLevel;
                        SendAndroidInfoData(new AndroidInfo() { power = BatteryLevel });
                    }
                }
            }
        }
        
    }


    public void message(string msg)
    {
        Debug.Log("get msg from android:"+msg);
        switch(msg)
        {
            case APPRESUME:
                //Debug.Log("App resume");
                UserInfoManager.instance.HMDMounted();
                break;
            case APPPAUSE:
                UserInfoManager.instance.HMDUnmounted();
                //Debug.Log("App pause");
                break;
        }
    }

    void Update()
    {
        refreshInfoTimer += Time.unscaledDeltaTime;
        if(refreshInfoTimer>refreshInfoInterval)
        {
            refreshInfoTimer -= refreshInfoInterval;
            RefreshBatteryLevel();
        }
    }
}
