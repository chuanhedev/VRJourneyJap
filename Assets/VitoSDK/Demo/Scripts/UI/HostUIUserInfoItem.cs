using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HostUIUserInfoItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler {

    public Text txtName;
    public Text txtNum;
    public Image imgIcon;
    public Image imgBg;
    public Image imgStatus;
    public Color mOverColor;
    public Text txtLevelInfo;
    public Image imgPower;
    private UserInfoData mUserInfoData;
    private HostUIUserListPanel mUserListPanel;
    public void Init(UserInfoData userInfo,HostUIUserListPanel panel)
    {
        userInfo.OnHMDStatusChange = this.OnHMDStatusChange;
        userInfo.OnPosStatusChange = this.OnPosStatusChange;
        userInfo.OnLevelStatusChange = this.OnLevelStatusChange;
        userInfo.onPowerChange = this.OnPowerChange;
        userInfo.OnRefresh = this.Refresh;
        mUserInfoData = userInfo;
        if(string.IsNullOrEmpty( mUserInfoData.name))
        {
            txtName.text = mUserInfoData.userid.ToString();
        }else
        {
            txtName.text = mUserInfoData.name.ToString();
        }
        if(string.IsNullOrEmpty(mUserInfoData.number))
        {
            txtNum.text = mUserInfoData.deviceid;
        }
        else
        {
            txtNum.text = mUserInfoData.number;
        }
        
        mUserListPanel = panel;
    }
    
    public void Refresh()
    {
        if (string.IsNullOrEmpty(mUserInfoData.name))
        {
            txtName.text = mUserInfoData.userid.ToString();
        }
        else
        {
            txtName.text = mUserInfoData.name.ToString();
        }
        if (string.IsNullOrEmpty(mUserInfoData.number))
        {
            txtNum.text = mUserInfoData.deviceid;
        }
        else
        {
            txtNum.text = mUserInfoData.number;
        }
    }
    
    public void OnLevelStatusChange(bool startLevel, int levelid, string levelname)
    {
        txtLevelInfo.text = startLevel ? string.Format("正在进行中") : string.Format("已完成");
    }
    public void OnPowerChange(float power)
    {
        imgPower.fillAmount = power;
    }


    public void OnPosStatusChange(UserPosStatus status)
    {
        //Debug.Log(status);
        switch (status)
        {
            case UserPosStatus.None:
                imgStatus.color = Color.gray;
                break;
            case UserPosStatus.DefaultScene:
                imgStatus.color = Color.white;
                break;
            case UserPosStatus.SceneIsLoading:
                imgStatus.color = Color.yellow;
                break;
            case UserPosStatus.SceneLoadedOver:
                imgStatus.color = Color.red;
                break;
            case UserPosStatus.InRuningScene:
                imgStatus.color = Color.green;
                break;
        }

    }

    public void OnHMDStatusChange(UserHMDStatus status)
    {
        switch(status)
        {
            case UserHMDStatus.PutOff:
                imgBg.color = Color.white;
                break;
            case UserHMDStatus.PutOn:
                imgBg.color = Color.green;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        txtName.color = mOverColor;
        txtNum.color = mOverColor;
    }
    public void OnPointerExit(PointerEventData data)
    {
        txtName.color = Color.white;
        txtNum.color = Color.white;
    }
    


    public void OnPointerClick(PointerEventData data)
    {
        mUserListPanel.OnUserClick(mUserInfoData);
    }

    	
}
