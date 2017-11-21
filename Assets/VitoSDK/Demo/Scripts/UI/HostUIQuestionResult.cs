using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using LitJson;



public class HostUIQuestionResult : MonoBehaviour {
    public Text txtFieldTitle;
    public Text txtFieldContent;
    public Text txtFieldOptionA;
    public Text txtFieldOptionB;
    public Text txtFieldOptionC;
    public Text txtFieldOptionD;
    public Button btnSend;

    public Transform optionAParent;
    public Transform optionBParent;
    public Transform optionCParent;
    public Transform optionDParent;
    public Transform optionUParent;

    public Text txtACount;
    public Text txtBCount;
    public Text txtCCount;
    public Text txtDCount;
    public Text txtUCount;

    /// <summary>
    /// 每次界面刷新时获取到的答题结果
    /// </summary>
    private List<QuestionResultData> questionResultList = null;

    private int answerIndex = 0;
    

    private List<JsonData> AnswerList = null;
    void Start()
    {

    }
    private void OnEnable()
    {        
        btnSend.onClick.AddListener(SendQuestion);
        if(VitoPluginQuestionManager.instance!=null)
        {
            AnswerList = VitoPluginQuestionManager.instance.AnswerList;
            questionResultList = VitoPluginQuestionManager.instance.QuestionResultList;
            answerIndex = 0;
            ShowNextQuestion();
        }
    }
    private void ShowNextQuestion()
    {
        if(AnswerList!=null&&AnswerList.Count>answerIndex)
        {
            JsonData curData = AnswerList[answerIndex];
            txtFieldTitle.text = (string)curData["title"];
            txtFieldContent.text = (string)curData["content"];
            txtFieldOptionA.text = ((int)curData["option1count"]).ToString();
            txtFieldOptionB.text = ((int)curData["option2count"]).ToString();
            txtFieldOptionC.text = ((int)curData["option3count"]).ToString();
            txtFieldOptionD.text = ((int)curData["option4count"]).ToString();
            answerIndex++;
        }
    }

    private void OnDisable()
    {
        btnSend.onClick.RemoveListener(SendQuestion);
    }

    public void SendQuestion()
    {
        ShowNextQuestion();
    }
}
