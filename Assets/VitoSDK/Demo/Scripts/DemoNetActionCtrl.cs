using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DemoNetActionCtrl : MonoBehaviour {

    #region 留给外部注册的事件
    public static Action Act_actionName1_0;
    public static Action<bool> Act_actionName2_b;
    public static Action<string> Act_actionName3_s;
    #endregion

    #region  给外部调用的接口
    public void Rpc_actionName1()
    {
        //非网络模式下直接调用本地命令
        if(!VitoPlugin.IsNetMode)
        {
            Cmd_actionName1();
        }else
        {
            VitoPlugin.RequestActionEvent("event1");
        }
        
    }
    public void Rpc_actionName2(bool isAction)
    {
        //非网络模式或者—— 客户端在自由模式下，没有被观察，不传输网络时间，直接调用本地
        if(!VitoPlugin.IsNetMode||(VitoPlugin.CM==CtrlMode.FreeMode&&VitoPlugin.CT==CtrlType.Player&&!VitoPlugin.isMaster))
        {
            Cmd_actionName1();
        }
        else
        {
            VitoPlugin.RequestActionEvent("event2",isAction.ToString());
        }
    }

    public void Rpc_actionName3(string msg)
    {
        if(!VitoPlugin.IsNetMode)
        {
            Cmd_actionName3(msg);
        }else
        {
            VitoPlugin.RequestActionEvent("event3",msg);
        }
        
    }
    #endregion

    #region 内部调用的接口
    private void Cmd_actionName1()
    {
        if(Act_actionName1_0!=null)
        {
            Act_actionName1_0();
        }
        //do something 
    }
    private void Cmd_actionName2(bool isAction)
    {
        if(Act_actionName2_b!=null)
        {
            Act_actionName2_b(isAction);
        }
        //do something like active or deactive gameobject
    }
    private void Cmd_actionName3(string msg)
    {
        if(Act_actionName3_s!=null)
        {
            Act_actionName3_s(msg);
        }
        //do something like change ui show
    }
    #endregion

    public static DemoNetActionCtrl instance { get; private set; }
    void Awake()
    {
        instance = this;
        VitoPlugin.RegisterActionEvent("event1",(string actionName,string parameter,string deviceid)=>{
            Cmd_actionName1();
        });
        VitoPlugin.RegisterActionEvent("event2", (string actionName, string parameter, string deviceid) => {
            //Cmd_actionName2(bool.Parse(parameter));
            Cmd_actionName2(parameter.Equals("True")||parameter.Equals("true"));
        });
        VitoPlugin.RegisterActionEvent("event3", (string actionName, string parameter, string deviceid) => {
            Cmd_actionName3(parameter);
        });
    }


	// Use this for initialization
	void Start () {
		
	}
}
