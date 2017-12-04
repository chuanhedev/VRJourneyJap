using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MicStudentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    Color enterColor;
    [SerializeField]
    Color clickedColor;
    Image bg;
    Text studentInfo;
    List<Quaternion> qList = new List<Quaternion>();
    string deviceId;
    string panoPath;
    string micTimer;
    bool isClicked;//已经点击

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //播放录音
        if (PlayBackController.instance.StartPlayBack(deviceId))
        {
            CheckUI();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SetBgColor(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SetBgColor(false);
    }

    void Awake()
    {
        Find();
    }

    void CheckUI()
    {
        HostUIManager hostUIManager = GetComponentInParent<HostUIManager>();
        hostUIManager.MicToggleEnable(true);
        hostUIManager.StudentMicListEnable(false);
        isClicked = true;
        SetBgColor(false);
        FacadeManager._instance.UpdatePano(panoPath, false,false);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="panoPath"></param>
    /// <param name="qList"></param>
    public void InitData(string deviceId, string panoPath, string micTimer, List<Quaternion> qList)
    {
        this.deviceId = deviceId;
        this.panoPath = panoPath;
        this.micTimer = micTimer;
        this.qList = qList;

        SetStudentInfo();
    }

    void SetBgColor(bool isEnter)
    {
        if (isEnter)
            bg.color = enterColor;
        else
        {
            if (isClicked)
            {
                bg.color = clickedColor;
            }
            else
            {
                bg.color = Color.white;
            }
        }
    }

    void Find()
    {
        bg = GetComponent<Image>();
        studentInfo = GetComponentInChildren<Text>();
    }

    void SetStudentInfo()
    {
        UserInfoData userInfoData = UserInfoManager.instance.getHistoryUserData(deviceId);

        string userName;
        if (userInfoData != null)
        {
            userName = userInfoData.name;
        }
        else
        {
            userName = "未录入";
        }
        studentInfo.text = userName + " 时长：" + micTimer + "''";
    }
}
