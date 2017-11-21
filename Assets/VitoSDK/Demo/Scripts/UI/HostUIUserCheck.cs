using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostUIUserCheck : MonoBehaviour {
    public Toggle toggleSavePass;
    public Button btnLogin;

    public InputField inputUserName;
    public InputField inputUserPass;

    public GameObject centerObj;
    public Animator mAnim;
    void OnEnable()
    {
        btnLogin.onClick.AddListener(OnBtnClick);
    }
    void OnDisable()
    {
        btnLogin.onClick.RemoveListener(OnBtnClick);
    }
	// Use this for initialization
	void Start () {
        string oldUserName=PlayerPrefs.GetString("UserName","");
        string oldUserPass = PlayerPrefs.GetString("UserPass", "");
        int savePass = PlayerPrefs.GetInt("SavePass",0);
        if(!string.IsNullOrEmpty( oldUserName))
        {
            inputUserName.text = oldUserName;
        }
        if(!string.IsNullOrEmpty(oldUserPass))
        {
            inputUserPass.text = oldUserPass;
        }
        if(savePass==1)
        {
            toggleSavePass.isOn = true;
        }else
        {
            toggleSavePass.isOn = false;
        }
	}

    void OnBtnClick()
    {
        string userName = inputUserName.text;
        string userPass = inputUserPass.text;
        if(string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(userPass))
        {

        }else
        {
            if(toggleSavePass.isOn)
            {
                PlayerPrefs.SetString("UserName",userName);
                PlayerPrefs.SetString("UserPass", userPass);
                PlayerPrefs.SetInt("SavePass", 1);
            }
            else
            {
                PlayerPrefs.SetInt("SavePass", 0);
            }
            LogicClient.instance.LoginWithUserName(userName,userPass,LoginCallback);
        }

    }
    void LoginCallback(bool isSuccess)
    {
        if(isSuccess)
        {
            centerObj.SetActive(false);
        }else
        {
            mAnim.Rebind();
            mAnim.Play("TipsShow",0);
        }
    }
}
