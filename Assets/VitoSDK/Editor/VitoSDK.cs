using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class VitoSDK{
    [MenuItem("VitoSDK/CreateGameClient")]
   static void CreateGameClient()
    {
        GameObject go = new GameObject();
        go.name = "GameClient";
        go.AddComponent<ConnectionClient>();
    }

    [MenuItem("VitoSDK/CreatePlayerPrefab")]
    static void CreatePlayerPrefab()
    {
        Object go = AssetDatabase.LoadAssetAtPath("Assets/VitoSDK/Prefabs/player_VRTypeNew.prefab", typeof(Object));
        GameObject playerVR = PrefabUtility.InstantiatePrefab(go as GameObject) as GameObject;
        playerVR.name = "Player_VRTypeNew";
    }

    [MenuItem("VitoSDK/CreateLogicClient")]
    static void CreateLogicClient()
    {
        GameObject logicClient = new GameObject();
        logicClient.name = "LogicClient";
        logicClient.AddComponent<VitoSDKConfig>();
        GameObject actionCtrl = new GameObject();
        actionCtrl.name = "ActionController";
        actionCtrl.transform.SetParent(logicClient.transform);
        actionCtrl.AddComponent<ActionController>();
        actionCtrl.AddComponent<VitoPluginLoadScene>();
        actionCtrl.AddComponent<VitoPluginPlayVideo>();
        actionCtrl.AddComponent<HostActionController>();
        actionCtrl.AddComponent<UserInfoManager>();
        actionCtrl.AddComponent<VitoPluginQuestionManager>();

        GameObject utility = new GameObject();
        utility.name = "Utility";
        utility.transform.SetParent(logicClient.transform);
        utility.AddComponent<NetManager>();
        utility.AddComponent<LogicClient>();

        Object go=AssetDatabase.LoadAssetAtPath("Assets/VitoSDK/Prefabs/UIRoot.prefab", typeof(Object));
        GameObject uiRoot = PrefabUtility.InstantiatePrefab(go as GameObject) as GameObject;
        uiRoot.name = "UIRoot";
        uiRoot.transform.SetParent(logicClient.transform);
        uiRoot.transform.localScale = Vector3.one;
        uiRoot.transform.localPosition = Vector3.zero;
        uiRoot.transform.localEulerAngles = Vector3.zero;

    }
}
