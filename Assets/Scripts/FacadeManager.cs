using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacadeManager : MonoBehaviour
{
    public static FacadeManager _instance;
    [SerializeField] GameObject menu;
    public Material[] panoMats;
    RefreshPanoManager refreshPanoManager;
    MyUIManager myUIManager;
    UpdatePanoTextures updatePanoTextures;
    UpdatePanoBlack updatePanoBlack;
    Action<string> LoadCallback;
    Image black;

    void Awake()
    {
        _instance = this;
        InitManagers();
        LoadCallback += OnLoadCallback;
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
    ///点击pico手柄home键时调用
    /// </summary>
    /// <param name="isShow"></param>
    public void SwitchPicoMenu()
    {
        //myUIManager.CheckPicoMenu(menu);
    }

    public void LoadPano(string panoPath)
    {
        updatePanoTextures.UpdatePano(panoPath);
    }

    public void UpdatePano(string panoPath)
    {
        updatePanoBlack.UpdatePano(black, panoPath);
    }

    void InitManagers()
    {
        refreshPanoManager = new RefreshPanoManager(this);
        myUIManager = new MyUIManager();
        updatePanoTextures = new UpdatePanoTextures(this);
        updatePanoBlack = new UpdatePanoBlack(this);
    }
}
