using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostUISubjectData
{
    public int SubjectID;
    public string Title;
    public string Content;
    public string OptionA;
    public string OptionB;
    public string OptionC;
    public string OptionD;
    public List<string> optionAList; //选择A的学生列表
    public List<string> optionBList;
    public List<string> optionCList;
    public List<string> optionDList;
    public List<string> optionUList; //未选择的学生列表
    public HostUISubjectData(int id,string title,string content,string a,string b,string c,string d)
    {
        SubjectID = id;
        Title = title;
        Content = content;
        OptionA = a;
        OptionB = b;
        OptionC = c;
        OptionD = d;
        optionAList = new List<string>();
        optionBList = new List<string>();
        optionCList = new List<string>();
        optionDList = new List<string>();
        optionUList = new List<string>();
    }
}

public class HostUISubjectListData
{
    public string ListTitle;
    public int ListID;
    public List<HostUISubjectData> Subjects;
    public HostUISubjectListData(string title,int id, List<HostUISubjectData> subjects)
    {
        ListTitle = title;
        ListID = id;
        Subjects = subjects;
    }
}


public class HostUISubjectManager : MonoBehaviour {

    public VitoPluginQuestionManager question { get { return VitoPluginQuestionManager.instance; }  private set { } }

    public static HostUISubjectManager instance;

    HostUIGetSubjectData data;

    public GameObject SubList, Subject;
    public List<HostUISubjectListData> SubjectListsData;
    public List<HostUISubjectList> SubjectLists;
    public HostUISubject[] aSubject;
    public GameObject ListPrefab;
    public Transform ListPos;
    public InputField subTitle, content, optionA, optionB, optionC, optionD;
    public InputField listTitle;
    public Text[] listContents;
    int subid;
    HostUISubject nowSubjectData;
    public GameObject listMiddle;
    public int nowList, nowSub;
    // Use this for initialization

    public Toggle[] trueOption;
    int trueIndex;
    public Button btnSend;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    void Start () {
        //Initialization2();
        //ShowList(0);
    }
    private void OnEnable()
    {
        Initialization2();
        //ReadData();
        ////Initialization();
        ////SetSubjects(SubjectListsData[0].Subjects);
        //ReadLists();
        //SubList.SetActive(true);
        //Subject.SetActive(false);
        //if (SubjectLists.Count <= 0)
        //{
        //    listMiddle.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update () {
		
	}
    //public void List2Sub(int subID)
    //{
    //    SubList.SetActive(false);
    //    Subject.SetActive(true);
    //    //test
    //    nowSubjectData = aSubject[subID].ShowSubject(subTitle, content, optionA, optionB, optionC, optionD);
    //    //Subject.GetComponent<HostUISubject>().ShowSubject(subTitle, content, optionA, optionB, optionC, optionD);
    //}
    //public void Sub2List()
    //{
    //    SubList.SetActive(true);
    //    Subject.SetActive(false);
    //    SubList.GetComponent<HostUISubjectList>().ShowList(listContents);
    //}
    //public void CreateList()
    //{
    //    List<HostUISubjectData> subjects = new List<HostUISubjectData>();
    //    for (int i = 0; i < 10; i++)
    //    {
    //        subjects.Add(new HostUISubjectData(i, "请录入题目信息", "", "", "", "", ""));
    //    }
    //    HostUISubjectListData newListData = new HostUISubjectListData("未命名", SubjectListsData.Count, subjects);
    //    GameObject newList = Instantiate(ListPrefab);
    //    newList.transform.position = ListPos.position;
    //    newList.GetComponent<RectTransform>().offsetMax = new Vector2(newList.GetComponent<RectTransform>().offsetMax.x, newList.GetComponent<RectTransform>().offsetMax.y);
    //    newList.GetComponent<RectTransform>().offsetMin = new Vector2(newList.GetComponent<RectTransform>().offsetMin.x, newList.GetComponent<RectTransform>().offsetMin.y);
    //    newList.transform.rotation = ListPos.rotation;
    //    newList.transform.parent = ListPos.parent;
    //    newList.transform.localScale = ListPos.localScale;
    //    newList.GetComponent<HostUISubjectList>().data = newListData;
    //    SubjectLists.Add(newList.GetComponent<HostUISubjectList>());
    //    SubjectListsData.Add(newList.GetComponent<HostUISubjectList>().data);
    //    ShowList(newListData);
    //    if (subid > 0)
    //    {
    //        SubjectLists[subid].gameObject.SetActive(false);
    //    }
    //    subid = SubjectLists.Count - 1;
    //    SubjectLists[subid].gameObject.SetActive(true);
    //    PreservationLists();
    //}
    public void ReadLists()
    {
        SubjectLists = new List<HostUISubjectList>();
        subid = 0;
        for (int i = 0; i < SubjectListsData.Count; i++)
        {
            List<HostUISubjectData> subjects = SubjectListsData[i].Subjects;
            GameObject newList = Instantiate(ListPrefab);
            newList.transform.position = ListPos.position;
            //print(newList.GetComponent<RectTransform>().offsetMax.y);
            //print(ListPos.GetComponent<RectTransform>().offsetMax.y);
            newList.GetComponent<RectTransform>().offsetMax = new Vector2(newList.GetComponent<RectTransform>().offsetMax.x, newList.GetComponent<RectTransform>().offsetMax.y);
            newList.GetComponent<RectTransform>().offsetMin = new Vector2(newList.GetComponent<RectTransform>().offsetMin.x, newList.GetComponent<RectTransform>().offsetMin.y);
            newList.transform.rotation = ListPos.rotation;
            newList.transform.parent = ListPos.parent;
            newList.transform.localScale = ListPos.localScale;
            //ListPos.position += Vector3.right * 240;
            newList.GetComponent<HostUISubjectList>().data = SubjectListsData[i];
            SubjectLists.Add(newList.GetComponent<HostUISubjectList>());
            if (i > 0)
            {
                newList.SetActive(false);
            }
            else
            {
                newList.SetActive(true);
            }
        }
    }

    public void ShowList(HostUISubjectListData list)
    {
        listMiddle.SetActive(true);
        SubList.SetActive(true);
        Subject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            listContents[i].GetComponent<Text>().text = list.Subjects[i].Title;
        }
    }

    public void ShowList2()
    {
        listTitle.gameObject.SetActive(true);
        listMiddle.SetActive(true);
        SubList.SetActive(true);
        Subject.SetActive(false);
        AskedList askedList = new AskedList();
        question.AskedLists.TryGetValue(nowList, out askedList);
        listTitle.text = askedList.listTitle;
        for (int i = 0; i < 10; i++)
        {
            listContents[i].GetComponent<Text>().text = askedList.askedList[i].questionTitle;
        }
    }

    public void PreservationListName2()
    {
        AskedList askedList = new AskedList();
        question.AskedLists.TryGetValue(nowList, out askedList);
        askedList.listTitle = listTitle.text;
        question.WriteHistoryQuestionData();
    }
    public void ShowSub2(int subID)
    {
        AskedList askedList = new AskedList();
        question.AskedLists.TryGetValue(nowList, out askedList);
        subTitle.text = askedList.askedList[subID].questionTitle;
        content.text = askedList.askedList[subID].questionContent;
        optionA.text = askedList.askedList[subID].optionAContent;
        optionB.text = askedList.askedList[subID].optionBContent;
        optionC.text = askedList.askedList[subID].optionCContent;
        optionD.text = askedList.askedList[subID].optionDContent;
        nowSub = subID;
        btnSend.gameObject.SetActive(!askedList.askedList[subID].hasSend);
        trueOption[askedList.askedList[nowSub].rightOptionIndex].isOn = true;
        for (int i = 0; i < 4; i++)
        {
            if (i == askedList.askedList[nowSub].rightOptionIndex) 
            {
                trueOption[i].isOn = true;
            }
            else
            {
                trueOption[i].isOn = false;
            }
        }
        SubList.SetActive(false);
        Subject.SetActive(true);
    }


    public void Initialization2()
    {
        AskedList askedList = new AskedList();
        question.AskedLists.TryGetValue(nowList, out askedList);
        if (askedList == null) 
        {
            listMiddle.SetActive(false);
            listTitle.gameObject.SetActive(false);
        }
        else
        {
            nowList = 0;
            ShowList2();
        }
    }
    public void CreateList2()
    {
        AskedList newList = new AskedList();
        newList.listID = question.AskedLists.Count;
        newList.listTitle = "未命名";
        newList.askedList = new Dictionary<int, AskedQuestionData>();
        for (int i = 0; i < 10; i++)
        {
            newList.askedList.Add(i,new AskedQuestionData(i, "请输入题目名称", "请输入题目内容", "选项A内容", "选项B内容", "选项C内容", "选项D内容"));
        }
        question.AskedLists.Add(newList.listID, newList);
        nowList = newList.listID;
        ShowList2();
    }
    public void Backward2()
    {
        if (nowList > 0)
        {
            nowList--;
            ShowList2();
        }
    }
    public void Forkward2()
    {
        if (nowList < question.AskedLists.Count - 1)
        {
            nowList++;
            ShowList2();
        }
    }

    //保存问题
    public void PreservationSubject2()
    {
        AskedList askedList = new AskedList();
        question.AskedLists.TryGetValue(nowList, out askedList);
        askedList.askedList[nowSub].questionTitle = subTitle.text;
        askedList.askedList[nowSub].questionContent = content.text;
        askedList.askedList[nowSub].optionAContent = optionA.text;
        askedList.askedList[nowSub].optionBContent = optionB.text;
        askedList.askedList[nowSub].optionCContent = optionC.text;
        askedList.askedList[nowSub].optionDContent = optionD.text;
        for (int i = 0; i < 4; i++)
        {
            if (trueOption[i].isOn)
            {
                askedList.askedList[nowSub].rightOptionIndex = i;
                break;
            }
        }

        question.WriteHistoryQuestionData();
        ShowList2();
    }

    //public void WriteLists()
    //{

    //}
    //public void SetSubjects(List<HostUISubjectData> subjectData)
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        if (subjectData[i] != null)
    //        {
    //            aSubject[i].data = subjectData[i];
    //        }
    //    }
    //}
    //public void SetSubjects2(int listID, int subID)
    //{
    //    AskedList askedList = new AskedList();
    //    question.AskedLists.TryGetValue(listID, out askedList);
    //    for (int i = 0; i < 10; i++)
    //    {
    //        aSubject[i].data=askedList.askedList[listID].
    //    }
    //}
    //public void Initialization()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        if (SubjectListsData[0].Subjects[i] != null)
    //        {
    //            aSubject[i].data = SubjectListsData[0].Subjects[i];
    //        }
    //    }
    //    //SubjectListsData = new List<HostUISubjectListData>();
    //    //SubjectLists = new List<HostUISubjectList>();
    //    //List<HostUISubjectData> subjects = new List<HostUISubjectData>();
    //    //subjects.Add(new HostUISubjectData("adsjio", "asdjizcxhguia", "aaa", "bbb", "ccc", "ddd"));
    //    //HostUISubjectListData newListData = new HostUISubjectListData("未命名", SubjectListsData.Count, subjects);
    //    //aSubject[0].data = subjects[0];
    //    ////SubjectLists[0].ShowList(listContents);
    //}
    //public void ChooseList(int id)
    //{
    //    for (int i = 0; i < aSubject.Length; i++)
    //    {
    //        aSubject[i].data = SubjectListsData[id].Subjects[i];
    //    }
    //}
    //[ContextMenu("ReadData")]
    //void ReadData()
    //{
    //    //data = GetComponent<HostUIGetSubjectData>();
    //    //SubjectListsData = new List<HostUISubjectListData>();
    //    //int i = 0;
    //    //int j = 0;
    //    //while (data._SubjectDataList.Count > i)
    //    //{
    //    //    List<HostUISubjectData> subjects = new List<HostUISubjectData>();
    //    //    while (data.getSubjectData(i) != null && data.getSubjectData(i).ListID == j) 
    //    //    {
    //    //        subjects.Add(new HostUISubjectData(data.getSubjectData(i).Title, data.getSubjectData(i).Content, data.getSubjectData(i).OptionA, data.getSubjectData(i).OptionB, data.getSubjectData(i).OptionC, data.getSubjectData(i).OptionD));
    //    //        i++;
    //    //    }
    //    //    j++;
    //    //    SubjectListsData.Add(new HostUISubjectListData(data.getSubjectData(i - 1).ListTitle, data.getSubjectData(i - 1).ListID, subjects));
    //    //    //for (int k = 0; k < 10; k++)
    //    //    //{
    //    //    //    print(SubjectListsData[j - 1].Subjects[k].OptionA);
    //    //    //}
    //    //}
    //    SubjectListsData = new List<HostUISubjectListData>();
    //    int i = 0;
    //    int j = 0;
    //    while (question.QuestionContentList.Count > i)
    //    {
    //        List<HostUISubjectData> subjects = new List<HostUISubjectData>();
    //        while (question.QuestionContentList[i] != null && question.QuestionContentList[i].listID == j)
    //        {
    //            subjects.Add(new HostUISubjectData(question.QuestionContentList[i].questionID, question.QuestionContentList[i].questionTitle, question.QuestionContentList[i].questionContent, question.QuestionContentList[i].optionAContent, question.QuestionContentList[i].optionBContent, question.QuestionContentList[i].optionCContent, question.QuestionContentList[i].optionDContent));
    //            i++;
    //        }
    //        j++;
    //        SubjectListsData.Add(new HostUISubjectListData(question.QuestionContentList[i - 1].listTitle, question.QuestionContentList[i - 1].listID, subjects));
    //    }
    //}
    //public void Backward()
    //{
    //    if (subid < SubjectLists.Count - 1) 
    //    {
    //        SubjectLists[subid].gameObject.SetActive(false);
    //        subid++;
    //        SubjectLists[subid].gameObject.SetActive(true);
    //    }
    //}
    //public void Forkward()
    //{
    //    if (subid > 0)
    //    {
    //        SubjectLists[subid].gameObject.SetActive(false);
    //        subid--;
    //        SubjectLists[subid].gameObject.SetActive(true);
    //    }
    //}
    //public void PreservationSubject()
    //{
    //    nowSubjectData.WriteSubject(subTitle, content, optionA, optionB, optionC, optionD);
       
    //    PreservationLists();
    //    SubList.SetActive(true);
    //    Subject.SetActive(false);
    //}
    //public void PreservationLists()
    //{
    //    //int k = 0;
    //    List<AskedQuestionData> QuestionContentList = new List<AskedQuestionData>();
    //    for (int i = 0; i < SubjectListsData.Count; i++)
    //    {
    //        for (int j = 0; j < SubjectListsData[i].Subjects.Count; j++)
    //        {
    //            QuestionContentList.Add(new AskedQuestionData(SubjectListsData[i].ListTitle, SubjectListsData[i].ListID, SubjectListsData[i].Subjects[j].SubjectID, SubjectListsData[i].Subjects[j].Title, SubjectListsData[i].Subjects[j].Content, SubjectListsData[i].Subjects[j].OptionA, SubjectListsData[i].Subjects[j].OptionB, SubjectListsData[i].Subjects[j].OptionC, SubjectListsData[i].Subjects[j].OptionD));
    //            //k++;
    //        }
    //    }
    //    question.QuestionContentList = QuestionContentList;
    //}
}
