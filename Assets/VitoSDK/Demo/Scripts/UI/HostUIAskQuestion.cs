using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using LitJson;
namespace com.vito.plugin.demo
{
    public class HostUIAskQuestion : MonoBehaviour
    {

        public InputField txtFieldTitle;
        public InputField txtFieldContent;
        public InputField txtFieldOptionA;
        public InputField txtFieldOptionB;
        public InputField txtFieldOptionC;
        public InputField txtFieldOptionD;
        public Button btnSend;
        public Button btnWrite;
        public Button[] subjects;

        void Start()
        {
            //gameObject.SetActive(true);
        }
        private void OnEnable()
        {
            btnSend.onClick.AddListener(SendQuestion);
            btnWrite.onClick.AddListener(HostUISubjectManager.instance.PreservationSubject2);
            //for (int i = 0; i < subjects.Length; i++)
            //{
            //    subjects[i].onClick.AddListener(HostUISubjectManager.instance.ShowSub2(i))
            //}
        }
        private void OnDisable()
        {
            btnSend.onClick.RemoveListener(SendQuestion);
            btnWrite.onClick.RemoveListener(HostUISubjectManager.instance.PreservationSubject2);
        }

        public void Show(int id)
        {
            gameObject.SetActive(true);

        }

        public void SendQuestion()
        {
            AskedQuestionData questionData = new AskedQuestionData();
            questionData.guid = HostUISubjectManager.instance.nowList + "_" + HostUISubjectManager.instance.nowSub;
            questionData.questionID = HostUISubjectManager.instance.nowSub;
            questionData.questionTitle = txtFieldTitle.text;
            questionData.questionContent = txtFieldContent.text;
            questionData.optionAContent = txtFieldOptionA.text;
            questionData.optionBContent = txtFieldOptionB.text;
            questionData.optionCContent = txtFieldOptionC.text;
            questionData.optionDContent = txtFieldOptionD.text;
            for (int i = 0; i < HostUISubjectManager.instance.trueOption.Length; i++)
            {
                if (HostUISubjectManager.instance.trueOption[i].isOn)
                {
                    questionData.rightOptionIndex = i;
                    break;
                }
            }

            VitoPluginQuestionManager.instance.AskQuestionAction(HostUISubjectManager.instance.nowList, questionData);
            txtFieldTitle.text = "";
            txtFieldContent.text = "";
            txtFieldOptionA.text = "";
            txtFieldOptionB.text = "";
            txtFieldOptionC.text = "";
            txtFieldOptionD.text = "";
            gameObject.SetActive(false);
        }
        void Update()
        {
            if (txtFieldContent.isFocused || !string.IsNullOrEmpty(txtFieldContent.text))
            {
                txtFieldContent.placeholder.enabled = false;
            }
            else
            {
                txtFieldContent.placeholder.enabled = true;
            }

            if (txtFieldOptionA.isFocused || !string.IsNullOrEmpty(txtFieldOptionA.text))
            {
                txtFieldOptionA.placeholder.enabled = false;
            }
            else
            {
                txtFieldOptionA.placeholder.enabled = true;
            }

            if (txtFieldOptionB.isFocused || !string.IsNullOrEmpty(txtFieldOptionB.text))
            {
                txtFieldOptionB.placeholder.enabled = false;
            }
            else
            {
                txtFieldOptionB.placeholder.enabled = true;
            }
            if (txtFieldOptionC.isFocused || !string.IsNullOrEmpty(txtFieldOptionC.text))
            {
                txtFieldOptionC.placeholder.enabled = false;
            }
            else
            {
                txtFieldOptionC.placeholder.enabled = true;
            }
            if (txtFieldOptionD.isFocused || !string.IsNullOrEmpty(txtFieldOptionD.text))
            {
                txtFieldOptionD.placeholder.enabled = false;
            }
            else
            {
                txtFieldOptionD.placeholder.enabled = true;
            }
            if (txtFieldTitle.isFocused || !string.IsNullOrEmpty(txtFieldTitle.text))
            {
                txtFieldTitle.placeholder.enabled = false;
            }
            else
            {
                txtFieldTitle.placeholder.enabled = true;
            }
        }
    }

}

