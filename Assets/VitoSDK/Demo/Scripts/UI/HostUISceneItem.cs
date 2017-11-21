using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.EventSystems;
//[RequireComponent(typeof(Button))]
public class HostUISceneItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{


    //private Button mBtn;
    public Image imgIconBG;
    public RawImage imgIcon;
    public Text txtTitle;
    public Text txtIntro;
    public Text txtTag1;
    public Text txtTag2;
    public string mSceneName;

    public Color mColor;
    private bool isVideo = false;
    private int videoIndex = 0;
    private string videoName = "";
    private List<Text> mTags=new List<Text>();
    public Image imgLeftBorder;

    void Awake()
    {
        //mBtn = GetComponent<Button>();
        //mBtn.onClick.AddListener(OnClick);
    }
    private void OnDestroy()
    {
       // mBtn.onClick.RemoveListener(OnClick);
    }


    void OnClick()
    {
        if(isVideo)
        {
            HostUIManager.instance.OnPlayVideo(videoName);
        }else
        {
            HostUIManager.instance.OnChangeScene(mSceneName);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        OnClick();
    }

     public  void OnPointerEnter(PointerEventData eventData )
    {
        txtTitle.color = mColor;
        txtIntro.color = mColor;
        imgLeftBorder.enabled = true;
        for(int i=0;i<mTags.Count;i++)
        {
            mTags[i].color = mColor;
        }
        //Debug.Log("OnPointerEnter");
    }

    public  void OnPointerExit(PointerEventData eventData)
    {
        txtTitle.color = Color.white;
        txtIntro.color = Color.white;
        imgLeftBorder.enabled = false;
        for (int i = 0; i < mTags.Count; i++)
        {
            mTags[i].color = Color.white;
        }
        //Debug.Log("OnPointerExit");
    }
    

    public void Init(VitoVideoConfigData data)
    {
        this.isVideo = true;
        this.videoName = data.videoName;
        txtTitle.text = data.title;
        txtIntro.text = data.intro;
        txtTag1.text = data.tags[0];
        mTags.Clear();
        mTags.Add(txtTag1);
        mTags.Add(txtTag2);
        txtTag2.text = data.tags[1];

        GameObject go = Instantiate<GameObject>(txtTag1.transform.parent.gameObject);
        go.transform.SetParent(txtTag1.transform.parent.parent);
        Vector3 pos = go.transform.localPosition;
        pos.z = 0;
        go.transform.localPosition = pos;
        go.transform.localScale = Vector3.one;
        mTags.Add(go.GetComponentInChildren<Text>());
        go.GetComponentInChildren<Text>().text = data.tags[2];
    }

    void ShowTextureCallBack(MyWWW www)
    {
        imgIcon.texture = www.tex;
        
    }

    public  void Init(VitoSceneConfigData data)
    {
        this.isVideo = false;
        txtTitle.text = data.title;
        txtIntro.text = data.intro;        
        mSceneName = data.sceneName;
        string[] tags = data.tags;
        for (int i=0;i<tags.Length;i++)
        {
            if(i>=2)
            {
                GameObject go=Instantiate<GameObject>(txtTag1.transform.parent.gameObject);
                go.transform.SetParent(txtTag1.transform.parent.parent);
                Vector3 pos = go.transform.localPosition;
                pos.z = 0;
                go.transform.localPosition = pos;
                go.transform.localScale = Vector3.one;
                mTags.Add(go.GetComponentInChildren<Text>());
                go.GetComponentInChildren<Text>().text = tags[i];
            }else
            {
                if(i==0)
                {
                    mTags.Add(txtTag1);
                    txtTag1.text = tags[0];
                }else if(i==1)
                {
                    mTags.Add(txtTag2);
                    txtTag2.text = tags[1];
                }
            }
        }
    }


        

}



