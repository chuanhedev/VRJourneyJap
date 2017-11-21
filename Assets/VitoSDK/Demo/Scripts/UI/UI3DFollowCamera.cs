using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI3DFollowCamera : MonoBehaviour {
    public static UI3DFollowCamera instance;
    private Transform mHead;
    public float mDistance=10;
    public bool mAlwaysFollow = false;

    private Transform mTransfrom;
    private bool hasInited = false;
    private Vector3 offset;
    void Awake()
    {
        instance = this;
        mTransfrom = transform;

        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void OnDestroy()
    {
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    void OnLevelWasLoaded()
    {
        SceneManager_sceneLoaded();
    }

    void SceneManager_sceneLoaded()
    {
        ResetPos();
    }
    // Use this for initialization
    void Start () {
        
	}

    public void ResetPos()
    {
        hasInited = false;
    }
    bool CheckHead()
    {
        if(mHead==null&&Camera.main!=null)
        {
            mHead = Camera.main.transform;
        }        
        if (mHead == null)
        {
            mHead = VRSwitchCameraRig.instance.mHead;
        }
        return mHead != null;
    }
    void Update()
    {
        if(CheckHead())
        {
            
            if(mAlwaysFollow)
            {
                mTransfrom.position = mHead.position + mHead.forward * mDistance;
                mTransfrom.forward = mHead.forward;
            }
            else
            {
                if(!hasInited)
                {                    
                    mTransfrom.position = mHead.position + mHead.forward * mDistance;
                    mTransfrom.forward = mHead.forward;
                    offset = mHead.position - mTransfrom.position;
                    hasInited = true;
                }else
                {
                    //mTransfrom.position = (mHead.position - offset);
                }
            }
            
        }
    }
}
