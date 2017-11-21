using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VitoPluginPlayVideo : MonoBehaviour {
    public static VitoPluginPlayVideo instance;
    private string willPlayVideoIndex = "0";

    void Update()
    {
        if (VitoPlugin.CT == CtrlType.Admin && VideoManager.instance != null && VideoManager.instance._mediaPlayer != null && !videoSliderDowned)
        {
            HostUIVideoCtrl.instance.UpdateProcess(VideoManager.instance._mediaPlayer.Control.GetCurrentTimeMs(), VideoManager.instance._mediaPlayer.Info.GetDurationMs());
        }
    }
    public void SetStatus(bool play) 
    {
        if (VideoManager.instance != null)
        {
            if (VideoManager.instance._mediaPlayer != null && play != VideoManager.instance._mediaPlayer.Control.IsPlaying())
            {
                if (play)
                {
                    VideoManager.instance._mediaPlayer.Play();
                }
                else
                {
                    VideoManager.instance._mediaPlayer.Pause();
                }
            }
        }
    }

    void Awake()
    {
        instance = this;
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    void OnLevelWasLoaded()
    {
        SceneManager_sceneLoaded();
    }
    void Start()
    {
        VitoPlugin.RegisterActionEvent("PlayVideo", (actionName, parameter, deviceId) =>
        {
            if (HostUIManager.instance != null)
            {
                HostUIManager.instance.OnResponsePlayVideo();
            }
            if (SceneManager.GetActiveScene().name != "PlayVideo" || LogicClient.instance.isCheckConnection)
            {
                willPlayVideoIndex = parameter;
                VitoPluginLoadScene.instance.OnResponseChangeScene("PlayVideo");
            }
            else
            {
                OnResponsePlayVideo(parameter);
            }
        });

        VitoPlugin.RegisterActionEvent("VideoSlider", (actionName, parameter, deviceId) =>
        {
            float value = 0;
            if (float.TryParse(parameter, out value))
            {
                if (VideoManager.instance != null && VideoManager.instance._mediaPlayer != null)
                {
                    VideoManager.instance._mediaPlayer.Control.Seek(VideoManager.instance._mediaPlayer.Info.GetDurationMs() * value);
                }
            }
        });
    }
    void OnDestroy()
    {
        //SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded()
    {
        if(SceneManager.GetActiveScene().name=="PlayVideo")
        {
            OnResponsePlayVideo(willPlayVideoIndex);
        }
    }


    #region 视频进度控制功能
    bool videoSliderDowned = false;
    public void VideoSliderDown()
    {
        videoSliderDowned = true;
    }

    public void VideoSliderUp()
    {
        StartCoroutine(VideoSliderAction(HostUIVideoCtrl.instance.getProcess()));
    }

    IEnumerator VideoSliderAction(float videoSliderValue)
    {
        float timer = 0;
        float sumTime = 0.3f;
        while (timer < sumTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        VitoPlugin.RequestActionEvent("VideoSlider", videoSliderValue.ToString());
         timer = 0;
         sumTime = 0.3f;
        while (timer < sumTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        videoSliderDowned = false;
    }
    #endregion

    public void OnRequestPlayVideo(string videoName)
    {
        VitoPlugin.RequestActionEvent("PlayVideo", videoName, true);
    }


    public void OnResponsePlayVideo(string parameter)
    {
        if (VideoManager.instance != null)
        {
            int videoindex = 0;
            int.TryParse(parameter, out videoindex);
            string videopath = "";

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                videopath = VitoSDKConfig.instance.videopath + "\\" + parameter;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                videopath = VitoSDKConfig.instance.videopath + "/" + parameter;
            }
            VideoManager.instance.PlayByPath(videopath);
        }
    }
}
