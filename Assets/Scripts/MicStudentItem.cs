using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MicStudentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    Color color;
    Image bg;
    Text studentInfo;
    List<Quaternion> qList = new List<Quaternion>();
    string deviceId;
    string scene;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //播放录音
        if (PlayBackController.instance.StartPlayBack(deviceId))
        {
            HostUIManager hostUIManager = GetComponentInParent<HostUIManager>();
            hostUIManager.MicToggleEnable(true);
            hostUIManager.StudentMicListEnable(false);
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

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="scene"></param>
    /// <param name="qList"></param>
    public void InitData(string deviceId, string scene, List<Quaternion> qList)
    {
        this.deviceId = deviceId;
        this.scene = scene;
        this.qList = qList;

        InitData();
    }

    void SetBgColor(bool isEnter)
    {
        if (isEnter)
            bg.color = color;
        else
            bg.color = Color.white;
    }

    void Find()
    {
        bg = GetComponent<Image>();
        studentInfo = GetComponentInChildren<Text>();
    }

    void InitData()
    {
        UserInfoData userInfoData = UserInfoManager.instance.GetUserInfoWithDeviceId(deviceId);

        if (userInfoData != null)
        {
            studentInfo.text = userInfoData.name;
        }
        else
        {
            studentInfo.text = "未录入";
        }
    }
}
