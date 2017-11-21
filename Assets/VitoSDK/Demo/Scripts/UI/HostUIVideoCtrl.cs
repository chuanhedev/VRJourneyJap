using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostUIVideoCtrl : MonoBehaviour {
    public static HostUIVideoCtrl instance { get; set; }
    void Awake()
    {
        instance = this;
        Hide();
    }

    public Slider mProcess;
    public Toggle mTogglePause;
    public Toggle mToggleContinue;
    public Text mTime;
    public GameObject mCenterObj;
    private float sumTime = 0;
    void OnEnable()
    {
        mTogglePause.onValueChanged.AddListener(OnPause);
        mToggleContinue.onValueChanged.AddListener(OnContinue);
        mProcess.onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(float value)
    {
        float tempSumTime = sumTime;

        float tempCurTime = sumTime * mProcess.value;
        tempSumTime /= 1000 * 60;
        tempCurTime /= 1000 * 60;
        mTime.text = string.Format("{0:00}:{1:00}/{2:00}:{3:00}", Mathf.FloorToInt(tempCurTime), Mathf.FloorToInt((tempCurTime - Mathf.FloorToInt(tempCurTime)) * 60), Mathf.FloorToInt(tempSumTime), Mathf.FloorToInt((tempSumTime - Mathf.FloorToInt(tempSumTime)) * 60));
    }
    void OnDisable()
    {
        mTogglePause.onValueChanged.RemoveListener(OnPause);
        mToggleContinue.onValueChanged.RemoveListener(OnContinue);
        mProcess.onValueChanged.RemoveListener(OnValueChange);
    }

    public void Show()
    {
        if(!mCenterObj.activeSelf)
        {
            mCenterObj.SetActive(true);

        }
        
    }
    public void Hide()
    {
        if(mCenterObj.activeSelf)
        {
            mCenterObj.SetActive(false);
        }
        
    }

    public void UpdateProcess(float curTime,float sumTime)
    {
        this.sumTime = sumTime;
        float rtn= curTime / sumTime;
        if(rtn>0&&rtn<1)
        {
            mProcess.value = Mathf.Clamp01(rtn);
        }                
        
    }

    public float getProcess()
    {
        return mProcess.value;
    }

    public void OnContinue(bool isOn)
    {
        if(isOn)
        {

        }
    }
    public void OnPause(bool isOn)
    {
        if(isOn)
        {

        }
    }
    
    	
}
