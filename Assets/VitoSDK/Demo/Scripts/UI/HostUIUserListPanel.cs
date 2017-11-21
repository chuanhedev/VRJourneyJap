using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostUIUserListPanel : MonoBehaviour {
    public static HostUIUserListPanel instance;
    public Transform userItemParent;
    public GameObject userItemPrefab;

    public GameObject detailInfoPanel;
    public Button btnObserver;
    public Button btnCancle;
    public InputField inputName;
    public InputField inputNum;
    public Text txtName;
    public Text txtNum;
    public Text txtUserCollectedData;

    public Text txtSumPeople;

    private UserInfoData mCurUserInfoData;


    void Awake()
    {
        instance = this;
    }
    void OnEnable()
    {
        btnObserver.onClick.AddListener(OnObserverClick);
        btnCancle.onClick.AddListener(OnCancleClick);
    }


    void OnDisable()
    {
        btnObserver.onClick.RemoveListener(OnObserverClick);
        btnCancle.onClick.RemoveListener(OnCancleClick);
    }
    void Start()
    {
        UserInfoManager.instance.OnUserCountChange = RefreshUserCount;
    }
    public void RefreshUserCount(int sum,int cur)
    {
        txtSumPeople.text = "人数统计:" + cur+"/"+ sum;
    }

    public void OnUserLogin(UserInfoData userInfo)
    {
#if UNITY_ANDROID
        DebugHealper.Log("OnUserLogin:"+userInfo.userid);
#endif
        GameObject go=Instantiate<GameObject>(userItemPrefab);
        go.transform.SetParent(userItemParent);
        Vector3 oldPos = go.transform.localPosition;
        oldPos.z = 0;
        go.transform.localPosition = oldPos;
        go.transform.localScale = Vector3.one;
        go.GetComponent<HostUIUserInfoItem>().Init(userInfo,this);
    }
    //public void OnUserRelogin(UserInfoData userInfo)
    //{
    //    userInfo.mUIItem.Init(userInfo,this);
    //}
    public void OnUserLogout(UserInfoData userInfo)
    {
        
    }

    public void OnUserClick(UserInfoData userInfoData)
    {
        detailInfoPanel.SetActive(true);
        mCurUserInfoData = userInfoData;
        if(userInfoData!=null)
        {            
            inputName.text = userInfoData.name;
            inputNum.text = userInfoData.number;
        }
        List<CDScore> scoreData = DataCollectionManager.instance.GetCollectedScoreData(userInfoData.deviceid);
        List<CDStar> starData = DataCollectionManager.instance.GetCollectedStarData(userInfoData.deviceid);
        string content = "";
        content += "得分数据:\n";
        if (scoreData!=null)
        {
            
            foreach( var data in scoreData)
            {
                content += string.Format("关卡:{0}\t 总分：{1}\t阶段1分数：{2}\t阶段2分数：{3}\t阶段3分数{4}\t",data.levelName, data.sumScore, data.score1, data.score2, data.score3);
            }
        }
        content += "\n星级数据:\n";
        if (starData!=null)
        {
            
            foreach(var data in starData)
            {
                content += string.Format("关卡:{0}\t 星级：{1}\t时长{2}\t", data.levelName, data.starGrade, data.sumTime);
            }
        }


        txtUserCollectedData.text = content;
    }

    public void OnObserverClick()
    {
        HostUIManager.instance.OnLookPlayer(mCurUserInfoData);
        detailInfoPanel.SetActive(false);
    }
    public void OnCancleClick()
    {
        if (!string.IsNullOrEmpty(inputName.text) || (!string.IsNullOrEmpty(inputNum.text)))
        {
            mCurUserInfoData.name = inputName.text;
            mCurUserInfoData.number = inputNum.text;

            UserInfoManager.instance.OnRefreshUserInfo(mCurUserInfoData);
        }

        detailInfoPanel.SetActive(false);
    }

    void Update()
    {
        if( detailInfoPanel.activeInHierarchy)
        {
            if(inputName.isFocused||!string.IsNullOrEmpty(inputName.text))
            {
                inputName.placeholder.enabled = false;
            }else
            {
                inputName.placeholder.enabled = true;
            }

            if (inputNum.isFocused || !string.IsNullOrEmpty(inputNum.text))
            {
                inputNum.placeholder.enabled = false;
            }
            else
            {
                inputNum.placeholder.enabled = true;
            }

        }
    }
}
