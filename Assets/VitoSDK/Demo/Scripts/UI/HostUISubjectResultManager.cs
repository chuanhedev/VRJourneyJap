using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostUISubjectResultManager : MonoBehaviour {

    public static HostUISubjectResultManager instance;

    public GameObject List, Result;
    public VitoPluginQuestionManager question { get { return VitoPluginQuestionManager.instance; } private set { } }

    public Text title, optionA, optionB, optionC, optionD/*, optionU*/;
    Text[] options;
    public Text listTitle;
    public Text[] listContents;
    public HostUISubjectResult[] aSubject;
    public List<HostUISubjectResultList> SubjectResultLists;
    public List<HostUISubjectListData> SubjectResultListsData;
    public GameObject ListPrefab;
    public Transform ListPos;
    int subid;
    public Transform gridA, gridB, gridC, gridD, gridU;
    public GameObject namePrefab;
    public GameObject listMiddle;
    public int nowSub, nowList;
    public Image imageA, imageB, imageC, imageD;
    void Awake()
    {
        instance = this;
        options = new Text[4];
        options[0] = optionA;
        options[1] = optionB;
        options[2] = optionC;
        options[3] = optionD;
        //Bingzhuangtu(0, 30, 50, 20);
    }
    private void OnEnable()
    {
        Initialization2();
        //ReadData();
        //ReadLists();
        //List.SetActive(true);
        //Result.SetActive(false);
        //if (SubjectResultLists.Count <= 0)
        //{
        //    listMiddle.SetActive(false);
        //}
    }

    GameObject  Instantiate(GameObject prefab,Transform parent)
    {
        GameObject  go= Instantiate(prefab);
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one;
        return go;
    }

    public void ShowResult(HostUISubjectData data)
    {
        List.SetActive(false);
        Result.SetActive(true);
        title.text = data.Title;
        for (int i = 0; i < data.optionAList.Count; i++) 
        {
            GameObject name = Instantiate(namePrefab, gridA);
            name.GetComponentInChildren<Text>().text = data.optionAList[i];
        }
        for (int i = 0; i < data.optionBList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridB);
            name.GetComponentInChildren<Text>().text = data.optionBList[i];
        }
        for (int i = 0; i < data.optionCList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridC);
            name.GetComponentInChildren<Text>().text = data.optionCList[i];
        }
        for (int i = 0; i < data.optionDList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridD);
            name.GetComponentInChildren<Text>().text = data.optionDList[i];
        }
        for (int i = 0; i < data.optionUList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridU);
            name.GetComponentInChildren<Text>().text = data.optionUList[i];
        }
    }
    public void ShowList(HostUISubjectListData list)
    {
        listTitle.transform.parent.gameObject.SetActive(true);
        listMiddle.SetActive(true);
        List.SetActive(true);
        Result.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            listContents[i].GetComponent<Text>().text = list.Subjects[i].Title;
        }
        for (int i = 0; i < 10; i++)
        {
            if (list.Subjects[i] != null)
            {
                aSubject[i].data = list.Subjects[i];
            }
        }
    }
    public void ReadLists()
    {
        SubjectResultLists = new List<HostUISubjectResultList>();
        for (int i = 0; i < SubjectResultListsData.Count; i++)
        {
            List<HostUISubjectData> subjects = SubjectResultListsData[i].Subjects;
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
            newList.GetComponent<HostUISubjectList>().data = SubjectResultListsData[i];
            SubjectResultLists.Add(newList.GetComponent<HostUISubjectResultList>());
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
    //void ReadData()
    //{
    //    SubjectResultListsData = new List<HostUISubjectListData>();
    //    int i = 0;
    //    int j = 0;
    //    while (question.QuestionResultList.Count > i)
    //    {
    //        List<HostUISubjectData> subjects = new List<HostUISubjectData>();
    //        while (question.QuestionResultList[i] != null && question.QuestionResultList[i].listID == j)
    //        {
    //            subjects.Add(new HostUISubjectData(question.QuestionResultList[i].questionID, question.QuestionResultList[i].questionTitle, question.QuestionResultList[i].questionCoutent, question.QuestionResultList[i].optionAContent, question.QuestionResultList[i].optionBContent, question.QuestionResultList[i].optionCContent, question.QuestionResultList[i].optionDContent));
    //            i++;
    //        }
    //        j++;
    //        SubjectResultListsData.Add(new HostUISubjectListData(question.QuestionResultList[i - 1].listTitle, question.QuestionResultList[i - 1].listID, subjects));
    //    }
    //}
    public void Backward()
    {
        if (subid < SubjectResultLists.Count - 1)
        {
            SubjectResultLists[subid].gameObject.SetActive(false);
            subid++;
            SubjectResultLists[subid].gameObject.SetActive(true);
        }
    }
    public void Forkward()
    {
        if (subid > 0)
        {
            SubjectResultLists[subid].gameObject.SetActive(false);
            subid--;
            SubjectResultLists[subid].gameObject.SetActive(true);
        }
    }
    public void Back2List()
    {
        List.SetActive(true);
        Result.SetActive(false);
    }
    public void ShowList2()
    {
        listTitle.transform.parent.gameObject.SetActive(true);
        listMiddle.SetActive(true);
        List.SetActive(true);
        Result.SetActive(false);
        ResultList resultList = new ResultList();
        question.ResultLists.TryGetValue(nowList, out resultList);
        listTitle.text = resultList.listTitle;
        foreach(var data in resultList.resultList.Keys)
        {
            listContents[data].GetComponent<Text>().text = resultList.resultList[data].questionTitle;
        }
    }


    public void ShowSub2(int subID)
    {
        ResultList resultList = new ResultList();
        question.ResultLists.TryGetValue(nowList, out resultList);
        options[resultList.resultList[subID].rightOptionIndex].color = Color.green;
        title.text = resultList.resultList[subID].questionTitle;
        int countA = 0, countB = 0, countC = 0, countD = 0;
        VitoSDKUtility.DestroyChildren(gridA);
        
        for (int i = 0; i < resultList.resultList[subID].optionAList.Count; i++) 
        {
            GameObject name = Instantiate(namePrefab, gridA);
            name.GetComponentInChildren<Text>().text = resultList.resultList[subID].optionAList[i];
            countA++;
        }
        optionA.text = "A" + countA;
        VitoSDKUtility.DestroyChildren(gridB);
        
        for (int i = 0; i < resultList.resultList[subID].optionBList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridB);
            name.GetComponentInChildren<Text>().text = resultList.resultList[subID].optionBList[i];
            countB++;
        }
        optionB.text = "B" + countB;
        VitoSDKUtility.DestroyChildren(gridC);
        for (int i = 0; i < resultList.resultList[subID].optionCList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridC);
            name.GetComponentInChildren<Text>().text = resultList.resultList[subID].optionCList[i];
            countC++;
        }
        optionC.text = "C" + countC;
        VitoSDKUtility.DestroyChildren(gridD);
        for (int i = 0; i < resultList.resultList[subID].optionDList.Count; i++)
        {
            GameObject name = Instantiate(namePrefab, gridD);
            name.GetComponentInChildren<Text>().text = resultList.resultList[subID].optionDList[i];
            countD++;
        }
        optionD.text = "D" + countD;
        //count = 0;
        //gridU.DestroyChildren();
        //for (int i = 0; i < resultList.resultList[subID].optionUList.Count; i++)
        //{
        //    GameObject name = Instantiate(namePrefab, gridU);
        //    name.GetComponentInChildren<Text>().text = resultList.resultList[subID].optionUList[i];
        //    count++;
        //}
        //optionU.text = "U" + count;
        int all = (countA + countB + countC + countD) / 360;
        if(all==0)
        {
            all = 1;
        }
        int a = countA / all;
        int b = countB / all;
        int c = countC / all;
        int d = countD / all;
        //Bingzhuangtu(a, b, c, d);

        nowSub = subID;
        List.SetActive(false);
        Result.SetActive(true);
    }

    void Bingzhuangtu(float a, float b, float c, float d)
    {
        if (a == 0)
        {
            imageA.gameObject.SetActive(false);
        }
        else
        {
            imageA.gameObject.SetActive(true);
        }
        if (b == 0)
        {
            imageB.gameObject.SetActive(false);
        }
        else
        {
            imageB.gameObject.SetActive(true);
        }
        if (c == 0)
        {
            imageC.gameObject.SetActive(false);
        }
        else
        {
            imageC.gameObject.SetActive(true);
        }
        if (d == 0)
        {
            imageD.gameObject.SetActive(false);
        }
        else
        {
            imageD.gameObject.SetActive(true);
        }
        imageA.transform.localEulerAngles = new Vector3(0, 0, 0);
        imageA.GetComponentInChildren<Text>().transform.eulerAngles = new Vector3(0, 0, 0);
        imageB.transform.localEulerAngles = new Vector3(0, 0, -a);
        imageB.GetComponentInChildren<Text>().transform.eulerAngles = new Vector3(0, 0, 0);
        imageC.transform.localEulerAngles = new Vector3(0, 0, -(a + b));
        imageC.GetComponentInChildren<Text>().transform.eulerAngles = new Vector3(0, 0, 0);
        imageD.transform.localEulerAngles = new Vector3(0, 0, -(a + b + c));
        imageD.GetComponentInChildren<Text>().transform.eulerAngles = new Vector3(0, 0, 0);
        imageA.fillAmount = a / 360;
        imageB.fillAmount = b / 360;
        imageC.fillAmount = c / 360;
        imageD.fillAmount = d / 360;
    }

    public void Initialization2()
    {
        ResultList resultList = new ResultList();
        question.ResultLists.TryGetValue(nowList, out resultList);
        if (resultList == null)
        {
            listTitle.transform.parent.gameObject.SetActive(false);
            listMiddle.SetActive(false);
        }
        else
        {
            nowList = 0;
            ShowList2();
        }
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
    
}
