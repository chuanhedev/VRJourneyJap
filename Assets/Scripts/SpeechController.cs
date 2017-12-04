using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Pvr_UnitySDKAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeechController : MonoBehaviour
{
    public static SpeechController instance;
    Transform head;
    List<LookDatas> lookDatasList = new List<LookDatas>();
    bool recording = false;

    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (!recording)
            return;
        LookDatas lookDatas = new LookDatas();
        lookDatas.headRotate = head.rotation + "";
        lookDatasList.Add(lookDatas);
    }

    public void StartRecord()
    {
        if (!head)
        {
            GameObject cameras = GameObject.Find("player_VRTypeNew");
            head = cameras.GetComponentInChildren<Camera>().transform;
        }
        lookDatasList.Clear();
        recording = true;
    }

    public void StopRecord()
    {
        recording = false;
        RecordFinish();
    }

    /// <summary>
    /// 获取保存的数据
    /// </summary>
    /// <param name="filePath">文件目录</param>
    /// <param name="deviceId">名字</param>
    /// <param name="panoPath">场景</param>
    /// <param name="qList">旋转</param>
    public void GetRecordData(string filePath, out string deviceId, out string panoPath, out string micTimer, out List<Quaternion> qList)
    {
        string id = "";
        string panoPathString = "";
        string micLength = "";
        List<Quaternion> headRotateList = new List<Quaternion>();
        string jsonText = GetStringData(filePath);
        try
        {
            LookBean dataBean = JsonMapper.ToObject<LookBean>(jsonText, false);
            id = dataBean.deviceId;
            panoPathString = dataBean.panoPath;
            micLength = dataBean.micTimer;
            foreach (LookDatas lookData in dataBean.lookDatasList)
            {
                string rotateString = lookData.headRotate;
                string[] rotateSplits = rotateString.Split(',');

                Quaternion q = new Quaternion();
                float x = 0;
                float y = 0;
                float z = 0;
                float w = 0;
                for (int i = 0; i < rotateSplits.Length; i++)
                {
                    string rotateSplit = rotateSplits[i];
                    if (i == 0)
                    {
                        x = float.Parse(rotateSplit.Substring(1));
                    }
                    else if (i == rotateSplits.Length - 1)
                    {
                        w = float.Parse(rotateSplit.Substring(0, rotateSplit.IndexOf(')')));
                    }
                    else
                    {
                        if (i == 1)
                        {
                            y = float.Parse(rotateSplit);
                        }
                        else
                        {
                            z = float.Parse(rotateSplit);
                        }
                    }
                }
                q.x = x;
                q.y = y;
                q.z = z;
                q.w = w;
                headRotateList.Add(q);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        deviceId = id;
        panoPath = panoPathString;
        micTimer = micLength;
        qList = headRotateList;
    }

    /// <summary>
    /// 保存
    /// </summary>
    bool RecordFinish()
    {
        if (lookDatasList.Count == 0)
        {
            Debug.Log("先添加数据"); return false;
        }

        LookBean lookBean = new LookBean();
        lookBean.deviceId = VitoPlugin.DeviceID;
        lookBean.panoPath = FacadeManager._instance.GetPanoPath();
        lookBean.micTimer = MicController.instance.txtTimer.text;
        lookBean.lookDatasList = lookDatasList;
        string data = JsonMapper.ToJson(lookBean);

        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
        StartCoroutine(MicUtils.UploadFileToServer(byteArray, MicController.instance.micRecorder.serverUrl, MicController.instance.userName + ".txt", "speech"));

        //bool success = Save(data);
        //if (success)
        //{
        //    MyPicoLog.SetLog1("保存成功");
        //    lookDatasList.Clear();
        //}
        //else
        //{
        //    MyPicoLog.SetLog1("保存失败");
        //}
        return true;
    }

    /// <summary>
    /// 读取保存的数据 string
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    string GetStringData(string filePath)
    {
        string jsonText;
        try
        {
            jsonText = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace); return string.Empty;
        }

        return jsonText;
    }

    /// <summary>
    /// 数据的路径
    /// </summary>
    /// <returns></returns>
    //string GetFilePath()
    //{
    //    return Application.persistentDataPath + "/SpeechData/" + VitoPlugin.DeviceID + ".txt";
    //}

    /*
    bool Save(string data)
    {
        string path = Application.persistentDataPath + "/SpeechData";
        string filePath;
        StreamWriter sw = null;
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            sw = File.CreateText(filePath);
            sw.WriteLine(data);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace); return false;
        }
        sw.Flush();
        sw.Close();
        Debug.Log("保存的目录：" + filePath);
        return true;
    }
    */
}

public class LookBean
{
    public string deviceId;
    public string panoPath;
    public string micTimer;
    public List<LookDatas> lookDatasList;
}

public class LookDatas
{
    public string headRotate;
}

