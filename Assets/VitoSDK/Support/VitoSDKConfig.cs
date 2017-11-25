using UnityEngine;
using System.Collections;
using LitJson;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
public class VitoSDKConfig : MonoBehaviour
{
    public static VitoSDKConfig instance { get; private set; }


    /// <summary>
    /// �ӷ�������ȡ�������ļ���·��
    /// </summary>
    private string globalSettingUrl = "http://121.40.93.137:8888/GlobalSetting.txt";

    /// <summary>
    /// PC �����������б�
    /// </summary>
    [HideInInspector]
    private string ui_scene_list_data_pc_url;


    /// <summary>
    /// ��׿�����������б�
    /// </summary>    
    private string ui_scene_list_data_android_url;
    public JsonData ui_othersscene_list_data_jsondata = new JsonData();

    public string FilePath { get; private set; }

    private string mLocalConfigPath;

    private bool isConfigLoadOver = false;

    /// <summary>
    /// ���������ļ��������
    /// </summary>

    public bool IsConfigLoadOver
    {
        get
        {
            return isConfigLoadOver;
        }
        private set
        {
            isConfigLoadOver = value;
            ConnectionClientConfig.isConfigLoadOver = value;
        }
    }
    private string _logicServerIP;
    private int _logicServerPort;
    private string _gameServerIP;
    private int _gameServerPort;

    public string logicServerIP
    {
        get { return _logicServerIP; }
        private set { _logicServerIP = value; ConnectionClientConfig.logicServerIp = value; }
    }

    public int logicServerPort
    {
        get { return _logicServerPort; }
        private set { _logicServerPort = value; ConnectionClientConfig.logicServerPort = value; }
    }
    public string gameServerIP
    {
        get { return _gameServerIP; }
        private set { _gameServerIP = value; ConnectionClientConfig.gameServerIp = value; }
    }

    public int gameServerPort
    {
        get { return _gameServerPort; }
        private set { _gameServerPort = value; ConnectionClientConfig.gameServerPort = value; }
    }

    /// <summary>
    /// ���紫������ֶ�
    /// </summary>
    public string secret { get { return ConnectionClientConfig.appkey; } private set { ConnectionClientConfig.appkey = value; } }
    /// <summary>
    /// ��Ƶ�ļ���Ŀ¼
    /// </summary>
    public string VideoDirRoot { get; private set; }

    /// <summary>
    /// ��PCƽ̨��ʹ���������
    /// </summary> 
    [Header("use relative path on pc")]
    public bool isRelativePath = false;

    /// <summary>
    /// ����ģʽ��������ģʽ(�û������ɲ���)�Ϳ���ģʽ(�û�ֻ�ܽ��ܿ���̨�Ĳ���)
    /// </summary>
    public CtrlMode ctrlMode = CtrlMode.FreeMode;
    /// <summary>
    /// ��ǰ�û����ͣ��ֹ���Ա����ͨ�û�������Ա������̨
    /// </summary>
    public CtrlType ctrlType = CtrlType.Admin;
    public AdminStatus adminStatus = AdminStatus.Wait;
    /// <summary>
    /// ��Ƶ�����ļ���·��
    /// </summary>
    private string videoconfigpath;
    /// <summary>
    /// ���������ļ���·��
    /// </summary>
    private string sceneconfigpath;
    /// <summary>
    /// pcƽ̨�ĸ�Ŀ¼
    /// </summary>
    private string pcfileroot;
    /// <summary>
    /// ��׿ƽ̨�ĸ�Ŀ¼
    /// </summary>
    private string androidfileroot;
    private string fileroot;
    public string videopath { get; set; }
    public List<VitoSceneConfigData> sceneConfigList = new List<VitoSceneConfigData>();
    public List<VitoVideoConfigData> videoConfigList = new List<VitoVideoConfigData>();

    void Awake()
    {
        instance = this;
        IsConfigLoadOver = false;
        VitoPlugin.CM = ctrlMode;
        VitoPlugin.CT = ctrlType;
        VitoPlugin.AS = adminStatus;
        DontDestroyOnLoad(gameObject);

    }
    void OnEnable()
    {
        LogicClient.LoginSuccessListener += LogicClientLoginSuccess;
    }
    void OnDisable()
    {
        LogicClient.LoginSuccessListener -= LogicClientLoginSuccess;
    }

    /// <summary>
    /// �û�����ϵͳ�ɹ�
    /// </summary>
    private void LogicClientLoginSuccess()
    {
        HostActionController.instance.OnRequestGetAllUserId();
        ActionController.instance.JoinBrodcastMesg();
        ActionController.instance.RequestLastBrocastMsg();
    }
    // Use this for initialization
    void Start()
    {
        FilePath = "file:///" + Application.streamingAssetsPath;
        //LoadLocalConfig();
        ReadGlobalConfigFile();
    }

    /// <summary>
    /// ��ȡȫ�������ļ�
    /// </summary>
    void ReadGlobalConfigFile()
    {

        string globalfile = "GlobalSetting.xml";
        if (Application.platform == RuntimePlatform.Android)
        {
            globalfile = "sdcard/vitoresources/" + globalfile;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            globalfile = @".\" + globalfile;
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        XmlReader reader = XmlReader.Create(globalfile);
        xmlDoc.Load(reader);
        XmlNode root = xmlDoc.SelectSingleNode("VitoSDKConfig");
        XmlNodeList xnl = root.ChildNodes;
        for (int i = 0; i < xnl.Count; i++)
        {
            string name = xnl.Item(i).Name.ToLower();
            string content = xnl.Item(i).InnerText;
            switch (name)
            {
                case "logicserverport":
                    logicServerPort = Int32.Parse(content);
                    break;
                case "gameserverport":
                    gameServerPort = Int32.Parse(content);
                    break;
                case "serverip":
                    logicServerIP = content;
                    gameServerIP = content;
                    break;
                case "appkey":
                    secret = content;
                    break;
                case "pcfileroot":
                    pcfileroot = content;
                    break;
                case "androidfileroot":
                    androidfileroot = content;
                    break;
                case "videoconfigfile":
                    videoconfigpath = content;
                    break;
                case "sceneconfigfile":
                    sceneconfigpath = content;
                    break;
                case "videopath":
                    videopath = content;
                    break;
            }
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            fileroot = androidfileroot;
            videopath = fileroot + "/" + videopath;
            videoconfigpath = fileroot + "/" + videoconfigpath;
            sceneconfigpath = fileroot + "/" + sceneconfigpath;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            fileroot = pcfileroot;
            if (!fileroot.Contains(":"))
            {
                fileroot += ":";
            }
            videopath = videopath.Replace("/", "\\");
            videoconfigpath = videoconfigpath.Replace("/", "\\");
            sceneconfigpath = sceneconfigpath.Replace("/", "\\");

            if (isRelativePath)
            {
                videopath = @".\" + videopath;
                videoconfigpath = @".\" + videoconfigpath;
                sceneconfigpath = @".\" + sceneconfigpath;
            }
            else
            {
                videopath = fileroot + "\\" + videopath;
                videoconfigpath = fileroot + "\\" + videoconfigpath;
                sceneconfigpath = fileroot + "\\" + sceneconfigpath;
            }


        }
        //����ǿ���̨�������ȡ���������ļ������������ļ���ȡ����
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            ReadConfigFile(videoconfigpath, true);
            ReadConfigFile(sceneconfigpath, false);
        }
        else
        {
            IsConfigLoadOver = true;
        }
    }

    void ReadConfigFile(string path, bool isVideo)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        XmlReader reader = XmlReader.Create(path);
        xmlDoc.Load(reader);
        XmlNode root = xmlDoc.SelectSingleNode("VitoConfig");
        string iconpath = ((XmlElement)root).GetAttribute("iconpath");
        if (Application.platform == RuntimePlatform.Android)
        {
            iconpath = fileroot + "/" + iconpath + "/";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            iconpath = iconpath.Replace("/", "\\");
            iconpath = fileroot + "\\" + iconpath + "\\";
        }
        XmlNodeList xnl = root.ChildNodes;
        foreach (XmlNode xn in xnl)
        {
            if (isVideo)
            {
                VitoVideoConfigData data = new VitoVideoConfigData();
                XmlNodeList xn0 = xn.ChildNodes;
                data.title = xn0.Item(0).InnerText;
                data.intro = xn0.Item(1).InnerText;
                string tags = xn0.Item(2).InnerText;
                tags = tags.Replace("|", ",");
                data.tags = tags.Split(',');
                data.videoName = xn0.Item(3).InnerText;
                data.iconPath = iconpath + xn0.Item(4).InnerText;
                videoConfigList.Add(data);
            }
            else
            {
                VitoSceneConfigData data = new VitoSceneConfigData();
                XmlNodeList xn0 = xn.ChildNodes;
                data.title = xn0.Item(0).InnerText;
                data.intro = xn0.Item(1).InnerText;
                string tags = xn0.Item(2).InnerText;
                tags = tags.Replace("|", ",");
                data.tags = tags.Split(',');
                data.sceneName = xn0.Item(3).InnerText;
                data.iconPath = iconpath + xn0.Item(4).InnerText;
                sceneConfigList.Add(data);
            }
        }
        IsConfigLoadOver = true;
    }

    /// <summary>
    /// ��ȡ����˵�ȫ�������ļ�·�����ɴ���û����
    /// </summary> 
    void GetNetConfigFilePath()
    {
        NetManager.instance.ProcessDownloadItem(new NetItem() { url = "http://" + logicServerIP + ":" + logicServerPort + "/GetGlobalSetting" }, (MyWWW www) =>
           {
               if (!string.IsNullOrEmpty(www.text))
               {
                   globalSettingUrl = www.text;
                   NetManager.instance.ProcessDownloadItem(new NetItem() { url = globalSettingUrl }, (MyWWW www1) =>
                   {
                       if (!string.IsNullOrEmpty(www1.text))
                       {
                           JsonData SettingInfoJD = JsonMapper.ToObject(www1.text);
                           ui_scene_list_data_pc_url = SettingInfoJD["ui_scene_list_data_pc_url"].ToString();
                           ui_scene_list_data_android_url = SettingInfoJD["ui_scene_list_data_android_url"].ToString();
                           LoadOtherSettingInfo();
                       }
                   });
               }
           });
    }

    /// <summary>
    /// ���ر��ص�ȫ��������Ϣ
    /// </summary>
    /*
    void LoadLocalConfig()
    {
        string configFilePath = "";
        if (Application.platform == RuntimePlatform.WindowsPlayer||Application.platform==RuntimePlatform.WindowsEditor)
        {
            configFilePath=Application.dataPath + "/../GlobalSetting.txt";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            configFilePath= "sdcard/vitoresources/GlobalSetting.txt";
        }
        if (!File.Exists(configFilePath))
        {
            Debug.LogError("�����ļ�������");
            return;
            //configFilePath = Application.streamingAssetsPath + "/VitoConfigFile.json.txt";
        }
        mLocalConfigPath = configFilePath;
        {
            string text=File.ReadAllText(mLocalConfigPath);
            //NetManager.instance.ProcessDownloadItem(new NetItem() { url = mLocalConfigPath }, (MyWWW www) =>
            {
                JsonData tempjd = JsonMapper.ToObject(text);

                if(tempjd.Contains("serverip"))
                {
                    logicServerIP = tempjd["serverip"].ToString();
                    gameServerIP = logicServerIP;
                }else
                {
                    logicServerIP = tempjd["logicServerIP"].ToString();
                    gameServerIP = tempjd["gameServerIP"].ToString();
                }
                if(tempjd.Contains("appid"))
                {
                    logicServerPort = Int32.Parse(tempjd["appid"].ToString());
                    gameServerPort = logicServerPort + 1000;
                }
                else
                {
                    logicServerPort = (int)tempjd["logicServerPort"];
                    gameServerPort = (int)tempjd["gameServerPort"];
                }
                if(tempjd.Contains("appkey"))
                {
                    secret = tempjd["appkey"].ToString();
                }else
                {
                    secret = tempjd["secret"].ToString();
                }


                if (tempjd.Contains("VitoDirRoom"))
                {
                    VideoDirRoot = tempjd["VitoDirRoom"].ToString();
                }else
                {
                    if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        VideoDirRoot = Application.dataPath.Substring(0, Application.dataPath.IndexOf(":"));
                    }
                }    
                if(tempjd.Contains("videoconfig"))
                {
                    bool videoConfig = (bool)tempjd["videoconfig"];
                    if(videoConfig)
                    {
                        ui_scene_list_data_pc_url = "file:///"+Application.streamingAssetsPath + "/data_ui_scene_others.json.txt";// SettingInfoJD["ui_scene_list_data_pc_url"].ToString();
                        ui_scene_list_data_android_url = "file:///"+Application.streamingAssetsPath + "/data_ui_scene_others.json.txt";
                        LoadOtherSettingInfo();
                    }
                    else
                    {
                        GetNetConfigFilePath();
                    }
                }
                else
                {
                    GetNetConfigFilePath();
                }
            }
        }
        
    }
    */
    /// <summary>
    /// ��������������Ϣ,��ʱû��,�Ժ��������������;.
    /// </summary>
    void LoadOtherSettingInfo()
    {
        string tempPath = ui_scene_list_data_pc_url;
        NetManager.instance.ProcessDownloadItem(new NetItem() { url = tempPath }, (MyWWW www) =>
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                ui_othersscene_list_data_jsondata = JsonMapper.ToObject(www.text);
                IsConfigLoadOver = true;
            }
        });
    }


}

public class VitoSceneConfigData
{
    public int id;
    public string title;
    public string intro;
    public string[] tags;
    public string sceneName;
    public string iconPath;
}
public class VitoVideoConfigData
{
    public int id;
    public string title;
    public string intro;
    public string[] tags;
    public string videoName;
    public string iconPath;
}