using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;

public class CommonUIQuestion : MonoBehaviour
{
    public static CommonUIQuestion instance { get; set; }

    public Text txtContent;
    public Text txtTitle;
    public Text optionA;
    public Text optionB;
    public Text optionC;
    public Text optionD;

    public int rightOptionIndex = -1;
    public VitoVRInteractiveItem itemA;
    public VitoVRInteractiveItem itemB;
    public VitoVRInteractiveItem itemC;
    public VitoVRInteractiveItem itemD;
    public GameObject centerObject;

    void OnEnable()
    {
        itemA.OnClick += SelectA;
        itemB.OnClick += SelectB;
        itemC.OnClick += SelectC;
        itemD.OnClick += SelectD;
    }

    void OnDisable()
    {
        itemA.OnClick -= SelectA;
        itemB.OnClick -= SelectB;
        itemC.OnClick -= SelectC;
        itemD.OnClick -= SelectD;
    }
    private float lastAnswerTime = 0;
    void SelectA()
    {

        Answer(1);
    }
    void SelectB()
    {

        Answer(2);
    }
    void SelectC()
    {

        Answer(3);
    }
    void SelectD()
    {

        Answer(4);
    }

    void Answer(int index)
    {
        if (Time.realtimeSinceStartup - lastAnswerTime > 1.5f)
        {
            lastAnswerTime = Time.realtimeSinceStartup;
        }
        else
        {
            return;
        }
        VitoPluginQuestionManager.instance.CheckAnswer(index);
        if (index == rightOptionIndex)
        {
            SetColor(Color.green);
        }
        else
        {
            SetColor(Color.red);
            switch (rightOptionIndex)
            {
                case 1:
                    optionA.color = Color.green;
                    break;
                case 2:
                    optionB.color = Color.green;
                    break;
                case 3:
                    optionC.color = Color.green;
                    break;
                case 4:
                    optionD.color = Color.green;
                    break;
            }
        }
        Invoke("Hide", 1.5f);
    }

    void SetColor(Color color)
    {
        txtContent.color = color;
        txtTitle.color = color;
        optionA.color = color;
        optionB.color = color;
        optionC.color = color;
        optionD.color = color;
    }

    void Hide()
    {
        SetColor(Color.white);
        centerObject.SetActive(false);
        if (SceneManager.GetActiveScene().name != "JapEatery")
            MicController.instance.enabled = true;
    }
    void Awake()
    {
        instance = this;
        centerObject.SetActive(false);
    }
    void Start()
    {
        VitoPluginQuestionManager.instance.OnShowQuestion += Init;
    }
    void OnDestroy()
    {
        VitoPluginQuestionManager.instance.OnShowQuestion -= Init;
    }

    public void Init(JsonData jd)
    {
        UI3DFollowCamera.instance.ResetPos();
        centerObject.SetActive(true);
        MicController.instance.enabled = false;
        txtTitle.text = jd["title"].ToString();
        txtContent.text = jd["content"].ToString();
        optionA.text = jd["option1"].ToString();
        optionB.text = jd["option2"].ToString();
        optionC.text = jd["option3"].ToString();
        optionD.text = jd["option4"].ToString();
        rightOptionIndex = System.Int32.Parse(jd["rightOption"].ToString());
    }

}
