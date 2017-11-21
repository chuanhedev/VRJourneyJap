using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Text;
using Newtonsoft.Json;
//注意此处只是做demo演示，事实上星级和得分往往在一个机构体内存储，只需要一个文件即可

#region 收集的数据的结构

//注意此处只是做demo演示，事实上星级和得分往往在一个机构体内存储，只需要一个文件即可

/// <summary>
/// 需要收集的数据类型枚举
/// </summary> 
public enum CDType
{
    StarGrade=1, //星级
    Score=2, //分数
    StartLevel=3,
    EndLevel=4,


    //可以自己添加需要收集的收据类型
}

/// <summary>
/// 收集的数据类型的基类
/// </summary>
public class CDBase
{
    public CDType dataType;

    public int level;  //当前操作或场景的编号,作为当前数据的唯一标识，如果收到同一个设备ID的同一个场景的数据，则只保留最新数据

    public string levelName; //当前操作或场景的名称
}

/// <summary>
/// 收集的星级数据的结构
/// </summary>
public class CDStar:CDBase
{
    public int starGrade; //当前操作获得的星级
    public float sumTime;  //完成当前操作使用的时间
}
/// <summary>
/// 收集的得分数据的结构
/// </summary>
public class CDScore:CDBase
{
    public int sumScore; //当前操作获得的分数
    public int score1; //环节1的得分
    public int score2;//环节2的得分
    public int score3; //环节3的得分
    public float sumTime;  //完成当前操作使用的时间
}
/// <summary>
/// 关卡或者环节开始消息
/// </summary>
public class CDStartLevel:CDBase
{
    public string startTime;
}
/// <summary>
/// 关卡或者环节结束消息
/// </summary>
public class CDEndLevel:CDBase
{
    public string endTime;        
}
#endregion

public class DataCollectionManager : MonoBehaviour {


    public static DataCollectionManager instance { get; private set; }

    public Dictionary<string, List<CDScore>> collectedScoreData = new Dictionary<string, List<CDScore>>();
    public Dictionary<string, List<CDStar>> collectedStarData = new Dictionary<string, List<CDStar>>();


    public Dictionary<string, CDStartLevel> collectedStartLevelData = new Dictionary<string, CDStartLevel>();

    public Dictionary<string, CDEndLevel> collectedEndLevelData = new Dictionary<string,CDEndLevel>();
    private Queue<CDBase> collectedData = new Queue<CDBase>();

    #region Unity Function
    private void Awake()
    {
        if(instance!=null)
        {
            DestroyImmediate(instance);
        }
        instance = this;
    }

    private void OnDestroy()
    {
        WriteHistoryCollectedData(CDType.Score);
        WriteHistoryCollectedData(CDType.StarGrade);
    }
    
	private void Start () {
        //注册数据收集消息事件:"s_c_d"
        VitoPlugin.RegisterActionEvent("s_c_d", (actionname,parameter,deviceid) =>
         {
             if(VitoPlugin.CT==CtrlType.Admin)
             {
                 //控制台 收到客户端发来的数据
                 ReceivedCollectedData(parameter,deviceid);
                 
             }
         });
        LoadHistoryCollectionData();
	}

    void Update()
    {
        ProcedualWillSendedCollectedData();
    }

    #endregion

    #region private function

    //注意此处只是做demo演示，实际使用中星级和得分往往在一个机构体内存储，只需要一个文件即可
    /// <summary>
    /// 接收到收集的数据
    /// </summary>
    /// <param name="content"></param>
    /// <param name="deviceid"></param>
    private void ReceivedCollectedData(string content,string deviceid)
    {
        JsonData jd = JsonMapper.ToObject(content);
        int level = (int)jd["level"];
        string levelName = jd["levelname"].ToString();
        CDType dataType = (CDType)(int)jd["datatype"];

        switch (dataType)
        {
            case CDType.Score:

                ReceiveScoreData(new CDScore()
                {
                    level = level,
                    dataType = dataType,
                    levelName = levelName,
                    score1 = (int)jd["score1"],
                    score2 = (int)jd["score2"],
                    score3 = (int)jd["score3"],
                    sumScore = (int)jd["sumscore"],
                    sumTime = float.Parse(jd["sumtime"].ToString()),
                },deviceid);
                break;
            case CDType.StarGrade:
                ReceiveStarData(new CDStar()
                {
                    level = level,
                    dataType = dataType,
                    levelName = levelName,
                    starGrade = (int)jd["stargrade"],
                    sumTime = float.Parse(jd["sumtime"].ToString()),
                },deviceid);
                break;
            case CDType.StartLevel:
                CDStartLevel startLevelData = new CDStartLevel()
                {
                    level = level,
                    dataType = dataType,
                    levelName = levelName,
                    startTime = jd["starttime"].ToString(),
                };
                //如果玩家重复开始此关卡 更新旧有的数据
                if(collectedStartLevelData.ContainsKey(deviceid))
                {
                    collectedStartLevelData[deviceid] = startLevelData;
                }
                else
                {
                    collectedStartLevelData.Add(deviceid,  startLevelData );
                }

                //如果玩家重复开始此关卡,即缓存中有之前通关的记录 删除之前保存的数据
                if (collectedEndLevelData.ContainsKey(deviceid))
                {
                    collectedEndLevelData.Remove(deviceid);
                }
                UserInfoManager.instance.ChangeUserLevelStatus(deviceid,true,startLevelData.level,startLevelData.levelName);
                break;
            case CDType.EndLevel:
                CDEndLevel endLevelData = new CDEndLevel()
                {
                    level = level,
                    dataType = dataType,
                    levelName = levelName,
                    endTime = jd["endtime"].ToString(),
                };

                if (collectedEndLevelData.ContainsKey(deviceid))
                {
                    collectedEndLevelData[deviceid] = endLevelData;
                }
                else
                {
                    collectedEndLevelData.Add(deviceid,  endLevelData );
                }

                UserInfoManager.instance.ChangeUserLevelStatus(deviceid, false, endLevelData.level, endLevelData.levelName);
                break;
        }
    }

    /// <summary>
    /// 根据数据类型，单独处理，此处为得分数据
    /// </summary>
    private void ReceiveScoreData(CDScore data,string deviceid)
    {
        if (collectedScoreData.ContainsKey(deviceid))
        {
            if (collectedScoreData[deviceid].Exists((d) => { return d.level == data.level; }))
            {
                //正常情况下这里不应该是直接替换，而是根据业务逻辑，判断当前得分是否比历史得分高决定是否替换
                collectedScoreData[deviceid].Remove(collectedScoreData[deviceid].Find((d) => { return d.level == data.level; }));
                collectedScoreData[deviceid].Add(data);
            }
            else
            {
                collectedScoreData[deviceid].Add(data);
            }
        }
        else
        {
            collectedScoreData.Add(deviceid, new List<CDScore>() { data });
        }
        WriteHistoryCollectedData(CDType.Score);
    }

    /// <summary>
    /// 根据数据类型，单独处理，此处为星级数据
    /// </summary>
    private void ReceiveStarData(CDStar data,string deviceid)
    {
        if(collectedStarData.ContainsKey(deviceid))
        {
            if(collectedStarData[deviceid].Exists((d) => { return d.level == data.level; }))
            {
                //正常情况下这里不应该是直接替换，而是根据业务逻辑，判断当前得分是否比历史得分高决定是否替换
               collectedStarData[deviceid].Remove(collectedStarData[deviceid].Find((d) => { return d.level == data.level; }));
                collectedStarData[deviceid].Add(data);
            }else
            {
                collectedStarData[deviceid].Add(data);
            }
        }
        else
        {
            collectedStarData.Add(deviceid, new List<CDStar>() { data });
        }
        WriteHistoryCollectedData(CDType.StarGrade);
    }



    /// <summary>
    /// 加载历史数据，在每次启动时加载一次
    /// </summary>
    private void LoadHistoryCollectionData()
    {
        //注意此处只是做demo演示，事实上星级和得分往往在一个机构体内存储，只需要一个文件即可

        string historyStarFile = "CollectedStarData.txt";  //星级数据历史纪录

        string historySocreFile = "CollectedScoreData.txt";  //得分历史纪录

        string path = Application.streamingAssetsPath + "/" + historyStarFile;
        if(File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            byte[] byData = new byte[file.Length];
            file.Read(byData, 0, (int)file.Length);

            string content = Encoding.UTF8.GetString(byData);
            collectedStarData = JsonConvert.DeserializeObject<Dictionary<string, List<CDStar>>>(content);
            file.Close();
        }
        path = Application.streamingAssetsPath + "/" + historySocreFile;
        if(File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            byte[] byData = new byte[file.Length];
            file.Read(byData, 0, (int)file.Length);
            string content = Encoding.UTF8.GetString(byData);
            collectedScoreData = JsonConvert.DeserializeObject<Dictionary<string, List<CDScore>>>(content);
            file.Close();
        }
    }

    /// <summary>
    /// 把收集的数据持久保存
    /// </summary>
    public void WriteHistoryCollectedData(CDType dataType )
    {
        string historyStarFile = "CollectedStarData.txt";  //星级数据历史纪录文件名称
        string historySocreFile = "CollectedScoreData.txt";  //得分历史纪录文件名称
        string path = "";
        switch(dataType)
        {
            case CDType.StarGrade:
                //星级数据文件路径
                path = Application.streamingAssetsPath + "/" + historyStarFile;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                {
                    FileStream file = new FileStream(path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(file);
                    string content = JsonConvert.SerializeObject(collectedStarData);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                break;
            case CDType.Score:
                //得分数据文件路径
                path = Application.streamingAssetsPath + "/" + historySocreFile;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                {
                    FileStream file = new FileStream(path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(file);
                    string content = JsonConvert.SerializeObject(collectedScoreData);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                break;
        }
        
        

    }

    /// <summary>
    /// 处理即将发送的数据
    /// </summary>
    private void ProcedualWillSendedCollectedData()
    {
        if (collectedData.Count > 0)
        {
            CDBase data = collectedData.Dequeue();
            if (VitoPlugin.IsNetMode && VitoPlugin.CT != CtrlType.Admin)
            {
                JsonData jd = new JsonData();
                jd["level"] = data.level;
                jd["levelname"] = data.levelName;
                jd["datatype"] = (int)data.dataType;
                switch (data.dataType)
                {
                    case CDType.Score:
                        CDScore scoredata = (CDScore)data;
                        jd["score1"] = scoredata.score1;
                        jd["score2"] = scoredata.score2;
                        jd["score3"] = scoredata.score3;
                        jd["sumscore"] = scoredata.sumScore;
                        jd["sumtime"] = scoredata.sumTime;
                        break;
                    case CDType.StarGrade:
                        CDStar stardata = (CDStar)data;
                        jd["stargrade"] = stardata.starGrade;
                        jd["sumtime"] = stardata.sumTime;
                        break;
                    case CDType.StartLevel:
                        CDStartLevel startLevelData = (CDStartLevel)data;
                        jd["starttime"] = startLevelData.startTime;
                        break;
                    case CDType.EndLevel:
                        CDEndLevel endLevelData = (CDEndLevel)data;
                        jd["endtime"] = endLevelData.endTime;
                        break;
                }

                VitoPlugin.RequestActionEvent("s_c_d", jd.ToJson());
            }
        }
    }

    #endregion



    #region public function
    /// <summary>
    /// 发送收集的数据
    /// </summary>
    /// <param name="data"> 收集的数据类型的基类，不要直接使用CDBase</param>
    public void SendCollectedData(CDBase data)
    {
        if (VitoPlugin.IsNetMode && VitoPlugin.CT != CtrlType.Admin)
        {
            collectedData.Enqueue(data);
        }            
    }


    public List<CDScore> GetCollectedScoreData(string deviceId)
    {
        if (collectedScoreData.ContainsKey(deviceId))
        {
            return collectedScoreData[deviceId];
        }
        return null;        
    }

    public List<CDStar> GetCollectedStarData(string deviceId)
    {
        if (collectedStarData.ContainsKey(deviceId))
        {
            return collectedStarData[deviceId];
        }
        return null;
    }

    #endregion


}
