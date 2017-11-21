using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RefreshPanoManager
{
    FacadeManager facadeManager;
    public RefreshPanoManager(FacadeManager facadeManager)
    {
        this.facadeManager = facadeManager;
    }

    string fileName = "PanoData.txt";
    string filePath;

    public void GetRefreshPanoData(Action<string> LoadCallback)
    {
        LoadPanoDataFile(LoadCallback);
    }

    void LoadPanoDataFile(Action<string> LoadCallback)
    {
        filePath =
#if UNITY_ANDROID && !UNITY_EDITOR
                  "jar:file:///" + Application.dataPath  + "!/assets/";
#elif UNITY_IPHONE && !UNITY_EDITOR
                   "file://" + Application.dataPath  +"/Raw/"
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                  "file://" + Application.streamingAssetsPath + "/";
#else
                   string.Empty;  
#endif
        facadeManager.StartCoroutine(DoLoad(LoadCallback));
    }

    IEnumerator DoLoad(Action<string> LoadCallback)
    {
        string path = filePath + fileName;
        WWW www = new WWW(path);
        yield return www;

        if (LoadCallback != null) LoadCallback(www.text);
    }

}
