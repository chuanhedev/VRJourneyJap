using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoNetActionUsage : MonoBehaviour {
    void OnEnable()
    {

        DemoNetActionCtrl.Act_actionName1_0 += OnResponseAct1;
        DemoNetActionCtrl.Act_actionName2_b += OnResponseAct2;
        DemoNetActionCtrl.Act_actionName3_s += OnResponseAct3;
    }
    void OnDisable()
    {
        DemoNetActionCtrl.Act_actionName1_0 -= OnResponseAct1;
        DemoNetActionCtrl.Act_actionName2_b -= OnResponseAct2;
        DemoNetActionCtrl.Act_actionName3_s -= OnResponseAct3;
    }

    void Start()
    {
        //在需要的地方调用对应的事件
        {
            DemoNetActionCtrl.instance.Rpc_actionName1();
            DemoNetActionCtrl.instance.Rpc_actionName2(false);
            DemoNetActionCtrl.instance.Rpc_actionName3("test");
        }
        
    }

    void OnResponseAct1()
    {
        //这里才是真正执行具体逻辑的位置
    }
    void OnResponseAct2(bool b)
    {
        //这里才是真正执行具体逻辑的位置
    }
    void OnResponseAct3(string msg)
    {
        //这里才是真正执行具体逻辑的位置
    }

}
