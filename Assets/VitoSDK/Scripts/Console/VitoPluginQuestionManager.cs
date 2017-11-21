using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class ResultList
{
    public int listID;
    public string listTitle;
    public Dictionary<int, QuestionResultData> resultList = new Dictionary<int,QuestionResultData>();
}
public class QuestionResultData
{
    public int questionID;
   
    public int rightOptionIndex; //正确答案索引,0代表A，1代表B，2代表C，3代表D
    public string guid; //id
    public string questionTitle; //问题标题
    public string questionCoutent; //问题内容
    public string optionAContent; //选项A内容
    public string optionBContent;
    public string optionCContent;
    public string optionDContent;
    public List<string> optionAList; //选择A的学生列表
    public List<string> optionBList;
    public List<string> optionCList;
    public List<string> optionDList;
    public List<string> optionUList; //未选择的学生列表
}

public class AskedList
{
    public int listID;
    public string listTitle;
    public Dictionary<int, AskedQuestionData> askedList = new Dictionary<int, AskedQuestionData>();
}

public class AskedQuestionData
{
    public AskedQuestionData(int QuestionID, string QuestionTitle, string QuestionCoutent, string OptionAContent, string OptionBContent, string OptionCContent, string OptionDContent)
    {
        questionID = QuestionID;
        questionTitle = QuestionTitle;
        questionContent = QuestionCoutent;
        optionAContent = OptionAContent;
        optionBContent = OptionBContent;
        optionCContent = OptionCContent;
        optionDContent = OptionDContent;
    }
    public AskedQuestionData()
    {

    }
    public int questionID;
    public int rightOptionIndex; //正确答案索引,0代表A，1代表B，2代表C，3代表D
    public bool hasSend; //已发送
    public string guid;
    public string questionTitle;
    public string questionContent;
    public string optionAContent;
    public string optionBContent;
    public string optionCContent;
    public string optionDContent;
}

public class VitoPluginQuestionManager : MonoBehaviour {
    public List<JsonData> QuestionList = new List<JsonData>();
    public List<JsonData> AnswerList = new List<JsonData>();

    public System.Action<JsonData> OnShowQuestion;
    /// <summary>
    /// 答题结果
    /// </summary>
    [HideInInspector]
    public List<QuestionResultData> QuestionResultList = new List<QuestionResultData>();

    /// <summary>
    /// 题集内容
    /// </summary>
    public List<AskedQuestionData> QuestionContentList = new List<AskedQuestionData>();

    public Dictionary<int,AskedList> AskedLists = new Dictionary<int,AskedList>();
    public Dictionary<int, ResultList> ResultLists = new Dictionary<int, ResultList>();
    

    private Dictionary<string, AskedQuestionData> HistoryQuestionContentMap = new Dictionary<string, AskedQuestionData>();
    public static VitoPluginQuestionManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }
    Queue<JsonData> receivedQuestions = new Queue<JsonData>();
    JsonData curQuestion;
    bool nextQuestion = true;
    public void AddQuestions(JsonData jd)
    {
        receivedQuestions.Enqueue(jd);
    }
    void ShowNextQuestion()
    {
        if(receivedQuestions.Count>0)
        {
            curQuestion = receivedQuestions.Dequeue();
            if(OnShowQuestion!=null)
            {
                OnShowQuestion(curQuestion);
            }
        }
    }

    public  void CheckAnswer(int index)
    {
        curQuestion["option1count"] = index == 1 ? 1 : 0;
        curQuestion["option2count"] = index == 2 ? 1 : 0;
        curQuestion["option3count"] = index == 3 ? 1 : 0;
        curQuestion["option4count"] = index == 4 ? 1 : 0;
        VitoPlugin.RequestActionEvent("SubmitQuestionAnswer",curQuestion.ToJson());
        StartCoroutine(INextQuestion(false));
    }

    IEnumerator INextQuestion(bool preIsRight)
    {
        if(preIsRight)
        {

        }else
        {

        }
        float timer = 0;
        float sumTime = 2;
        while (timer < sumTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        nextQuestion = true;
        yield return null;
    }
    void Update()
    {
        if(nextQuestion&&receivedQuestions.Count>0)
        {
            nextQuestion = false;
            ShowNextQuestion();
        }
    }

    public void LoadHistoryQuestionData()
    {
        string historyFileName = "QuestionContentData.txt";
        string path = Application.streamingAssetsPath + "/" + historyFileName;
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            byte[] byData = new byte[file.Length];
            file.Read(byData, 0, (int)file.Length);

            string content = Encoding.UTF8.GetString(byData);
            //Debug.Log(content);
             AskedLists = JsonConvert.DeserializeObject<Dictionary<int, AskedList>>(content);      
            foreach(var list in AskedLists.Values)
            {
                foreach(var data in list.askedList.Values)
                {
                    data.hasSend = false;
                }
            }    
        }
    }
    public void WriteHistoryQuestionData()
    {
        string historyFileName = "QuestionContentData.txt";
        string path = Application.streamingAssetsPath + "/" + historyFileName;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        {
            FileStream file = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            string content = JsonConvert.SerializeObject(AskedLists);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
    }


    void Start()
    {
        if(VitoPlugin.CT==CtrlType.Admin)
        {
            LoadHistoryQuestionData();
        }
        VitoPlugin.RegisterActionEvent("SubmitQuestionAnswer", (string type, string content, string deviceid) => {
            if (VitoPlugin.CT == CtrlType.Admin)
            {
                SetAnswerList(content, deviceid);
            }
        });
    }


    /// <summary>
    /// 保存或发送问题内容
    /// </summary>
    /// <param name="data"> 问题内容</param>
    /// <param name="justSave">true 只是保存到本地，不发送 </param>
    public void AskQuestionAction(int askedListIndex, AskedQuestionData data, bool justSave = false)
    {
        if(AskedLists.ContainsKey(askedListIndex))
        {
            if(AskedLists[askedListIndex].askedList.ContainsKey(data.questionID))
            {
                AskedLists[askedListIndex].askedList[data.questionID] = data;
            }
            else
            {
                AskedLists[askedListIndex].askedList.Add(data.questionID,data);
            }
        }
                
        if(!justSave)
        {
            data.hasSend = true;
            JsonData jd = new JsonData();
            jd["guid"] = data.guid;
            jd["title"] = data.questionTitle;
            jd["content"] = data.questionContent;
            jd["option1"] = data.optionAContent;
            jd["option2"] = data.optionBContent;
            jd["option3"] = data.optionCContent;
            jd["option4"] = data.optionDContent;
            jd["rightOption"] = data.rightOptionIndex+1;
            jd["option1count"] = 0;
            jd["option2count"] = 0;
            jd["option3count"] = 0;
            jd["option4count"] = 0;
            VitoPlugin.RequestActionEvent("AskQuestion", jd.ToJson());
        }
        WriteHistoryQuestionData();
    }

    public void SetAnswerList(string contont, string deviceid)
    {
        QuestionResultData resultData = null;
        JsonData jd = JsonMapper.ToObject(contont);
        UserInfoData au = UserInfoManager.instance.GetUserInfoWithDeviceId(deviceid);        

        string guid = jd["guid"].ToString();
        string[] guids = guid.Split('_');
        int listIndex = 0;
        System.Int32.TryParse(guids[0], out listIndex);
        int questionIndex = 0;
        System.Int32.TryParse(guids[1], out questionIndex);
        AskedQuestionData qjd = GetQuestionByGuid(listIndex,questionIndex);
        resultData = GetQeustionResultData(listIndex,questionIndex);
        if(resultData==null)
        {
            resultData = new QuestionResultData();
            resultData.guid = guid;
            resultData.optionAContent = qjd.optionAContent;
            resultData.optionAList = new List<string>();
            resultData.optionBContent = qjd.optionBContent;
            resultData.optionBList = new List<string>();
            resultData.optionCContent = qjd.optionCContent;
            resultData.optionCList = new List<string>();
            resultData.optionDContent = qjd.optionDContent;
            resultData.optionDList = new List<string>();
            resultData.rightOptionIndex = qjd.rightOptionIndex;
            resultData.questionTitle = qjd.questionTitle;
            resultData.questionCoutent = qjd.questionContent;
            resultData.questionID = qjd.questionID;
            if(ResultLists.ContainsKey(listIndex))
            {
                ResultLists[listIndex].resultList.Add(resultData.questionID, resultData);
                
            }
            else
            {
                ResultList tempResultList = new ResultList();
                tempResultList.listID = listIndex;
                tempResultList.listTitle = AskedLists[listIndex].listTitle;
                tempResultList.resultList = new Dictionary<int, QuestionResultData>();
                tempResultList.resultList.Add(resultData.questionID, resultData);
                ResultLists.Add(listIndex,tempResultList);
            }                                 
        }

        if((int)jd["option1count"]>0)
        {
            resultData.optionAList.Add(au.name);
        }
        if ((int)jd["option2count"] > 0)
        {
            resultData.optionBList.Add(au.name);
        }
        if ((int)jd["option3count"] > 0)
        {
            resultData.optionCList.Add(au.name);
        }
        if ((int)jd["option4count"] > 0)
        {
            resultData.optionDList.Add(au.name);
        }
    }

    QuestionResultData GetQeustionResultData(int listIndex,int questionIndex)
    {
       if(ResultLists.ContainsKey(listIndex))
        {
            if(ResultLists[listIndex].resultList.ContainsKey(questionIndex))
            {
                return ResultLists[listIndex].resultList[questionIndex];
            }
        }

        return null;
    }

    AskedQuestionData GetQuestionByGuid(int listIndex,int questionIndex)
    {
        

        if(AskedLists.ContainsKey(listIndex))
        {
            if(AskedLists[listIndex].askedList.ContainsKey(questionIndex))
            {
                return AskedLists[listIndex].askedList[questionIndex];
            }
        }
        return null;
    }



}
